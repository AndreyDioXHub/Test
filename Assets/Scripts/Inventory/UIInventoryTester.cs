using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryTester 
{
    private InventoryItemInfo _appleInfo;
    private InventoryItemInfo _pepperInfo;
    private UIInventorySlot[] _uiSlots;

    public Inventory CurInventory { get; }

    public UIInventoryTester(InventoryItemInfo appleInfo, InventoryItemInfo pepperInfo, UIInventorySlot[] uiSlots)
    {
        _appleInfo = appleInfo;
        _pepperInfo = pepperInfo;
        _uiSlots = uiSlots;
        CurInventory = new Inventory(15);
        CurInventory.OnInventoryStateChangedEvent += OnInventoryStateChanged;
    }

    public void FillSlot()
    {
        var allSlots = CurInventory.GetAllSlots();
        var availableSlots = new List<IInventorySlot>(allSlots);
        Debug.Log(availableSlots.Count);
        var fillSlots = 5;
        for (int i=0; i< fillSlots; i++)
        {
            var filledSlot = AddRandomApplesIntoRandomSlot(availableSlots);
            availableSlots.Remove(filledSlot);

            filledSlot = AddRandomPeppersIntoRandomSlot(availableSlots);
            availableSlots.Remove(filledSlot);
        }

        SetupInventoryUI(CurInventory);
    }

    private IInventorySlot AddRandomApplesIntoRandomSlot(List<IInventorySlot> slots)
    {
        var rSlotIndex = Random.Range(0, slots.Count-1);
        var rSlot = slots[rSlotIndex];
        var rCount = Random.Range(1, 4);
        var apple = new Apple(_appleInfo);
        apple.State.Amount = rCount;
        CurInventory.TryAddToSlot(this, rSlot, apple);
        return rSlot;
    }

    private IInventorySlot AddRandomPeppersIntoRandomSlot(List<IInventorySlot> slots)
    {
        var rSlotIndex = Random.Range(0, slots.Count);
        var rSlot = slots[rSlotIndex];
        var rCount = Random.Range(1, 4);
        var pepper = new Papper(_pepperInfo);
        pepper.State.Amount = rCount;
        CurInventory.TryAddToSlot(this, rSlot, pepper);
        return rSlot;
    }

    private void SetupInventoryUI(Inventory inventory)
    {
        var allSlots = inventory.GetAllSlots();
        var allSlotsCount = allSlots.Length;
        for(int i=0; i< allSlotsCount; i++)
        {
            var slot = allSlots[i];
            var uiSlot = _uiSlots[i];
            uiSlot.SetSlot(slot);
            uiSlot.Refresh();
        }

    }

    private void OnInventoryStateChanged(object sender)
    {
        foreach(var uiSlot in _uiSlots)
        {
            uiSlot.Refresh();
        }
    }
}
