using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowPlayer : MonoBehaviour
{
    private Vector2 prevPos;
    private Vector3 original_pos;
    private Transform playerPos;
    private bool isEnabled = false;
    void OnEnable() => isEnabled = true;

    void OnDisable() => isEnabled = false;

    private void Start()
    {
        original_pos = transform.position;
        playerPos = DrawManager.instance.player.transform.GetChild(0).transform;
    }
    void Update()
    {
        if (!isEnabled) return;
        TransformToPlayer();
    }
    public void TransformToPlayer()
    {
        prevPos = transform.position;
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (transform.position != original_pos) UpdateBrushes(original_pos, prevPos);
            transform.position = original_pos;
            return;
        }

        transform.position = new Vector3(playerPos.position.x, playerPos.position.y);
        UpdateBrushes(transform.position, prevPos);
    }
    private void UpdateBrushes(Vector2 position, Vector2 prevPos)
    {
        var direction = position - prevPos;
        foreach (Transform child in transform)
        {
            child.GetComponent<Brush_logic>().FollowMap(direction);
        }
    }
}
