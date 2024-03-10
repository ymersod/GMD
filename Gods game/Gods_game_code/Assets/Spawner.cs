using System;
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
   public static Spawner Instance { get; private set; }

   void Awake()
   {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
   }

   public void SpawnQuickCastIndicator(Vector2 start, Vector2 end, float duration, GameObject prefab, float length, float width)
   {
        var indicator = Instantiate(prefab, start, Quaternion.Euler(0, 0, 0));
        var indicator_sprite = indicator.transform.GetChild(0).gameObject;
        
        indicator_sprite.transform.localScale = new Vector3(length, width, 1);
        var xOffsetSprite = indicator_sprite.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        indicator_sprite.transform.position += new Vector3(xOffsetSprite, 0);
        
        var indicator_sprite_mask = indicator_sprite.transform.GetChild(0).gameObject;
        var xOffsetMask = indicator_sprite_mask.GetComponent<SpriteRenderer>().localBounds.size.x;
        var maskLocalPosition = indicator_sprite_mask.transform.localPosition;
        indicator_sprite_mask.transform.localPosition += new Vector3(-xOffsetMask, 0);

        var angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
        indicator.transform.rotation = Quaternion.Euler(0, 0, angle);
        
        StartCoroutine(MoveIndicator(indicator_sprite_mask.transform.localPosition, maskLocalPosition, duration, indicator_sprite_mask));
   }

   private IEnumerator MoveIndicator(Vector3 start, Vector3 end, float duration, GameObject indicator_mask)
   {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            indicator_mask.transform.localPosition = Vector3.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        indicator_mask.transform.localPosition = end;
        Destroy(indicator_mask.transform.parent.transform.parent.gameObject);
   }

    public IEnumerator SpawnPoop(Vector2 player, Vector2 self, float poop_speed, float poop_size, float poop_dmg, GameObject poop_prefab)
    {
        var poop = Instantiate(poop_prefab, self, Quaternion.Euler(0, 0, 0));
        
        poop.transform.localScale = new Vector3(poop_size, poop_size, 1);
        
        var direction = player - self;

        poop.transform.GetChild(0).GetComponent<PoopScript>().SetPoopStats(poop_dmg, 0, direction);
        
        float elapsedTime = 0;
        while(elapsedTime < 1f && poop != null)
        {
            poop.transform.position += poop_speed * Time.deltaTime * Vector3.Normalize((Vector3)direction);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (poop != null) Destroy(poop);
    }

    public IEnumerator SpawnFireBall(Vector2 direction, Vector2 playerPosition, float speed, float size, float dmg, float knockback, GameObject fire_ballPrefal)
    {
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var fireball = Instantiate(fire_ballPrefal, playerPosition, Quaternion.Euler(0, 0, angle));

        fireball.transform.localScale = new Vector3(size, size, 1);

        fireball.transform.GetChild(0).GetComponent<FireBallScript>().SetStats(dmg, knockback, direction);

        float elapsedTime = 0;
        while(elapsedTime < 1f && fireball != null)
        {
            fireball.transform.position += speed * Time.deltaTime * Vector3.Normalize((Vector3)direction);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (fireball != null) Destroy(fireball);
    }
}
