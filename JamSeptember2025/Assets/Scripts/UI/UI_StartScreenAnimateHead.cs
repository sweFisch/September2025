using UnityEngine;

public class UI_StartScreenAnimateHead : MonoBehaviour
{

    private float offset;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

       offset = Random.Range(0.0f,360.0f);
        
    }

    // Update is called once per frame
    void Update()
    {

       
        transform.Translate(Vector3.up * Mathf.Cos(Time.time + offset ) * Time.deltaTime * 36.0f);
        transform.Translate(Vector3.left * Mathf.Sin(Time.time + offset) * Time.deltaTime * 36.0f);
    }
}
