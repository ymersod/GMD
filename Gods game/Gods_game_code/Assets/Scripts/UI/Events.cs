using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Events : MonoBehaviour
{
    public static Events instance;

    public GameObject Canvas_inventory { get; set; }
    public event Action Interact;
    public event Action Map;
    public event Action Inventory;
    public event Action Fire;
    public event Action AltFire;
    public event Action<InputValue> Move;
    public GameObject player;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(transform.gameObject);
    }

    void OnInventory()
    {
        Inventory?.Invoke();
        //FixInputs();
        gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap("GameUI");
    }


    void OnInteract() => Interact?.Invoke();


    void OnMap()
    {
        Map?.Invoke();
        // FixInputs();
        gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap("GameUI");
        print(gameObject.GetComponent<PlayerInput>().currentActionMap.name);
    }
    
    void OnFire() => Fire?.Invoke();
    void OnAltFire() => AltFire?.Invoke();
    void OnMove(InputValue value) => Move?.Invoke(value);
    

    /* FixInputs
    void FixInputs()
    {
        player.transform.GetChild(0).gameObject.GetComponent<PlayerInput>().enabled = false;
        gameObject.GetComponent<PlayerInput>().enabled = false;
        UIEvents.instance.GetComponent<PlayerInput>().enabled = true;
    } /* FixInputs */

}
