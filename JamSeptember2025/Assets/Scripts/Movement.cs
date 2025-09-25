using System;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    float jumpPower = 36f;
    float fallAcceleration = 110f;
    float maxFallSpeed = 30f;
    [SerializeField] float _groundingForce = -1.5f;
    [SerializeField] float _groundedDistance = 0.05f;

    //[Header("Handle X movement")]
    float groundDeceleration = 60f;
    float airDeceleration = 30f;
    float maxSpeed = 14f;
    float acceleration = 120f;


    Vector2 _frameVelocity; // Velocity vector each frame

    Rigidbody2D _rb;
    CapsuleCollider2D _capsuleCollider;

    // Jump stuff
    bool _bufferedJumpUsable = false;
    [SerializeField] float _jumpBuffer = 0.2f;

    bool _coyoteUsable = false;
    [SerializeField] float _coyoteTime = 0.1f;


    float _time = 0f; // timer for jump
    private float _frameLeftGrounded = float.MinValue;
    private bool _isGrounded;

    private bool _cachedQueryStartInColliders; // bool to capture orignial settings

    // Jump extra input data
    bool _jumpToConsume = false;
    bool _endedJumpEarly = false;
    float _timeJumpWasPressed = -555.0f;

    // Input data
    Vector2 inputMove; // input vector
    bool _jumpIsPressed = false;

    Stats _stats;

    public event EventHandler OnJumpImpact;

    public bool FacingRight { get; private set; }


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _capsuleCollider = _rb.GetComponent<CapsuleCollider2D>();

        _stats = gameObject.GetComponent<Stats>();

        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
    }

    public void OnMove(InputValue value)
    {
        inputMove = value.Get<Vector2>();

        if (inputMove.x > 0.1f)
        {
            FacingRight = true;
        }
        else if (inputMove.x < -0.1f)
        {
            FacingRight = false;
        }
    }

    public void OnJump(InputValue button)
    {
        if (button.isPressed)
        {
            _jumpToConsume = true;
            _jumpIsPressed = true;
            _timeJumpWasPressed = _time;
        }
        //Debug.Log(button);
        if (!button.isPressed)
        {
            // Button release (This needs a press and release modifier on the input system!)
            _jumpIsPressed = false;
        }
    }


    void Update()
    {
        
        _time += Time.deltaTime;

        // Now using a Player Input and the send messages 

    }

    private void FixedUpdate()
    {
        GetStats();

        CheckCollisions();

        HandleJump();
        HandleDirection();
        HandleGravity();

        ApplyMovement();
    }

    private void GetStats()
    {
        jumpPower = _stats.JumpPowerValue;
        maxFallSpeed = _stats.MaxFallSpeedValue;
        fallAcceleration = _stats.FallAccelerationValue;
        groundDeceleration = _stats.GroundDecelerationValue;
        airDeceleration = _stats.AirDecelerationValue;
        maxSpeed = _stats.MaxSpeedValue;
        acceleration = _stats.AccelerationValue;
    }

    private void CheckCollisions()
    {
        // Check for ground and handle that

        Physics2D.queriesStartInColliders = false; // Dont detect colliders if they start inside a colider

        bool groundHit = Physics2D.CapsuleCast(_capsuleCollider.bounds.center, _capsuleCollider.size, _capsuleCollider.direction, 0f, Vector2.down, _groundedDistance); // possible to usea layermask as well
        bool ceilingHit = Physics2D.CapsuleCast(_capsuleCollider.bounds.center, _capsuleCollider.size, _capsuleCollider.direction, 0f, Vector2.up, _groundedDistance);

        if (ceilingHit)
        {
            _frameVelocity.y = Mathf.Min(0f, _frameVelocity.y);
        }

        // landed on the Ground
        if (!_isGrounded && groundHit)
        {
            _isGrounded = true;
            _coyoteUsable = true;
            _bufferedJumpUsable = true;

            // ended early false
            // Invoke possible event for particle ect
            OnJumpImpact?.Invoke(this, EventArgs.Empty);
        }
        // Left the ground
        else if (_isGrounded && !groundHit)
        {
            _isGrounded = false;
            _frameLeftGrounded = _time; // used for coyote time

            // Invoke possible event for particle ect
        }

        Physics2D.queriesStartInColliders = _cachedQueryStartInColliders; // set the flag back to what was saved in awake
    }

    


    // Properties with getters that run the bool check
    private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _jumpBuffer;
    private bool CanUseCoyote => _coyoteUsable && !_isGrounded && _time < _frameLeftGrounded + _coyoteTime;

    private void HandleJump()
    {
        // Check for short jump press - early release
        if (!_endedJumpEarly && !_isGrounded && !_jumpIsPressed && _rb.linearVelocity.y > 0) 
        {
            //Debug.Log("Ended Jump Early!");
            _endedJumpEarly = true; 
        }

        // Check for buffered jump input else return
        if (!_jumpToConsume && !HasBufferedJump) {return; }

        // Check Coyote time
        // Check grounded
        if (_isGrounded || CanUseCoyote) // Or Coyote jump (_isGroudnded || CanUseCoyote)
        {
            //if (CanUseCoyote) { Debug.Log("CoyoteTime used"); }
            Executejump();
        }

        _jumpToConsume = false;
    }


    private void Executejump()
    {
        _endedJumpEarly = false;
        _timeJumpWasPressed = 0;
        _bufferedJumpUsable = false;
        _coyoteUsable = false;

        // update Velocity
        _frameVelocity.y = jumpPower; 

        // Possible Event Action Jumped
    }

    private void HandleDirection()
    {
        if (Mathf.Abs(inputMove.x) < 0.1f) // if no real input decelerate
        {
            // use ground or air value

            float deceleration = _isGrounded ? groundDeceleration : airDeceleration;

            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x,0,deceleration * Time.fixedDeltaTime);
        }
        else
        {


            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, inputMove.x * maxSpeed, acceleration * Time.fixedDeltaTime);
        }
    }

    private void HandleGravity()
    {
        // if grounded add force down for handling slopes
        if (_isGrounded && _frameVelocity.y <= 0f)
        {
            _frameVelocity.y = _groundingForce;
        }
        else
        {
            float inAirGravity = fallAcceleration;
            // ended jump early ? extra modifier for gravity so it gets aborted quicker, and then falls like normal
            if (_endedJumpEarly && _frameVelocity.y > 0f)
            {
                float jumpEndedEarlyModifier = 3f;
                inAirGravity *= jumpEndedEarlyModifier;
            }
            // make a growing velocity down towards the maxFallSpeed
            _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -maxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            
        }
        // else in air, handle gravity depending if jumping up or falling

    }

    private void ApplyMovement()
    {
        _rb.linearVelocity = _frameVelocity;
    }


    public Vector2 GetPlayerVelocity()
    {
        return _frameVelocity;
    }
}
