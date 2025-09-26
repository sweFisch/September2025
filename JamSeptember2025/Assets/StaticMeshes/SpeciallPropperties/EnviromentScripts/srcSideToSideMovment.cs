using UnityEngine;

public class srcSideToSideMovment : MonoBehaviour
{
    public float speed = 2.5f;
    public bool horisontal = true;
    public float timeTillSwitchDirection = 2.5f;
    public bool initialDirection = false;
    private float timeTillSwitchReset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        timeTillSwitchReset = timeTillSwitchDirection;

        if(!horisontal){

            speed = speed * -1;

        }
        timeTillSwitchDirection = timeTillSwitchDirection / 2;
        
    }

    // Update is called once per frame
    void Update()
    {



        if(horisontal){
            if(!initialDirection){
        transform.Translate(speed * Time.deltaTime,0.0f,0.0f);


        }

        else if(initialDirection){
        transform.Translate(speed * Time.deltaTime * -1.0f,0.0f,0.0f);

        }
        


        }

        else if(!horisontal){
            
            if(!initialDirection){
        transform.Translate(0.0f,speed * Time.deltaTime,0.0f);


            }

        else if(initialDirection){
        transform.Translate(0.0f,speed * Time.deltaTime * -1.0f,0.0f);

            }



        }

                timeTillSwitchDirection -= Time.deltaTime;
        if (timeTillSwitchDirection <= 0.0f){

            initialDirection = !initialDirection;
            timeTillSwitchDirection = timeTillSwitchReset;

            }
    }
}
