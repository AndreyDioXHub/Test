using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EptyWeapon : Weapon
{
    [SerializeField]
    private string _message;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void Shoot()
    {
        base.Shoot();
        Debug.Log("ïûù ïûù " + _message);
    }

    public override void ShootRelease()
    {
        base.ShootRelease();
    }

    public override void Remove()
    {
        base.Remove();
        Destroy(this);
    }

}
