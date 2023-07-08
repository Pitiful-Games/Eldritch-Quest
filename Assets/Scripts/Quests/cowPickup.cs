using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cowPickup : ItemPickup
{
    protected override void PickUp()
    {
        var inventory = UIManager.Instance.GetUI<Inventory>();
        inventory.AddItem(item);
    }
}
