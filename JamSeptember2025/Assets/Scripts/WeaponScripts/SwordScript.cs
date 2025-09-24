using UnityEngine;

public class SwordScript : Item
{

    [SerializeField] Transform _firePoint;

    public GameObject bullet;
    public override void Use()
    {
        GameObject spawned = Instantiate(bullet, _firePoint.position, _firePoint.rotation);
        spawned.GetComponent<SwordSlash>()?.Fire(Mathf.Abs(_playerVelocity.x));
        print(_playerVelocity);
    }

    public override void Mishap()
    {
        Use();
    }
}
