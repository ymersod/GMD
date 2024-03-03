using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class movement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb_player;
    private Transform t_player;
    private Vector2 moveInput;
    private Animator player_anim;
    private BoxCollider2D player_boxcollider;
    /* [SerializeField]
    private Vector2 boxcastSize = new Vector2(0.5f, 0.5f); */
    public float speed = 5f;
    /* [SerializeField] private float immobilizeTimeout  = 0.2f; */
    void Start()
    {
        rb_player = gameObject.GetComponent<Rigidbody2D>();
        t_player = gameObject.GetComponent<Transform>();
        player_anim = gameObject.GetComponent<Animator>();
        player_boxcollider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(!IsVectorZero(moveInput) && IsVectorZero(rb_player.velocity))
        {
            player_anim.SetBool("IsMoving", true);
            MovePlayer(moveInput);
        }
        else
        {
            
            player_anim.SetBool("IsMoving", false);
        }
    }

    public void MovePlayer(Vector2 move)
    {
        rb_player.MovePosition(rb_player.position + speed * Time.fixedDeltaTime * move);
    }

    void OnMove(InputValue value)
    {
        var direction = value.Get<Vector2>();
        moveInput = direction;
        if(!IsVectorZero(moveInput))
        {
            player_anim.SetFloat("XInput", direction.x);
            player_anim.SetFloat("YInput", direction.y);
        }
    }

    bool IsVectorZero(Vector2 vector)
    {
        return vector == Vector2.zero;
    }

    /* bool RayCastForEnemyHitBox()
    {   
        Vector2 start = player_boxcollider.bounds.min; //Start point
        Vector2 end = player_boxcollider.bounds.max; // End point
        Vector2 size = boxcastSize; // Example size, adjust based on your visualization
        float angle = 0; // Example angle, adjust based on your visualization
        Vector2 direction = end - start; // Calculate the direction from start to end
        float distance = direction.magnitude; // Calculate the distance of the BoxCast
        RaycastHit2D hit = Physics2D.BoxCast(start, size, angle, direction, distance, 1 << 9);
        if (hit.collider != null && hit.collider.CompareTag("enemy")) {
            
            // Collision detected with an enemy
            Debug.Log("Player is about to collide with an enemy!");
            return true;
        }
        return false;
    } */

   
}

