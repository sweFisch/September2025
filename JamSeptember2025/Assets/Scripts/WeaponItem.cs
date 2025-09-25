using Unity.VisualScripting;
using UnityEngine;

public class WeaponItem : Item
{
    [SerializeField] float cooldown;
    float currentCD;
    [SerializeField] float ammo;


    float despawntimer;

    [SerializeField] Transform _firePoint;

    public GameObject bullet;
    public GameObject outOfAmmoEffect;
    public override void Use()
    {
        if (currentCD <= Time.time && ammo > 0)
        {
            currentCD = Time.time + cooldown;
            ammo--;

            GameObject spawned = Instantiate(bullet, _firePoint.position, _firePoint.rotation);
            spawned.GetComponent<Bullet>()?.Fire(Mathf.Abs(_playerVelocity.x));

            UseParticle(_firePoint);
        } else if (ammo <= 0)
        {
            Instantiate(outOfAmmoEffect,_firePoint.position,Quaternion.identity);
        }
    }

    public override void Mishap()
    {
        Use();
    }

    private void Update()
    {
        if (ammo <= 0 && !owned)
        {
            despawntimer -= Time.deltaTime;
            if (despawntimer < 0)
            {
                Instantiate(outOfAmmoEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        else { despawntimer = 5; }
    }
}
