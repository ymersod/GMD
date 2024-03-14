

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System;

public class PiggyAttackMode : MonoBehaviour
{
    private delegate IEnumerator Action();
    private List<Action> actions;
    private Vector2 player;
    private Vector2 self;
    private Rigidbody2D enemy_rb;
    private Animator enemy_anim;
    private Tilemap world_tilemap;
    [SerializeField] private float sprint_speed = 20f;
    [SerializeField] private float regular_attack_range = 1f;
    [SerializeField] private float timeout_var = 5f;
    private float timeout_counter = 0f;
    private float attack_timeout_counter = 0f;
    private float poop_timeout_counter = 0f;
    private float circleAround_timeout_counter = 0f;
    [SerializeField] private GameObject quick_cast_indicator;
    [SerializeField] private float attack_width = 10f;
    [SerializeField] private float attack_length = 10f;
    [SerializeField] private float attack_chargeup_duration = 2f;
    [SerializeField] private float attack_force = 100f;
    [SerializeField] private float poop_size = 1f;
    [SerializeField] private float poop_speed = 10f;
    [SerializeField] private float poop_dmg = 10f;
    [SerializeField] private GameObject poop_prefab;
    
    void Start()
    {
        world_tilemap = LOSManager.Instance.ground_tilemap;
    }
    void OnEnable()
    {
        GetComponent<enemy_move>().AttackMode += PiggyChooseAction;
        enemy_rb = GetComponent<Rigidbody2D>();
        enemy_anim = GetComponent<Animator>();

        actions = new List<Action>();
        //Weighted towards moving closer
        actions.AddRange(new Action[] { MoveCloserFast, MoveCloserFast, MoveCloserFast, MoveCloserFast,CircleAround, ThrowPoop });
    }

    private void PiggyChooseAction(Vector2 from, Vector2 to)
    {
        player = to;
        self = from;
        var choice = UnityEngine.Random.Range(0, actions.Count);
        var action = actions[choice];

        timeout_counter = SubtractTime(timeout_counter);
        if (CheckTime(timeout_counter))
        {
            attack_timeout_counter = SubtractTime(attack_timeout_counter);
            poop_timeout_counter = SubtractTime(poop_timeout_counter);
            circleAround_timeout_counter = SubtractTime(circleAround_timeout_counter);
            
            if(Vector2.Distance(from, to) < regular_attack_range && Math.Abs(from.x - to.x) > Math.Abs(from.y - to.y) && CheckTime(attack_timeout_counter))
            {
                timeout_counter = timeout_var;
                StartCoroutine(RegularAttack());
                attack_timeout_counter = timeout_var;
            }
            else if(action.Method.Name == "ThrowPoop" && CheckTime(poop_timeout_counter))
            {  
                StartCoroutine(action());
                poop_timeout_counter = timeout_var + UnityEngine.Random.Range(0f, timeout_var);
            }
            else if(action.Method.Name == "CircleAround" && CheckTime(circleAround_timeout_counter))
            {
                timeout_counter = timeout_var;
                StartCoroutine(action());
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

    private IEnumerator ThrowPoop()
    {
        StartCoroutine(Spawner.Instance.SpawnPoop(player, self, poop_speed, poop_size, poop_dmg, poop_prefab));
        yield return new WaitForSeconds(0f);
    }

    private IEnumerator RegularAttack()
    {
        Vector2 direction = player - self;
        enemy_anim.SetBool("IsWalking", false);
        enemy_anim.SetFloat("InputX", direction.x);
        
        enemy_anim.SetTrigger("Charge");
        Spawner.Instance.SpawnQuickCastIndicator(gameObject, self, player, attack_chargeup_duration, quick_cast_indicator, attack_length, attack_width);

        yield return new WaitForSeconds(attack_chargeup_duration);
        enemy_anim.SetTrigger("Attack");

        //TODO: Attack is weird 
        enemy_rb.AddForce(Vector3.Normalize(direction) * attack_force, ForceMode2D.Impulse);
    }

    //Can we use the yield here as circling maybe?
    private IEnumerator MoveCloserFast()
    {   
        var direction = NormalizeVector2Direction(player, self);

        enemy_rb.AddForce(direction * sprint_speed);
        
        enemy_anim.SetBool("IsWalking", true);
        enemy_anim.SetFloat("InputX", direction.x);

        yield return new WaitForSeconds(0f);

    }
    private IEnumerator CircleAround()
    {
        var random_direction = UnityEngine.Random.Range(0, 2);
        
        float elapsedTime = 0f;
        while (elapsedTime < timeout_var)
        {
            Vector2 direction_to_Player = player - self;

            Vector2 rotatedVector = random_direction == 0 ? new(-direction_to_Player.y, direction_to_Player.x) : new(direction_to_Player.y, -direction_to_Player.x);

            Vector2 finalVector = rotatedVector + new Vector2(self.x, self.y);
            
            var direction = Vector3.Normalize(finalVector-self);
            enemy_anim.SetBool("IsWalking", true);
            enemy_anim.SetFloat("InputX", direction.x);

            enemy_rb.AddForce(direction * 70f); 
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        circleAround_timeout_counter = timeout_var;
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
