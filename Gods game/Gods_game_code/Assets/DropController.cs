using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DropController : MonoBehaviour
{
    private Vector3 original_position;
    [SerializeField]private float speed = 1f;
    [SerializeField] private float height = .5f;
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
    
}
