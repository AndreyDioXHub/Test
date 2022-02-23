using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolProjectile : Bullet
{
    private float _speed = 0;
    private float _lifeTime = 0;
    private float _bulletDamage = 0;

    public override void Init(float speed, float lifeTime, float bulletDamage)
    {
        base.Init(speed, lifeTime, bulletDamage);
        _speed = speed;
        _lifeTime = lifeTime;
        _bulletDamage = bulletDamage;
        Destroy(this.gameObject, _lifeTime);
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        transform.position += _speed * transform.forward * Time.deltaTime;
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.Update();

        if (other.tag == "Damageble")
        {
            other.transform.GetComponent<Damageble>().InvokeDamage(_bulletDamage);
        }

        Destroy(this.gameObject);
    }
}
