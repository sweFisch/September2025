using UnityEngine;
using UnityEngine.Events;

public class PotionItem : Item
{
    public GameObject SplashObject;
    public GameObject particles;

    public enum StatusBuff
    {
        speedUp = 0,
        speedDown = 1,
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Use()
    {
        ItemHandler []players = FindObjectsByType<ItemHandler>(FindObjectsSortMode.None);
        foreach (var p in players) 
        { 
            if (p._item == this)
            {
                Affect(p.gameObject);
            }
        }
    }

    public override void SetPosition(Transform newTransform, Vector2 playerVelocity, bool isFacingRight)
    {
        base.SetPosition(newTransform, playerVelocity, isFacingRight);
        
        if (!isFacingRight)
        {
            _spriteRenderer.flipX = true;
            _spriteRenderer.flipY = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
            _spriteRenderer.flipY = false;
        }
    }

    public override void Mishap()
    {
        Instantiate(particles, transform.position, Quaternion.identity);
        Instantiate(SplashObject,transform.position,Quaternion.identity);
        Destroy(this.gameObject);
    }

    void Affect(GameObject player)
    {

    }
}
