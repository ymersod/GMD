using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class player_attack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float meeleSpeed;
    [SerializeField] private float dmg;
    [SerializeField] private Transform attackPoint;
    private Vector2 lastMoveInput;
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
        print("Fire button pressed");
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack_sword"))
        {
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
        if(col.gameObject.CompareTag("enemy"))
        {
            col.gameObject.GetComponent<Enemy>().TakeDamage(dmg);
        }
    }
}
