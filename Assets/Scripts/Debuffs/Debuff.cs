using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuff 
{
    public virtual float HpDebuff()
    {
        return 0;
    }

    public virtual DebuffType GetDebuffType()
    {
        return new DebuffType(TypeDebuff.None, Color.white, 0);
    }
}

public class DebuffType
{
    public bool OverTime;
    public float Period;
    public float Duration;    
    public TypeDebuff DType;
    public Color HPBarColor;
    public float Misc;

    public DebuffType(float period, float duration, TypeDebuff dtype, Color hpbarcolor, float misc)
    {
        OverTime = true;
        Period = period;
        Duration = duration;        
        DType = dtype;
        HPBarColor = hpbarcolor;
        Misc = misc;
    }

    public DebuffType(TypeDebuff dtype, Color hpbarcolor, float misc)
    {
        Period = 0;
        Duration = 0;
        OverTime = false;
        DType = dtype;
        HPBarColor = hpbarcolor;
        Misc = misc;
    }
}

public enum TypeDebuff
{
    None = 0,
    Fire = 1,
    Water = 2
}
