using UnityEngine;

public class AuraEffect : MonoBehaviour
{
    public Color color = Color.white;
    public float radius = 5;
    public Stats.StatusEffects effect;
    public float AuraDurration = 5;
    public GameObject endParticle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        transform.localScale = Vector3.zero;
    }
    void Start()
    {
        Destroy(gameObject,AuraDurration);
    }

    float currentScale;
    // Update is called once per frame
    void Update()
    {
        currentScale = Mathf.Lerp(currentScale, radius, 0.25f);
        transform.localScale = new Vector3(currentScale, currentScale, currentScale); ;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Stats targetStats = collision.GetComponent<Stats>();
        if (targetStats != null) 
        {
            targetStats.BuffPicker(effect, 0.1f);
        }
    }

    private void OnDestroy()
    {
        GameObject endparticleInstance = Instantiate(endParticle,transform.position,Quaternion.identity);
        ParticleSystem ps = endparticleInstance.GetComponent<ParticleSystem>();
        var main = ps.main;                                                        //we need to get main this way because unity is stupid
        main.startColor = new Color (color.r,color.g,color.b,main.startColor.color.a);
    }
}
