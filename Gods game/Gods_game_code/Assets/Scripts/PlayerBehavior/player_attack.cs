using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class player_attack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float meeleSpeed;
    [SerializeField] public float dmg = 50f;
    [SerializeField] private float knockback = 10f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float alt_speed = 10f;
    [SerializeField] private float alt_dmg = 10f;
    [SerializeField] private float alt_knockback = 10f;
    [SerializeField] private float alt_size = 2f;
    [SerializeField] private GameObject fireball;
    [SerializeField] private AudioClip fireballSound;
    [SerializeField] private AudioClip swordSound;
    private Vector2 lastMoveInput;
    private Vector2 lastAttackDirection;
    private Action<InputValue> moveDelegate;
    
    void Start()
    {
        Events.instance.Fire += OnFire;
        Events.instance.AltFire += OnAltFire;
        moveDelegate = (value) => OnMove(value);
        Events.instance.Move += moveDelegate;
    }

    void OnFire()
    {
        if (gameObject == null || gameObject.Equals(null))
            return;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack_sword"))
        {
            if(audioSource.clip != swordSound ) audioSource.clip = swordSound;
            audioSource.Play();
            lastAttackDirection = lastMoveInput;
            float angle = Mathf.Atan2(lastMoveInput.y, lastMoveInput.x) * Mathf.Rad2Deg;
            attackPoint.rotation = Quaternion.Euler(0, 0, angle);
            animator.SetTrigger("Attack_0");
        }
    }

    void OnMove(InputValue value)
    {
        var direction = value.Get<Vector2>();
        if (direction != Vector2.zero)
        {
            lastMoveInput = value.Get<Vector2>();
        }
    }

    void OnAltFire()
    {
        if(audioSource.clip != fireballSound ) audioSource.clip = fireballSound;
            audioSource.Play();

        if (gameObject == null || gameObject.Equals(null))
            return;
        StartCoroutine(Spawner.Instance.SpawnFireBall(lastMoveInput, transform.position, alt_speed, alt_size, alt_dmg, alt_knockback, fireball));
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("enemy"))
        {
            col.gameObject.GetComponent<Enemy>().TakeDamage(dmg, knockback, lastAttackDirection);
        }
    }

    public void Unsub()
    {
        Events.instance.Fire -=  OnFire;
        Events.instance.AltFire -= OnAltFire;
        Events.instance.Move -= moveDelegate;
    } 
}
