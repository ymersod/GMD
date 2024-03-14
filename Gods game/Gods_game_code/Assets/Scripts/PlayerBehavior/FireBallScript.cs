using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallScript : MonoBehaviour
{
    private float dmg;
    private float knockback;
    private Vector2 direction;

    internal void SetStats(float dmg, float knockback, Vector2 direction)
    {
        this.dmg = dmg;
        this.knockback = knockback;
        this.direction = direction; 
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            Destroy(gameObject.transform.parent.gameObject);
            collision.gameObject.GetComponent<Enemy>().TakeDamage(dmg, knockback, direction);
        }
    }
}   
