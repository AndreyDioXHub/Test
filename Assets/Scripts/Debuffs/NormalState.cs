using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : Debuff
{
    public override float HpDebuff()
    {
        float debuf = 0;
        return debuf;
    }

    public override DebuffType GetDebuffType()
    {
        return new DebuffType(TypeDebuff.None, Color.white, 0);
    }
}
