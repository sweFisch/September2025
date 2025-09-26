using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class ControlCam : MonoBehaviour
{
    //private Camera _camera;

    public List<GameObject> gameObjectList;

    public float _bufferAroundObjects = 8f;

    public float _smoothTime = 0.2f;
    public float _smoothTimeSize = 0.3f;
    private Vector3 _velocity = Vector3.zero;

    private float _sizeVelocity;

    //Bounds _levelBounds;
    //[SerializeField] BoxCollider2D _boxColliderBounds;

    [SerializeField] Bounds _bounds;

    void Start()
    {
        //_camera = Camera.main;
        //_levelBounds = _boxColliderBounds.bounds;
    }

    private (Vector3 center, float size) CalculateOrthoSize()
    {
        _bounds = new Bounds(Vector3.zero,Vector3.one);

        if(gameObjectList.Count > 0)
        {
            _bounds = new Bounds(gameObjectList[0].transform.position,Vector3.one);

            foreach (GameObject go in gameObjectList)
            {
                if (go != null)
                {
                    _bounds.Encapsulate(go.transform.position);
                }
            }
        }

        _bounds.Expand(_bufferAroundObjects);

        //_bounds = ClampBounds(_bounds);

        float vertical = _bounds.size.y;
        float horizontal = _bounds.size.x;

        var size = Mathf.Max(horizontal, vertical) * 0.5f; // Get the half size for ortographic camera  OBS testing outside 0.5
        var center = _bounds.center + new Vector3(0, 0, -10); // get center and offset so camera is not at zero

        return (center, size);

    }

    private Bounds CalculateBoundsOfPlayers()
    {
        Bounds bounds = new Bounds(Vector3.zero, Vector3.one);

        if (gameObjectList.Count > 0)
        {
            bounds = new Bounds(gameObjectList[0].transform.position, Vector3.one);

            foreach (GameObject go in gameObjectList)
            {
                if (go != null)
                {
                    bounds.Encapsulate(go.transform.position);
                }
            }
        }

        return bounds;
    }

    private Bounds ClampBounds(Bounds boundToClamp)
    {
        //Vector3 pointMax = new Vector3(boundToClamp.extents.x + boundToClamp.center.x, boundToClamp.extents.y + boundToClamp.center.y, 0f);
        //Vector3 pointMin = new Vector3(-boundToClamp.extents.x + boundToClamp.center.x, -boundToClamp.extents.y + boundToClamp.center.y, 0f);
        
        Vector3 pointMax = boundToClamp.max;
        Vector3 pointMin = boundToClamp.min;

        Debug.DrawLine(pointMin,pointMax,Color.red);
        

        pointMax = ReturnPointInsideLevelBounds(pointMax);
        pointMin = ReturnPointInsideLevelBounds(pointMin);

        Debug.DrawLine(pointMin, pointMax, Color.green);

        Bounds bounds = new Bounds(pointMax,Vector3.zero);
        bounds.Encapsulate(pointMin);

        return bounds;
    }

    private Vector3 ReturnPointInsideLevelBounds(Vector3 pos)
    {
        // Clamp pos x y to level bounds
        //pos = new Vector3(Mathf.Clamp(pos.x,
        //    -_levelBounds.extents.x + _levelBounds.center.x,
        //    _levelBounds.extents.x + _levelBounds.center.x),
        //    Mathf.Clamp(pos.y,
        //    -_levelBounds.extents.y + _levelBounds.center.y,
        //    _levelBounds.extents.y + _levelBounds.center.y),
        //    0f);


        return pos;
    }

    void LateUpdate()
    {
        
        //Camera.main.orthographicSize = CalculateOrthoSize().size;
        //Vector3 preferedCameraPos = CalculateOrthoSize().center;
        
        _bounds = CalculateBoundsOfPlayers();
        _bounds.Expand(_bufferAroundObjects);

        var center = _bounds.center + new Vector3(0, 0, -10);
        Vector3 preferedCameraPos = center;

        float targetSize = _bounds.extents.y;
        if (_bounds.extents.x > _bounds.extents.y * Camera.main.aspect)
        {
            Camera.main.orthographicSize = _bounds.extents.x / Camera.main.aspect;
        }
        else
        {
            Camera.main.orthographicSize = _bounds.extents.y;
        }
        Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, targetSize, ref _sizeVelocity, _smoothTimeSize);


        // Camera Shake
        if (Time.time < shakeTotalDurationTimer)
        {
            ApplyCameraShake(preferedCameraPos);
        }

        // Smoothing camera Position
        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, preferedCameraPos, ref _velocity, _smoothTime);
    }
    
    // Camera Shake
    float shakeTotalDurationTimer;
    float stepTimer = 0;
    float shakeSteps = 0.02f;
    float magnitude;

    public void CameraShake(float inputMagnitude, float duration)
    {
        magnitude = inputMagnitude;
        shakeTotalDurationTimer = Time.time + duration;
        stepTimer = Time.time + shakeSteps;
    }
    private void ApplyCameraShake(Vector3 actualPosition)
    {
        if (Time.time > stepTimer) 
        {
            Vector3 newPos = actualPosition + (Vector3)Random.insideUnitCircle.normalized * magnitude;
            Camera.main.transform.position = newPos;

            stepTimer = Time.time + shakeSteps;
            magnitude *= 0.8f;
        }
        
    }

    public void AddTrackingGameObject(GameObject newGameObject)
    {
        if (gameObjectList.Contains(newGameObject)) { return; }

        gameObjectList.Add(newGameObject);
    }

    public void RemoveTrackingGameObject(GameObject removeGameObject) 
    {
        gameObjectList.Remove(removeGameObject);
    }

    public void ClearTackingGameObjectList()
    {
        gameObjectList.Clear();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_bounds.center, _bounds.size);
    }
}
