using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SlimeAttackMode : MonoBehaviour
{
    private Animator enemy_anim;
    private Rigidbody2D enemy_rb;
    private Vector2 player;
    private Vector2 self;
    private Tilemap world_tilemap;
    [SerializeField] private float sprint_speed = 20f;
    [SerializeField] private GameObject quick_cast_indicator;
    [SerializeField] private float attack_width = 10f;
    [SerializeField] private float attack_length = 10f;
    [SerializeField] private float attack_chargeup_duration = 2f;
    [SerializeField] private float attack_force = 100f;
    [SerializeField] private float attack_range = 2f;
    [SerializeField] private float timeout_var = 2f;
    private float timeout_counter = 0f;
    private float attack_timeout_counter = 0f;


    void Start()
    {
        world_tilemap = LOSManager.Instance.GetTilemap();
    }
    void OnEnable()
    {
        GetComponent<enemy_move>().AttackMode += SlimeChooseAction;
        enemy_anim = GetComponent<Animator>();
        enemy_rb = GetComponent<Rigidbody2D>();
    }

    void SlimeChooseAction(Vector2 self, Vector2 player)
    {
        this.player = player;
        this.self = self;
        timeout_counter = SubtractTime(timeout_counter);
        if(CheckTime(timeout_counter))
        {
            attack_timeout_counter = SubtractTime(attack_timeout_counter);
            if (Vector2.Distance(self, player) < attack_range && CheckTime(attack_timeout_counter))
            {
                timeout_counter = timeout_var;
                StartCoroutine(RegularAttack());
                attack_timeout_counter = timeout_var;
            }
            else
            {
                StartCoroutine(MoveCloserFast());
            }
        }
    }

    private bool CheckTime(float counter)
    {
        if (counter <= 0)
        {
            return true;
        }
        return false;
    }
    private float SubtractTime(float counter)
    {
        counter -= Time.deltaTime;
        return counter;
    }

    IEnumerator RegularAttack()
    {
        Vector2 direction = player - self;
        enemy_anim.SetBool("IsWalking", false);
        enemy_anim.SetFloat("InputX", direction.x);
        
        enemy_anim.SetTrigger("Charge");
        Spawner.Instance.SpawnQuickCastIndicator(self, player, attack_chargeup_duration, quick_cast_indicator, attack_length, attack_width);

        yield return new WaitForSeconds(attack_chargeup_duration);
        enemy_anim.SetTrigger("Attack");

        //TODO: Attack is weird 
        enemy_rb.AddForce(Vector3.Normalize(direction) * attack_force, ForceMode2D.Impulse);
    }

    IEnumerator MoveCloserFast()
    {
        var direction = NormalizeVector2Direction(player, self);
        enemy_rb.AddForce(direction * sprint_speed);
        enemy_anim.SetBool("IsWalking", true);
        yield return null;
    }

    private Vector2 NormalizeVector2Direction(Vector2 target, Vector2 self)
    {
        Vector3Int player_cellPosition = world_tilemap.WorldToCell(target);
        Vector3Int self_cellPosition = world_tilemap.WorldToCell(self);
        Vector3 direction = player_cellPosition - self_cellPosition;
        direction.Normalize();
        float adjustedX = direction.x > 0 ? 1 : direction.x < 0 ? -1 : 0;
        float adjustedY = direction.y > 0 ? 1 : direction.y < 0 ? -1 : 0;

        if (Mathf.Abs(direction.x) > 0.5f && Mathf.Abs(direction.y) > 0.5f)
        {
            adjustedX = direction.x > 0 ? 0.71f : -0.71f;
            adjustedY = direction.y > 0 ? 0.71f : -0.71f;
        }

        return new Vector2(adjustedX, adjustedY);
    }
}
