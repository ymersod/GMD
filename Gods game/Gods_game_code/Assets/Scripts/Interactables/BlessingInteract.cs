using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlessingInteract : MonoBehaviour
{
    private bool in_range = false;
    [SerializeField] private GameObject blessing_prefab;


    void OnEnable()
    {
        Events.instance.Interact += Activate;
    }

    void Activate()
    {
        if(in_range)
        {
            print(gameObject.transform.position);
            Events.instance.Interact -= Activate;
            InventoryManager.instance.AddItemToBag(blessing_prefab, blessing_prefab.GetComponent<SpriteRenderer>().sprite);
        };
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
