using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Bullet[] bullets = GetComponentsInChildren<Bullet>();
        if (bullets != null)
        {
            transform.DetachChildren();
            foreach (Bullet bullet in bullets) { bullet.Fire(0); }      
        }
        Destroy(gameObject, timer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        
    }
}
