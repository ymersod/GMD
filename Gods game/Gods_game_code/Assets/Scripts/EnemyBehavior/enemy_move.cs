using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class enemy_move : MonoBehaviour
{    
    [SerializeField]
    private float moveSpeed = 0.4f;

    [SerializeField]
    private Vector2 decisionTime = new(1, 3);
    private float decisionTimeCount = 0;
    private readonly Vector2[] moveDirections = new Vector2[] { Vector2.right, Vector2.left, Vector2.up, Vector2.down, Vector2.zero, Vector2.zero };
    private int currentMoveDirection;
    private Rigidbody2D enemy_rb;
    private Animator enemy_anim;
    private Transform player_transform = null;
    private LOSCheck LOSCheck;
    public event System.Action<Vector2, Vector2> AttackMode;

    void Start()
    {
        enemy_rb = GetComponent<Rigidbody2D>();
        enemy_anim = GetComponent<Animator>();
        LOSCheck = GetComponent<LOSCheck>();
 
        // Set a random time delay for taking a decision ( changing direction, or standing in place for a while )
        decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);
    
        ChooseMoveDirection();
    }
 
    void FixedUpdate()
    {   
        if(LOSCheck)
        {
            if(LOSCheck.IsInLOS()){
                if(player_transform == null) player_transform = LOSManager.Instance.GetPlayerTransform();
                AttackModeMove(transform.position, player_transform.position);
            }
            else Move();
        }
        else Move();
    }

    private void AttackModeMove(Vector2 self, Vector2 player)
    {
        AttackMode?.Invoke(self, player);
    }


    void ChooseMoveDirection()
    {
        // Choose whether player move sideways or up/down
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
