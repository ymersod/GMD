using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class inventory_element : MonoBehaviour
{
    public GameObject item_object { get; set; }
    public void Click(string tag)
    {
        switch(tag)
        {
            case "bag_use_item":
                    InventoryManager.instance.UseItem(item_object, gameObject);
                break;
            case "bag_item":
                    InventoryManager.instance.EquipItem(item_object, gameObject);
                break;
            default:
                break;
        }
    }
}
