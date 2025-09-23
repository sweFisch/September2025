using UnityEngine;

public class WeaponItem : Item
{

    public GameObject bullet;
    public override void Use()
    {
        GameObject spawned = Instantiate(bullet, transform.position, Quaternion.identity);
        spawned.GetComponent<Bullet>()?.Fire(Mathf.Abs(_playerVelocity.x));
    }

    public override void Mishap()
    {
        Use();
    }
}
