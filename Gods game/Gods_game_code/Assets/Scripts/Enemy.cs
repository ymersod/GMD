using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator enemy_anim;
    private Rigidbody2D enemy_rb;
    private ParticleSystem enemy_ps;
    private DamageFlash damage_flash;
    [SerializeField] private float health = 100f;
    [SerializeField] private float weight = 1f;
    [SerializeField] private float drag = 8f;
    [SerializeField] private float contact_dmg = 5f;
    [SerializeField] private float contact_knockback = 5f;

    void Start()
    {
        enemy_anim = this.GetComponent<Animator>();
        enemy_rb = this.GetComponent<Rigidbody2D>();
        enemy_ps = this.GetComponent<ParticleSystem>();
        damage_flash = this.GetComponent<DamageFlash>();
        enemy_rb.drag = drag;
        enemy_rb.mass = weight;
    }

    public void TakeDamage(float damage, float knockback, Vector2 lastAttackInput)
    {
        health -= damage;
        if(health > 0)
            enemy_rb.AddForce(lastAttackInput * knockback, ForceMode2D.Impulse);
            enemy_anim.SetTrigger("Hit");
            ParticleSystem.ShapeModule ps_rotation = enemy_ps.shape;
            ps_rotation.rotation = CalculateRotation(lastAttackInput);
            damage_flash.TakeDamage();
            enemy_ps.Play();
        if(health <= 0)
            Die();
    }

    private Vector3 CalculateRotation(Vector2 lastAttackInput)
    {
        float angle = Mathf.Atan2(lastAttackInput.y, lastAttackInput.x) * Mathf.Rad2Deg;
        
        return new Vector3(0, 0, angle - 20f);
    }

    public void Die()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        enemy_anim.SetTrigger("Death");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            var direction = Vector2.zero;
            foreach(ContactPoint2D contact in collision.contacts)
            {
                direction += contact.normal;
            }
            collision.gameObject.GetComponent<PlayerStatus>().TakeDmg(contact_dmg, contact_knockback, direction);
        }
    }
}
