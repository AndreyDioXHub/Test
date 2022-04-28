using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItemState : IInventoryItemState
{
    public int ItemAmount;
    public bool ItemIsEquipped;
    public int Amount 
    { 
        get => ItemAmount; 
        set => ItemAmount = value; 
    }
    public bool IsEquipped 
    { 
        get => ItemIsEquipped; 
        set => ItemIsEquipped = value; 
    }

    public InventoryItemState()
    {
        ItemAmount = 0;
        ItemIsEquipped = false;
    }
}
