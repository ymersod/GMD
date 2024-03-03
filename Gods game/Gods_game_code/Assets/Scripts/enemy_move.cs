using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_move : MonoBehaviour
{
    private Transform thisTransform;
    
    [SerializeField]
    private float moveSpeed = 0.4f;

    [SerializeField]
    private Vector2 decisionTime = new Vector2(1, 3);
    private float decisionTimeCount = 0;
    private Vector2[] moveDirections = new Vector2[] { Vector2.right, Vector2.left, Vector2.up, Vector2.down, Vector2.zero, Vector2.zero };
    private int currentMoveDirection;
    private Rigidbody2D enemy_rb;
    private Animator enemy_anim;
 
    void Start()
    {
        thisTransform = this.transform;
        enemy_rb = this.GetComponent<Rigidbody2D>();
        enemy_anim = this.GetComponent<Animator>();
 
        // Set a random time delay for taking a decision ( changing direction, or standing in place for a while )
        decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);
 
        ChooseMoveDirection();
    }
 
    void FixedUpdate()
    {
        Move();
    }
 
    void ChooseMoveDirection()
    {
        // Choose whether to move sideways or up/down
        currentMoveDirection = Mathf.FloorToInt(Random.Range(0, moveDirections.Length));
    }

    void Move()
    {
        if(enemy_rb.velocity == Vector2.zero)
        {
            enemy_anim.SetBool("IsWalking", false);
        }
        else enemy_anim.SetBool("IsWalking", true);

        enemy_rb.AddForce(moveDirections[currentMoveDirection] * moveSpeed);
 
        if (decisionTimeCount > 0) decisionTimeCount -= Time.deltaTime;
        else
        {
            decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);

            ChooseMoveDirection();
        }
    }
}
