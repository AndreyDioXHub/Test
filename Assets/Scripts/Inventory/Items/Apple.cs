using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : IInventoryItem
{
    public IInventoryItemInfo Info { get; }

    public IInventoryItemState State { get; }

    public Type ItemType => GetType();

    public Apple(IInventoryItemInfo info)
    {
        this.Info = info;
        State = new InventoryItemState();
    }

    public IInventoryItem Clone()
    {
        var clonedApple = new Apple(Info);
        clonedApple.State.Amount = State.Amount;
        return clonedApple;
    }
}
