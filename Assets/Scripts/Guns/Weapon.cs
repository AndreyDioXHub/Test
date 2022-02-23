using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual GunProps CupyGun()
    {
        GameObject n1 = null;
        GameObject n2 = null;
        GunProps props = new GunProps(n1,n2);
        return props;
    }


    public virtual void Init(GunProps props)
    {

    }

    public virtual void Shoot()
    {

    }

    public virtual void ShootRelease()
    {

    }

    public virtual void Remove()
    {

    }


}

public class GunProps
{
    public GameObject Gun;
    public GameObject Bullet;
    public GunProps(GameObject gun, GameObject bullet)
    {
        Gun = gun;
        Bullet = bullet;
    }
}
