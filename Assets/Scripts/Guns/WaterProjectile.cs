using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterProjectile : Bullet
{
    private float _speed = 0;
    private float _lifeTime = 0;
    private float _wetness = 0;
    private float _gravity = -98f;
    private Vector3 _velosity = Vector3.zero; 
    public override void Init(float speed, float lifeTime, float bulletDamage)
    {
        base.Init(speed, lifeTime, bulletDamage);
        _speed = speed;
        _lifeTime = lifeTime;
        _wetness = bulletDamage;
        Destroy(this.gameObject, _lifeTime);
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        _velosity += _gravity * Time.deltaTime * Time.deltaTime * Vector3.up;
        transform.position += _speed * (transform.forward + _velosity) * Time.deltaTime;
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.Update();

        if (other.tag == "Damageble")
        {
            other.transform.GetComponent<Damageble>().InvokeDebuff(new WatterDebuf());
        }

        Destroy(this.gameObject);
    }
}
