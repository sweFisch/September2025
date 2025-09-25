using System;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public enum StatusEffects
    {
        SpeedUp = 0,
        SpeedDown = 1,
    }

    [SerializeField] Sprite _bloodStain;
    [SerializeField] Sprite[] _playerSprites;
    [SerializeField] int _index = 0;
    [SerializeField] SpriteRenderer _spriteRenderer;

    #region Movement Values
    // Movement Sats
    [Header("Movment Base Stats")]
    [SerializeField] float jumpPowerBase = 36f;
    [SerializeField] float maxFallSpeedBase = 30f;
    [SerializeField] float fallAccelerationBase = 110f;
    [SerializeField] float groundDecelerationBase = 60f;
    [SerializeField] float airDecelerationBase = 30f;
    [SerializeField] float maxSpeedBase = 14f;
    [SerializeField] float accelerationBase = 120f;

    // Movement Mods
    [Header("Movment Values that gets applied")]
    public float JumpPowerValue { get; private set; }
    public float MaxFallSpeedValue { get; private set; }
    public float FallAccelerationValue { get; private set; }
    public float GroundDecelerationValue { get; private set; }
    public float AirDecelerationValue { get; private set; }
    public float MaxSpeedValue { get; private set; }
    public float AccelerationValue { get; private set; }


    [Header("Movment Modifier Precent")]
    [SerializeField] float jumpPowerMod = 1f;
    [SerializeField] float maxFallSpeedMod = 1f;
    [SerializeField] float fallAccelerationMod = 1f;
    [SerializeField] float groundDecelerationMod = 1f;
    [SerializeField] float airDecelerationMod = 1f;
    [SerializeField] float maxSpeedMod = 1f;
    [SerializeField] float accelerationMod = 1f;
    #endregion

    // Health 
    [SerializeField] Health _healthScript; // use functions for setting and getting health
    //[SerializeField] float currentHealth = 100;
    [SerializeField] float maxHealth = 100;

    private void Start()
    {
        SetSprite();

        if (_healthScript == null)
        {
            _healthScript = gameObject.GetComponent<Health>();
        }
        _healthScript.SetMaxHealth(maxHealth);
    }

    private void SetSprite()
    {
        _spriteRenderer.sprite = _playerSprites[_index];
    }

    public void SetSprite(int index)
    {
        _index = index;
        SetSprite();
    }

    public void SetBloodSprite()
    {
        _spriteRenderer.sprite = _bloodStain;
    }


    float moveSpeedBuffTimer;
    public void AddMoveSpeedBuff(float buffDuration)
    {
        print("used speed");
        moveSpeedBuffTimer = Math.Max(moveSpeedBuffTimer, buffDuration) + Time.time;
    }

    public void BuffPicker(StatusEffects buff, float buffDuration)
    {
        switch (buff)
        {
            case StatusEffects.SpeedUp:
                AddMoveSpeedBuff(buffDuration);
                break;
            case StatusEffects.SpeedDown:

                break;
            default:
                break;
        }
    }


    private void Update()
    {
        // Movemet Reset mod
        JumpPowerValue = jumpPowerBase;
        MaxFallSpeedValue = maxFallSpeedBase;
        FallAccelerationValue = fallAccelerationBase;
        GroundDecelerationValue = groundDecelerationBase;
        AirDecelerationValue = airDecelerationBase;
        MaxSpeedValue = maxSpeedBase;
        AccelerationValue = accelerationBase;


        if ( Time.time < moveSpeedBuffTimer) { MaxSpeedValue = maxSpeedBase * maxSpeedMod; } // value = base * mod
        

    }
}
