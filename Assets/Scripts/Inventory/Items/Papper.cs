using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Papper : IInventoryItem
{
    public IInventoryItemInfo Info { get; }

    public IInventoryItemState State { get; }

    public Type ItemType => GetType();

    public Papper(IInventoryItemInfo info)
    {
        this.Info = info;
        State = new InventoryItemState();
    }

    public IInventoryItem Clone()
    {
        var clonedPapper = new Papper(Info);
        clonedPapper.State.Amount = State.Amount;
        return clonedPapper;
    }
}
