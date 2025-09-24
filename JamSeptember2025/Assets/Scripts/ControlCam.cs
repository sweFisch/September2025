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
        //_camera.transform.position = center;

        // Smoothing camera
        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, CalculateOrthoSize().center, ref _velocity, _smoothTime);

    }

    public void AddTrackingGameObject(GameObject newGameObject)
    {
        gameObjectList.Add(newGameObject);
    }

    public void RemoveTrackingGameObject(GameObject removeGameObject) 
    {
        gameObjectList.Remove(removeGameObject);
    }
}
