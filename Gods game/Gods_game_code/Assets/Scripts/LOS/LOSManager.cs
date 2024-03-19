using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LOSManager : MonoBehaviour
{
    public static LOSManager Instance { get; private set; }
    private Dictionary<string, List<Vector3>> LOS = new();
    private Dictionary<string, Vector3> Center = new();
    private GameObject player;
    public Tilemap ground_tilemap { get; set; }
    private int index = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public string AddToLOS(List<Vector3> los_tiles, Vector3 center, string tag)
    {
        if (LOS.Keys.Contains(tag))
        {
            tag += index;
            index++;
            LOS.Add(tag, los_tiles);
            Center.Add(tag, center);
            return tag;
        }
        else{ 
            LOS.Add(tag, los_tiles);
            Center.Add(tag, center);
        }
        return tag;
    }
    public Transform GetPlayerTransform()
    {
        return player.transform;
    }
    public void UpdateLOS(List<Vector3> los, Vector3 center, string tag)
    {
        LOS[tag] = los;
        Center[tag] = center;
    }
    public (bool isInLOS, Vector3 targetPos) IsInLOS(string char_tag, string target_tag)
    {
        
        var char_los = LOS[char_tag];
        var target_center = Center[target_tag];
        if(char_los.Contains(target_center))
        {
            return (true, target_center);
        }
        return (false, target_center);
    }

    public Vector3[] Pursuit(string char_tag, string target_tag, Vector3 self_pos)
    {
        var char_los = LOS[char_tag];
        var target_los = LOS[target_tag];
        var target = Center[target_tag];
        Vector3Int cellPosition = ground_tilemap.WorldToCell(self_pos);
        
        var diff = target - cellPosition;
        var moveX = diff.x switch
        {
            > 0 => Vector3Int.right,
            < 0 => Vector3Int.left,
            _ => Vector3Int.zero
        };
        
        var moveY = diff.y switch
        {
            > 0 => Vector3Int.up,
            < 0 => Vector3Int.down,
            _ => Vector3Int.zero
        };

        var totalXdiff = diff.x;
        var totalYdiff = diff.y;

        for(int i = 0; i <= Math.Abs(diff.x) + Math.Abs(diff.y); i++)
        {

        }

        return null;
    }

    public bool ShouldPursuit(string char_tag, string target_tag)
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
        return true;
    }
}
