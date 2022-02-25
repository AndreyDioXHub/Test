using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : IInventory
{
    public event Action<object, IInventoryItem, int> OnInventoryItemAddedEvent;
    public event Action<object, Type, int> OnInventoryItemRemovedEvent;

    public int Capacity { get; set; }

    public bool IsFull => _slots.All(slot => slot.IsFull);

    private List<IInventorySlot> _slots;

    public Inventory(int capacity)
    {
        this.Capacity = capacity;
        _slots = new List<IInventorySlot>(capacity);
        for(int i=0; i< capacity; i++)
        {
            _slots.Add(new InventorySlot());
        }
    }

    public IInventoryItem[] GetAllItems()
    {
        List<IInventoryItem> allitems = new List<IInventoryItem>();
        foreach(var slot in _slots)
        {
            if (!slot.IsEmpty)
            {
                allitems.Add(slot.Item);
            }
        }
        return allitems.ToArray();
    }

    public IInventoryItem[] GetAllItems(Type itemtype)
    {
        List<IInventoryItem> allitems = new List<IInventoryItem>();
        List<IInventorySlot> slotsoftype = _slots.FindAll(slot => !slot.IsEmpty && slot.ItemType == itemtype);
        foreach (var slot in slotsoftype)
        {
            if (!slot.IsEmpty)
            {
                allitems.Add(slot.Item);
            }
        }
        return allitems.ToArray();
    }

    public IInventoryItem[] GetEquipedItems()
    {
        List<IInventoryItem> items = new List<IInventoryItem>();
        List<IInventorySlot> equipedslots = _slots.FindAll(slot => !slot.IsEmpty && slot.Item.IsEquipped);
        foreach (var slot in equipedslots)
        {
            if (!slot.IsEmpty)
            {
                items.Add(slot.Item);
            }
        }
        return items.ToArray();
    }

    public IInventoryItem GetItem(Type itemtype)
    {
        return _slots.Find(slot => slot.ItemType == itemtype).Item;
    }

    public int GetItemAmount(Type itemtype)
    {
        int amount = 0;
        List<IInventorySlot> allitemsslots = _slots.FindAll(slot => !slot.IsEmpty && slot.ItemType == itemtype);
        foreach (var slot in allitemsslots)
        {
            amount += slot.Amount;
        }
        return amount;
    }

    public bool HasItem(Type type, out IInventoryItem item)
    {
        item = GetItem(type);
        return item != null;
    }

    public bool Remove(object sender, Type itemtype, int amount = 1)
    {
        bool result = false;
        var slotswithitems = GetAllSlots(itemtype);
        if(slotswithitems.Length == 0)
        {
            return result;
        }

        int amounttoremove = amount;
        int count = slotswithitems.Length;

        for(int i = count - 1; i>=0; i--)
        {
            var slot = slotswithitems[i];

            if(slot.Amount >= amounttoremove)
            {
                slot.Item.Amount -= amounttoremove;

                if (slot.Amount <= 0)
                {
                    slot.Clear();
                }
                OnInventoryItemRemovedEvent?.Invoke(sender, itemtype, amounttoremove);
                result = true;
                break;
            }


            amounttoremove -= slot.Amount;
            slot.Clear();
            result = true;
            OnInventoryItemRemovedEvent?.Invoke(sender, itemtype, slot.Amount);
        }

        return result;
    }

    public IInventorySlot[] GetAllSlots(Type itemtype)
    {
        return _slots.FindAll(slot => !slot.IsEmpty && slot.ItemType == itemtype).ToArray();
    }

    public IInventorySlot[] GetAllSlots()
    {
        return _slots.FindAll(slot => !slot.IsEmpty).ToArray();
    }

    public bool TryToAdd(object sender, IInventoryItem item)
    {
        IInventorySlot sameitemslot = _slots.Find(slot => !slot.IsEmpty && slot.ItemType == item.ItemType && !IsFull);

        if(sameitemslot != null)
        {
            return TryAddToSlot(sender, sameitemslot, item);
        }

        IInventorySlot emptyslot = _slots.Find(slot => slot.IsEmpty);

        if (emptyslot != null)
        {
            return TryAddToSlot(sender, emptyslot, item);
        }

        return false; //инвентарь полный

    }

    private bool TryAddToSlot(object sender, IInventorySlot slot, IInventoryItem item)
    {
        bool fits = slot.Amount + item.Amount <= item.MaxItemsInInventorySlot;
        int amounttoadd = fits ? item.Amount : item.MaxItemsInInventorySlot - slot.Amount;
        int amountleft = item.Amount - amounttoadd;
        var cloneitem = item.Clone();
        cloneitem.Amount = amounttoadd;

        if(slot.IsEmpty)
        {
            slot.SetItem(cloneitem);
        }
        else
        {
            slot.Item.Amount += amounttoadd;
        }

        OnInventoryItemAddedEvent?.Invoke(sender, item, amounttoadd);

        if (amountleft <= 0)
        {
            return true;
        }

        item.Amount = amountleft;

        return TryToAdd(sender, item);
    }
}
