using UnityEngine;

public class WeaponItem : Item
{

    [SerializeField] Transform _firePoint;

    public GameObject bullet;
    public override void Use()
    {
        GameObject spawned = Instantiate(bullet, _firePoint.position, Quaternion.identity);
        spawned.GetComponent<Bullet>()?.Fire(Mathf.Abs(_playerVelocity.x));
        print(_playerVelocity);
    }

    public override void Mishap()
    {
        Use();
    }
}
