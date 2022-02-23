using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDebuff : Debuff
{
    public override float HpDebuff()
    {
        float debuf = 10;
        return debuf;
    }

    public override DebuffType GetDebuffType()
    {
        float burnperiode = 1;
        float burnduration = 10;
        float burndamage = 1;
        Color firecolor = Color.red;
        return new DebuffType(burnperiode, burnduration, TypeDebuff.Fire, firecolor, burndamage);
    }
}
