using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private bool in_range = false;
    public int scene_number;

    void OnEnable()
    {
        Events.instance.Interact += Activate;
    }

    void Activate()
    {
        if(in_range) Scene_manager.instance.LoadScene(scene_number);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            in_range = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            in_range = false;
        }
    }
}
