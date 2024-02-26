using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class movement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb_player;
    private Vector2 moveInput;
    public float speed = 5f;
    void Start()
    {
        rb_player = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb_player.MovePosition(rb_player.position + moveInput * speed * Time.fixedDeltaTime);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
