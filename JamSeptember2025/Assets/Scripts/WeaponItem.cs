using UnityEngine;

public class WeaponItem : Item
{

    [SerializeField] Transform _firePoint;

    public GameObject bullet;
    public override void Use()
    {
        GameObject spawned = Instantiate(bullet, _firePoint.position, _firePoint.rotation);
        spawned.GetComponent<Bullet>()?.Fire(Mathf.Abs(_playerVelocity.x));

        UseParticle(_firePoint);
    }

    public override void Mishap()
    {
        Use();
    }
}
