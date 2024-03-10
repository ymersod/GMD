using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LOSManager : MonoBehaviour
{
    public static LOSManager Instance { get; private set; }
    private Dictionary<string, Vector2[]> LOS = new();
    private Transform player_transform;
    [SerializeField]
    private Tilemap ground_tilemap;
    private int index = 1;

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

    public string AddToLOS(Vector2[] los, string tag)
    {
        if (LOS.Keys.Contains(tag))
        {
            tag += index;
            index++;
            LOS.Add(tag, los);
            return tag;
        }
        else LOS.Add(tag, los);
        return tag;
    }
    public void AddPlayer(Transform player)
    {
        player_transform = player;
    }
    public Transform GetPlayerTransform()
    {
        return player_transform;
    }
    public void UpdateLOS(Vector2[] los, string tag)
    {
        LOS[tag] = los;
    }
    public bool IsInLOS(string char_tag, string target_tag)
    {
        if (LOS.ContainsKey(char_tag) && LOS.ContainsKey(target_tag))
        {
            var char_los = LOS[char_tag];
            var target_los = LOS[target_tag];
            foreach (var char_tile in char_los)
            {
                foreach (var target_tile in target_los)
                {
                    if (char_tile == target_tile)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public Tilemap GetTilemap()
    {
        return ground_tilemap;
    }
}
