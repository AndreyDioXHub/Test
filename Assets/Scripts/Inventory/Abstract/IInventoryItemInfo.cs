using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryItemInfo
{

    string Id { get; }
    string Title { get; }
    string Description { get; }
    int MaxItemsInInventorySlot { get; }
    Sprite Icon { get; }
}
