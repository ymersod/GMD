using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class movement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb_player;
    private Transform t_player;
    public Vector2 moveInput;
    public Vector2 lastMoveInput;
    public float speed = 5f;
    void Start()
    {
        rb_player = gameObject.GetComponent<Rigidbody2D>();
        t_player = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb_player.MovePosition(rb_player.position + speed * Time.fixedDeltaTime * moveInput);
        // if(moveInput.x != 0 || moveInput.y != 0)
        //     t_player.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg - 90);
    }

    void OnMove(InputValue value)
    {
        var direction = value.Get<Vector2>();
        moveInput = direction;
        if(direction.x != 0 || direction.y != 0)
            lastMoveInput = direction;
    }
}
