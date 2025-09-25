using Unity.VisualScripting;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] Spawnables;
    [SerializeField] float timeToRespawn = 5;
    [SerializeField] float leashRange = 5;
    [SerializeField] bool respawning = true;
    float currentTimer;
    bool counting;
    GameObject currentItem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnItem();
    }

    // Update is called once per frame
    void Update()
    {
        if (!respawning) { return; }
        Nullify();

        if (currentItem == null && currentTimer < Time.time)
        {
            if (!counting) { currentTimer = Time.time + timeToRespawn; counting = true; }
            else { SpawnItem(); counting = false; }
        }

    }

    private void Nullify()
    {
        if (currentItem != null)
        {
            if (Vector2.Distance(transform.position, currentItem.transform.position) > leashRange) { currentItem = null; return; }
            if (currentItem.GetComponent<Item>().owned) { currentItem = null; return; }
        }
    }

    void SpawnItem()
    {
        currentItem = Instantiate(Spawnables[Random.Range(0, Spawnables.Length)], transform.position, Quaternion.identity);
    }
}
