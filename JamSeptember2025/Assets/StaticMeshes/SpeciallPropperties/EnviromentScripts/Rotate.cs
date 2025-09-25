using UnityEngine;

public class Rotate : MonoBehaviour
{

    public bool clockWise;
    public float speed = 30.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (!clockWise){

            speed = speed * -1;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        

        transform.Rotate(0.0f, 0.0f, speed * Time.deltaTime, Space.Self);
        
    }
}
