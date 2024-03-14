using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class inventory_element : MonoBehaviour
{
    public GameObject item_object { get; set; }
    public void Equip()
    {
        InventoryManager.instance.EquipItem(item_object, gameObject);
    }
}
