using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class player_attack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float meeleSpeed;
    [SerializeField] private float dmg;
    [SerializeField] private Transform attackPoint;
    private movement move_script;
    float timeUntilMeele;
    void Start()
    {
        move_script = gameObject.GetComponent<movement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnFire(InputValue value)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack_1"))
        {
            float angle = Mathf.Atan2(move_script.lastMoveInput.y, move_script.lastMoveInput.x) * Mathf.Rad2Deg;
            print(angle);
            attackPoint.rotation = Quaternion.Euler(0, 0, angle);
            print(attackPoint.rotation);
            animator.SetTrigger("Attack");
        }
    }
}
