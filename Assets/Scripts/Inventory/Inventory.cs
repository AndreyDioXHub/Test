using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : IInventory
{
    public event Action<object, IInventoryItem, int> OnInventoryItemAddedEvent;
    public event Action<object, Type, int> OnInventoryItemRemovedEvent;

    public event Action<object> OnInventoryStateChangedEvent;

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
        List<IInventorySlot> equipedslots = _slots.FindAll(slot => !slot.IsEmpty && slot.Item.State.IsEquipped);
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

    public void TransitFromSlotToSlot(object sender, IInventorySlot fromSlot, IInventorySlot toSlot)
    {
        if (fromSlot.IsEmpty == true)
        {
            return;
        }

        if (toSlot.IsFull)
        {
            return;
        }

        if(toSlot.IsEmpty == false && fromSlot.ItemType != toSlot.ItemType)
        {
            return;
        }

        if(fromSlot.ItemType != toSlot.ItemType)
        {
            var itemfrom = fromSlot.Item;
            var itemto= toSlot.Item;
            fromSlot.Clear();
            toSlot.Clear();

            fromSlot.SetItem(itemfrom);
            toSlot.SetItem(itemto);

            OnInventoryStateChangedEvent?.Invoke(sender);
            OnInventoryStateChangedEvent?.Invoke(sender);
        }
        /*
        if (toSlot.IsFull)
        {
            return;
        }*/

        var slotCapacity = fromSlot.Capacity;
        var fits = fromSlot.Amount + toSlot.Amount <= slotCapacity;
        var amountToAdd = fits ? fromSlot.Amount : slotCapacity - toSlot.Amount;
        var amountLeft = fromSlot.Amount - amountToAdd;

        if (toSlot.IsEmpty == true)
        {
            toSlot.SetItem(fromSlot.Item);
            fromSlot.Clear();
            OnInventoryStateChangedEvent?.Invoke(sender);
            OnInventoryStateChangedEvent?.Invoke(sender);
        }

        toSlot.Item.State.Amount += amountToAdd;

        if(fits == true)
        {
            fromSlot.Clear();
        }
        else
        {
            fromSlot.Item.State.Amount = amountLeft;
        }
        OnInventoryStateChangedEvent?.Invoke(sender);
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
                slot.Item.State.Amount -= amounttoremove;

                if (slot.Amount <= 0)
                {
                    slot.Clear();
                }
                OnInventoryItemRemovedEvent?.Invoke(sender, itemtype, amounttoremove);
                OnInventoryStateChangedEvent?.Invoke(sender);
                result = true;
                break;
            }


            amounttoremove -= slot.Amount;
            slot.Clear();
            result = true;
            OnInventoryItemRemovedEvent?.Invoke(sender, itemtype, slot.Amount);
            OnInventoryStateChangedEvent?.Invoke(sender);
        }

        return result;
    }

    public IInventorySlot[] GetAllSlots(Type itemtype)
    {
        return _slots.FindAll(slot => !slot.IsEmpty && slot.ItemType == itemtype).ToArray();
    }

    public IInventorySlot[] GetAllSlots()
    {
        //return _slots.FindAll(slot => !slot.IsEmpty).ToArray();
        return _slots.ToArray();
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

    public bool TryAddToSlot(object sender, IInventorySlot slot, IInventoryItem item)
    {
        bool fits = slot.Amount + item.State.Amount <= item.Info.MaxItemsInInventorySlot;
        int amounttoadd = fits ? item.State.Amount : item.Info.MaxItemsInInventorySlot - slot.Amount;
        int amountleft = item.State.Amount - amounttoadd;
        var cloneitem = item.Clone();
        cloneitem.State.Amount = amounttoadd;

        if(slot.IsEmpty)
        {
            slot.SetItem(cloneitem);
        }
        else
        {
            slot.Item.State.Amount += amounttoadd;
        }

        OnInventoryItemAddedEvent?.Invoke(sender, item, amounttoadd);

        if (amountleft <= 0)
        {
            return true;
        }

        item.State.Amount = amountleft;

        return TryToAdd(sender, item);
    }
}
