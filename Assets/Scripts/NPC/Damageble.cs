using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageble : MonoBehaviour
{
    public UnityEvent<float> OnDamage;
    public UnityEvent<Debuff> OnDebuff;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InvokeDamage(float damage)
    {
        OnDamage?.Invoke(damage);
    }

    public void InvokeDebuff(Debuff debuff)
    {
        OnDebuff?.Invoke(debuff);
    }

}
