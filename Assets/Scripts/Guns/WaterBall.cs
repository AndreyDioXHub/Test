using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBall : Weapon
{
    [SerializeField]
    private GameObject _pistolModelPrefab;
    private GameObject _pistolModel;
    private Animator _pistolAnimator;

    private Transform _gunBarrel;
    private string _gunBarrelName = "GunBarrel";

    [SerializeField]
    private GameObject _bulletPrefab;
    private GameObject _bullet;

    private float _bulletspeed = 5f;
    private float _bulletlifetime = 5f;
    private float _bulletdamage = 10f;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }
    public override GunProps CupyGun()
    {
        GunProps props = new GunProps(_pistolModelPrefab, _bulletPrefab);
        return props;
    }

    public override void Init(GunProps props)
    {
        base.Init(props);
        _pistolModelPrefab = props.Gun;
        _bulletPrefab = props.Bullet;

        _pistolModel = Instantiate(_pistolModelPrefab, transform);
        _pistolAnimator = _pistolModel.GetComponent<Animator>();
        _gunBarrel = _pistolModel.transform.Find(_gunBarrelName);
    }

    public override void Shoot()
    {
        base.Shoot();
        _pistolAnimator.SetTrigger("Shoot");
        _bullet = Instantiate(_bulletPrefab, _gunBarrel.position, _gunBarrel.rotation);        
        _bullet.GetComponent<Bullet>().Init(_bulletspeed, _bulletlifetime, _bulletdamage);
    }
    public override void ShootRelease()
    {
        base.ShootRelease();
    }

    public override void Remove()
    {
        base.Remove();
        Destroy(_pistolModel);
        Destroy(this);
    }
}
