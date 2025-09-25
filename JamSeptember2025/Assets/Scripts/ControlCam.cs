using System.Collections.Generic;
using UnityEngine;

public class ControlCam : MonoBehaviour
{
    //private Camera _camera;

    public List<GameObject> gameObjectList;

    public float _bufferAroundObjects = 2f;

    public float _smoothTime = 0.3f;
    private Vector3 _velocity = Vector3.zero;

    void Start()
    {
        //_camera = Camera.main;
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

        float vertical = bounds.size.y;
        float horizontal = bounds.size.x;

        var size = Mathf.Max(horizontal, vertical) * 0.6f; // Get the half size for ortographic camera  OBS testing outside 0.5
        var center = bounds.center + new Vector3(0, 0, -10); // get center and offset so camera is not at zero

        return (center, size);

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
