using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField]
    private float max_health = 100f;
    public float current_health = 100f;
    [SerializeField] private float knockback_duration = .5f;
    private Image hp_bar;
    private Rigidbody2D player_rb;
    private Animator player_anim;
    void OnEnable()
    {
        player_rb = GetComponent<Rigidbody2D>();
        GameObject myGameObject = GameObject.Find("Health_bar");
        player_anim = GetComponent<Animator>();

        // Check if the GameObject was found
        if (myGameObject != null)
        {   
           hp_bar = myGameObject.GetComponent<Image>();
        }
    }

    void Update()
    {
        hp_bar.fillAmount = UpdateHealthBar(current_health, max_health);
        CheckDeath();
    }

    private float UpdateHealthBar(float current_health, float max_health)
    {
        return current_health / max_health;
    }

    public IEnumerator TakeDmg(float dmg, float knockback, Vector2 direction)
    {
        player_anim.SetTrigger("IsHit");
        current_health -= dmg;
        
        /* var elapsedTime = 0f;
        while(elapsedTime < knockback_duration)
        {
            elapsedTime += Time.deltaTime;
            player_rb.velocity= -direction * knockback;
            yield return null;
        } */
        yield return null;
    }

    public void CheckDeath()
    {
        if(current_health <= 0)
        {
            player_anim.SetTrigger("Death");
        }
    }
}
