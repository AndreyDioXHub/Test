using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : IInventorySlot
{
    public bool IsFull => Amount == Capacity;

    public bool IsEmpty => Item == null;

    public IInventoryItem Item { get; private set; }

    public Type ItemType => Item.ItemType;

    public int Amount => IsEmpty ? 0 : Item.Amount;

    public int Capacity { get; private set; }

    public void Clear()
    {
        if (IsEmpty)
        {
            return;
        }

        Item.Amount = 0;
        Item = null;
    }

    public void SetItem(IInventoryItem item)
    {
        if (!IsEmpty)
        {
            return;
        }

        this.Item = item;
        this.Capacity = item.MaxItemsInInventorySlot;
    }
}
