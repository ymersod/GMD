using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIEvents : MonoBehaviour
{
    public static UIEvents instance;
    public event Action Click;
    public event Action Point;
    public event Action ActivateItem;
    public event Action Close;
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
        DontDestroyOnLoad(transform.gameObject);
    }

    void OnClickInventory() => Click?.Invoke();
    void OnHoldMap() => Click?.Invoke();
    void OnActivate() => ActivateItem?.Invoke();
    void OnClose()
    {
        Close?.Invoke();

        gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap("GamepadBS");
        print(gameObject.GetComponent<PlayerInput>().currentActionMap.name);
        //player.transform.GetChild(0).gameObject.GetComponent<PlayerInput>().enabled = true;
        //Events.instance.GetComponent<PlayerInput>().enabled = true;

        //gameObject.GetComponent<PlayerInput>().enabled = false;
    }
}
