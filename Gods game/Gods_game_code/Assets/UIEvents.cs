using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
   public static UIEvents instance;
   public event Action Click;

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

    void OnClickInventory() =>  Click?.Invoke();
    void OnHoldMap() => Click?.Invoke();
}
