using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : UIItem
{
    [SerializeField]
    private Image _imageIcon;
    [SerializeField]
    private Text _textAmount;

    public IInventoryItem Item { get; private set; }

    public void Refresh(IInventorySlot slot)
    {
        if(slot.IsEmpty == true)
        {
            CleanUp();
            return;
        }

        Item = slot.Item;
        _imageIcon.sprite = Item.Info.Icon;
        _imageIcon.gameObject.SetActive(true);

        var textAmountEnabled = slot.Amount > 1;
        _textAmount.gameObject.SetActive(textAmountEnabled);

        if (textAmountEnabled)
        {
            _textAmount.text = $"x{slot.Amount.ToString()}";
        }
    }

    private void CleanUp()
    {
        _imageIcon.gameObject.SetActive(false);
        _textAmount.gameObject.SetActive(false);
    }
}
