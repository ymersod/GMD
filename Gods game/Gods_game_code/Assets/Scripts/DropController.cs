using System;
using UnityEngine;

public class DropController : MonoBehaviour
{
    private Vector3 original_position;
    [SerializeField]private float speed = 1f;
    [SerializeField] private float height = .5f;
    [SerializeField] private GameObject weapon_prefab;
    void Start()
    {
        original_position = transform.position;
    }

    void Update()
    {
        var current_position = transform.position;
        
        float newY = original_position.y + height * Mathf.Sin(Time.time * speed);
        
        transform.position = new Vector3(current_position.x, newY, current_position.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            InventoryManager.instance.AddItemToBag(weapon_prefab, gameObject.GetComponent<SpriteRenderer>().sprite);
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
    
}
