using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Weapon
{
    [SerializeField]
    private GameObject _pistolModelPrefab;
    [SerializeField]
    private float _farLook = 5f;
    private GameObject _pistolModel;
    private Animator _pistolAnimator;

    private GameObject _gunBarrel;
    private string _gunBarrelName = "GunBarrel";
    private bool _isShoot = false;

    private float _time = 0;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        _time += Time.deltaTime;

        if (_time > 1)
        {
            _time = 1;
        }

        if (_isShoot)
        {
            if (Physics.Raycast(_gunBarrel.transform.position, _gunBarrel.transform.forward, out RaycastHit hit, _farLook))
            {
                Debug.DrawRay(_gunBarrel.transform.position, _gunBarrel.transform.forward * _farLook, Color.red);
                if (hit.collider.tag == "Damageble")
                {
                    if (_time == 1)
                    {
                        hit.collider.transform.GetComponent<Damageble>().InvokeDebuff(new FireDebuff());
                        _time = 0;
                    }                    
                }
            }
            else
            {
                Debug.DrawRay(_gunBarrel.transform.position, _gunBarrel.transform.forward * _farLook, Color.green);
            }
        }
    }
    public override GunProps CupyGun()
    {
        GameObject n = null;
        GunProps props = new GunProps(_pistolModelPrefab, n);
        return props;
    }
    public override void Init(GunProps props)
    {
        base.Init(props);
        _pistolModelPrefab = props.Gun;
        _pistolModel = Instantiate(_pistolModelPrefab, transform);
        _pistolAnimator = _pistolModel.GetComponent<Animator>();
        _gunBarrel = _pistolModel.transform.Find(_gunBarrelName).gameObject;
        _gunBarrel.SetActive(false);
    }

    public override void Shoot()
    {
        base.Shoot();
        _pistolAnimator.SetTrigger("Shoot");
        _gunBarrel.SetActive(true);
        _isShoot = true;
    }
    public override void ShootRelease()
    {
        base.ShootRelease();
        _gunBarrel.SetActive(false);
        _isShoot = false;
    }

    public override void Remove()
    {
        base.Remove();
        Destroy(_pistolModel);
        Destroy(this);
    }
}
