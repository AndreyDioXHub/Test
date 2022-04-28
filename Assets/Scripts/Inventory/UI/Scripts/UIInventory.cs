using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    [SerializeField]
    private InventoryItemInfo _appleInfo;
    [SerializeField]
    private InventoryItemInfo _papperInfo;

    public Inventory CurInventory => _tester.CurInventory;
    private UIInventoryTester _tester;

    private void Awake()
    {
        
        //CurInventory = new Inventory(15);
    }

    void Start()
    {
        var uiSlots = GetComponentsInChildren<UIInventorySlot>();
        _tester = new UIInventoryTester(_appleInfo, _papperInfo, uiSlots);
        _tester.FillSlot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
