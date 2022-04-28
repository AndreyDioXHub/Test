using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemInfo", menuName = "Gameplay/Items/Create New ItemInfo")]
public class InventoryItemInfo : ScriptableObject, IInventoryItemInfo
{
    [SerializeField] 
    private string ItemID;
    [SerializeField] 
    private string ItemTitle;
    [SerializeField] 
    private string ItemDescription;
    [SerializeField] 
    private int ItemMaxItemsInInventorySlot;
    [SerializeField] 
    private Sprite ItemIcon;

    public string Id => ItemID;

    public string Title => ItemTitle;

    public string Description => ItemDescription;

    public int MaxItemsInInventorySlot => ItemMaxItemsInInventorySlot;

    public Sprite Icon => ItemIcon;
}
