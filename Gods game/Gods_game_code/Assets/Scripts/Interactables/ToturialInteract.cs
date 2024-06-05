using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ToturialInteract : MonoBehaviour
{
    private bool in_range = false;
    private int count = 1;

    [SerializeField] private GameObject toturial_text;
    private GameObject text_obj;

    void Awake()
    {
        text_obj = Instantiate(toturial_text);
    }
    void OnEnable()
    {
        Events.instance.Interact += Activate;
    }

    void Activate()
    {
        if(in_range)
        {
            StopAllCoroutines();
            text_obj.transform.GetChild(count-1).gameObject.SetActive(false);

            if(count == 6) count = 0;
            
            text_obj.transform.GetChild(count).gameObject.SetActive(true);
            count++;
            StartCoroutine(DeleteTextIn5Seconds(count));
        }
    }

    IEnumerator DeleteTextIn5Seconds(int count)
    {
        yield return new WaitForSeconds(5f);
        text_obj.transform.GetChild(count).gameObject.SetActive(false);
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
