using UnityEngine;

public class UI_StartScreenAnimateHead : MonoBehaviour
{

    private float offset;
    private float IncreaseAmount;
    public float multiplier = 36.0f;
    public float speed = 1.0f;
    public bool useOffset = true;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
       offset = Random.Range(0.0f,360.0f);

       if (useOffset == false){

        offset = 0.0f;
       }
        
    }

    // Update is called once per frame
    void Update()
    {

       
        transform.Translate(Vector3.up * Mathf.Cos(IncreaseAmount * speed + offset) * Time.deltaTime * multiplier);
        transform.Translate(Vector3.left * Mathf.Sin(IncreaseAmount * speed + offset) * Time.deltaTime * multiplier);

        IncreaseAmount += 1 * Time.deltaTime;


    }
}
