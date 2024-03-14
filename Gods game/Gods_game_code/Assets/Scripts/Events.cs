using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Events : MonoBehaviour
{
    public static Events instance;

    public GameObject Canvas_inventory {get; set;}
    public event Action Interact;
    public event Action Map; 
    public event Action Inventory;
    public event Action Draw;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(transform.gameObject);
    }

    void OnInventory() => Inventory?.Invoke();
    

    void OnInteract() => Interact?.Invoke();
    

    void OnMap() => Map?.Invoke();
    
}
