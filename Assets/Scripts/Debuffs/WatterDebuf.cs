using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatterDebuf : Debuff
{
    public override float HpDebuff()
    {
        float debuf = -10;
        return debuf;
    }

    public override DebuffType GetDebuffType()
    {
        Color watercolor = Color.blue;
        float wetness = 10;
        return new DebuffType(TypeDebuff.Water, Color.blue, wetness);
    }
}
