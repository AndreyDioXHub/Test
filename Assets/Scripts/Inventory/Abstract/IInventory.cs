using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory
{
    int Capacity { get; set; }
    bool IsFull { get; }

    IInventoryItem GetItem(Type itemtype);
    IInventoryItem[] GetAllItems();
    IInventoryItem[] GetAllItems(Type itemtype);
    IInventoryItem[] GetEquipedItems();
    int GetItemAmount(Type itemtype);

    bool TryToAdd(object sender, IInventoryItem item);
    bool Remove(object sender, Type itemtype, int amount = 1);
    bool HasItem(Type type, out IInventoryItem item);
}
