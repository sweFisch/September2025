using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class ControlCam : MonoBehaviour
{
    //private Camera _camera;

    public List<GameObject> gameObjectList;

    public float _bufferAroundObjects = 2f;

    public float _smoothTime = 0.3f;
    private Vector3 _velocity = Vector3.zero;

    Bounds _levelBounds;
    [SerializeField] BoxCollider2D _boxColliderBounds;

    void Start()
    {
        //_camera = Camera.main;
        _levelBounds = _boxColliderBounds.bounds;
    }

    private (Vector3 center, float size) CalculateOrthoSize()
    {
        Bounds bounds = new Bounds(Vector3.zero,Vector3.one);

        if(gameObjectList.Count > 0)
        {
            bounds = new Bounds(gameObjectList[0].transform.position,Vector3.one);

            foreach (GameObject go in gameObjectList)
            {
                if (go != null)
                {
                    bounds.Encapsulate(go.transform.position);
                }
            }
        }

        bounds.Expand(_bufferAroundObjects);

        bounds = ClampBounds(bounds);

        float vertical = bounds.size.y;
        float horizontal = bounds.size.x;

        print($"{vertical} : {horizontal}   --- bounds SIZE");

        var size = Mathf.Max(horizontal, vertical) * 0.5f; // Get the half size for ortographic camera  OBS testing outside 0.5
        var center = bounds.center + new Vector3(0, 0, -10); // get center and offset so camera is not at zero

        //Test level bounds
        //vertical = _levelBounds.extents.y;
        //horizontal = _levelBounds.extents.x;


        //center = _levelBounds.center + new Vector3(0, 0, -10);
        //size = vertical * 0.5f; //Mathf.Max(horizontal, vertical) * 0.5f;

        return (center, size);

    }

    private Bounds ClampBounds(Bounds boundToClamp)
    {
        //Vector3 pointMax = new Vector3(boundToClamp.extents.x + boundToClamp.center.x, boundToClamp.extents.y + boundToClamp.center.y, 0f);
        //Vector3 pointMin = new Vector3(-boundToClamp.extents.x + boundToClamp.center.x, -boundToClamp.extents.y + boundToClamp.center.y, 0f);
        
        Vector3 pointMax = new Vector3(boundToClamp.size.x * 0.5f, boundToClamp.size.y * 0.5f, 0f);
        Vector3 pointMin = new Vector3(-boundToClamp.size.x * 0.5f, -boundToClamp.size.y * 0.5f, 0f);

        print(pointMax);
        print(pointMin);

        pointMax = ReturnPointInsideLevelBounds(pointMax);
        pointMin = ReturnPointInsideLevelBounds(pointMin);
        
        print(pointMax);
        print(pointMin);

        Bounds bounds = new Bounds(pointMax, Vector3.one);
        bounds.Encapsulate(pointMin);

        return bounds;
    }

    private Vector3 ReturnPointInsideLevelBounds(Vector3 pos)
    {
        // Clamp pos x y to level bounds
        pos = new Vector3(Mathf.Clamp(pos.x,
            -_levelBounds.extents.x + _levelBounds.center.x,
            _levelBounds.extents.x + _levelBounds.center.x),
            Mathf.Clamp(pos.y,
            -_levelBounds.extents.y + _levelBounds.center.y,
            _levelBounds.extents.y + _levelBounds.center.y),
            0f);
        return pos;
    }

    void LateUpdate()
    {
        //var (center, size) = CalculateOrthoSize();
        Camera.main.orthographicSize = CalculateOrthoSize().size;
        Vector3 preferedCameraPos = CalculateOrthoSize().center;

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
    public void ApplyCameraShake(Vector3 actualPosition)
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
}
