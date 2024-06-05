using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinInteract : MonoBehaviour
{
    private bool in_range = false;
    public int scene_number;
    [SerializeField] private Canvas win_text;
    [SerializeField] private Canvas not_worthy_text;

    void OnEnable()
    {
        Events.instance.Interact += Activate;
    }

    void Activate()
    {
        if(in_range && InventoryManager.instance.CountBlessings())
        {
            StartCoroutine(WinProcess());
        }
        else if(in_range)
        {
            StopCoroutine(NotWorthyProcess());
            StartCoroutine(NotWorthyProcess());
        }
    }

    IEnumerator WinProcess()
    {
        Instantiate(win_text);
        yield return new WaitForSeconds(5f);
        Scene_manager.instance.LoadScene(scene_number);
        yield return null;
    }

    IEnumerator NotWorthyProcess()
    {
        var text = Instantiate(not_worthy_text);
        yield return new WaitForSeconds(3f);
        Destroy(text.gameObject);
        yield return null;
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

    void OnDestroy()
    {
        Events.instance.Interact -= Activate;
    }
}
