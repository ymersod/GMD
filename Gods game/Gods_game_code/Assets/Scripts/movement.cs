using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class movement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb_player;
    private Transform t_player;
    private Vector2 moveInput;
    private Animator player_anim;
    public float speed = 5f;
    void Start()
    {
        rb_player = gameObject.GetComponent<Rigidbody2D>();
        t_player = gameObject.GetComponent<Transform>();
        player_anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(moveInput != Vector2.zero)
        {
            player_anim.SetBool("IsMoving", true);
            MovePlayer(moveInput);
        }
        else
        {
            player_anim.SetBool("IsMoving", false);
        }

        // if(moveInput.x != 0 || moveInput.y != 0)
        //     t_player.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg - 90);
    }

    public void MovePlayer(Vector2 move)
    {
        rb_player.MovePosition(rb_player.position + speed * Time.fixedDeltaTime * move);
    }

    void OnMove(InputValue value)
    {
        var direction = value.Get<Vector2>();
        moveInput = direction;
        if(moveInput != Vector2.zero)
        {
            player_anim.SetFloat("XInput", direction.x);
            player_anim.SetFloat("YInput", direction.y);
        }
    }
}
