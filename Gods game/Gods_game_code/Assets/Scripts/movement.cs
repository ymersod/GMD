using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb_player;
    private Vector2 moveInput;
    private Animator player_anim;
    private BoxCollider2D player_boxcollider;
    public float speed = 5f;
    private bool is_dashing = false;
    private bool can_dash = true;
    [SerializeField] private float dash_force = 10f;
    [SerializeField] private float dash_time = 2f;
    [SerializeField] private float dash_cooldown = 2f;
    /* [SerializeField] private float immobilizeTimeout  = 0.2f; */
     /* [SerializeField]
    private Vector2 boxcastSize = new Vector2(0.5f, 0.5f); */
    void Start()
    {
        rb_player = gameObject.GetComponent<Rigidbody2D>();
        player_anim = gameObject.GetComponent<Animator>();
        player_boxcollider = gameObject.GetComponent<BoxCollider2D>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(is_dashing)
        {
            return;
        } 
        if(!IsVectorZero(moveInput))
        {
            player_anim.SetBool("IsMoving", true);
            rb_player.velocity = speed * moveInput;
        }
        else
        {
            player_anim.SetBool("IsMoving", false);
        }
    }

    bool IsVectorZero(Vector2 vector)
    {
        return vector == Vector2.zero;
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
    private IEnumerator Dash(Vector2 direction)
    {
        can_dash = false;
        is_dashing = true;
        var elapsedTime = 0f;
        while(elapsedTime < dash_time)
        {
            rb_player.velocity= direction * dash_force;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        is_dashing = false;
        yield return new WaitForSeconds(dash_cooldown);
        can_dash = true;
    }
    void OnDash()
    {
        if(can_dash) StartCoroutine(Dash(moveInput));
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

