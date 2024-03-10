using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopScript : MonoBehaviour
{
    private float poop_dmg;
    private float contact_knockback;
    private Vector2 direction;

    public void SetPoopStats(float dmg, float knockback, Vector2 dir)
    {
        poop_dmg = dmg;
        contact_knockback = knockback;
        direction = dir;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject.transform.parent.gameObject);
            StartCoroutine(collision.gameObject.GetComponent<PlayerStatus>().TakeDmg(poop_dmg, 0, direction));
        }
    }
}   
