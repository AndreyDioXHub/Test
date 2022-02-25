using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryItem 
{
    bool IsEquipped { get; set; }
    Type ItemType { get; }
    int MaxItemsInInventorySlot { get; }
    int Amount { get; set; }

    IInventoryItem Clone();
}
