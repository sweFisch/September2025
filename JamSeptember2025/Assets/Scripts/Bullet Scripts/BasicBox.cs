using UnityEngine;

public class BasicBox : MonoBehaviour
{

    public float lifeTime = 10.0f;
    public bool willDie = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0.0f && willDie == true){

            Destroy(gameObject);
        }
    }
}
