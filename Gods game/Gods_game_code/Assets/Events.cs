using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Events : MonoBehaviour
{
    [SerializeField] private GameObject canvas_inventory;
    void OnInventory()
    {
        var is_inventory_active = canvas_inventory.activeInHierarchy;
        canvas_inventory.SetActive(!is_inventory_active);
    }
}
