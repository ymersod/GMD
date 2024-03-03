using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class player_attack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float meeleSpeed;
    [SerializeField] private float dmg = 50f;
    [SerializeField] private float knockback = 10f;
    [SerializeField] private Transform attackPoint;
    private Vector2 lastMoveInput;
    private Vector2 lastAttackDirection;
    float timeUntilMeele;
    void Start()
    {
        //move_script = gameObject.GetComponent<movement>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnFire(InputValue value)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack_sword"))
        {
            lastAttackDirection = lastMoveInput;
            float angle = Mathf.Atan2(lastMoveInput.y, lastMoveInput.x) * Mathf.Rad2Deg;
            attackPoint.rotation = Quaternion.Euler(0, 0, angle);
            animator.SetTrigger("Attack_0");
        }
    }

    void OnMove(InputValue value)
    {
        var direction = value.Get<Vector2>();
        if(direction != Vector2.zero)
        {
            lastMoveInput = value.Get<Vector2>();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //TODO: Bug where if i move the other direction last split second after hitting an enemy, the enemy will be knocked back in the wrong direction
        if(col.gameObject.CompareTag("enemy"))
        {
            col.gameObject.GetComponent<Enemy>().TakeDamage(dmg, knockback, lastAttackDirection);
        }
    }
}
