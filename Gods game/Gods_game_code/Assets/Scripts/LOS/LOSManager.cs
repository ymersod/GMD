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
    public (bool IsInLOS, Vector3 playerPos) IsInLOS(string char_tag, string target_tag)
    {
        
        var char_los = LOS[char_tag];
        var target_center = Center[target_tag];
        if(char_los.Contains(target_center))
        {
            return (true, target_center);
        }
        return (false, target_center);
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

    public List<Vector3Int> Pursuit(string char_tag, Vector3 target_center)
    {
        var path = new List<Vector3Int>();

        var char_los = LOS[char_tag];
        var self_center = Center[char_tag];

        var diff = target_center - self_center;
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

        var totalXdiff = Math.Abs(diff.x);
        var totalYdiff = Math.Abs(diff.y);
        
        var horizontalPriority = totalXdiff >= totalYdiff;
        path = CanPath(totalXdiff, totalYdiff, moveX, moveY, horizontalPriority, self_center, target_center, char_los, path);
        if(path.Count == 0)
        {
            path = CanPath(totalXdiff, totalYdiff, moveY, moveX, !horizontalPriority, self_center, target_center, char_los, path);
        }
        return path;
    }

    private List<Vector3Int> CanPath(float totalXdiff, float totalYdiff, Vector3Int moveX, Vector3Int moveY, bool horizontalPriority, Vector3 self_center, Vector3 target_center, List<Vector3> self_los_tiles, List<Vector3Int> path)
    {
        var currentPos = self_center;
        var lastPos = currentPos;
        var currentDirection = PathingDirectionEnum.Zero;
        var shouldSave = false;
        
        var localTotalX = totalXdiff;
        var localTotalY = totalYdiff;
        for(int i = 0; i <= totalXdiff + totalYdiff; i++)
        {
            var moveChosen = false;
            if (shouldSave)
            {
                path.Add(Vector3Int.FloorToInt(lastPos));
                shouldSave = false;
            }
            if(localTotalX >= 1 && localTotalY >= 1)
            {
                if(self_los_tiles.Contains(currentPos + moveX) && self_los_tiles.Contains(currentPos + moveY) && self_los_tiles.Contains(currentPos + moveX + moveY))
                {
                    lastPos = currentPos;
                    currentPos += moveX + moveY;
                    localTotalX--;
                    localTotalY--;
                    i++;
                    
                    if(currentDirection != PathingDirectionEnum.Diagonal) shouldSave = true;
                    currentDirection = PathingDirectionEnum.Diagonal;
                    moveChosen = true;
                }
            }
            if(moveChosen == false)
            {
                if(horizontalPriority)
                {
                    if(localTotalX >= 1 && self_los_tiles.Contains(currentPos + moveX))
                    {
                        lastPos = currentPos;
                        currentPos += moveX;
                        localTotalX--;
                        if(currentDirection != PathingDirectionEnum.Horizontal) shouldSave = true;
                        currentDirection = PathingDirectionEnum.Horizontal;
                    }
                else if(localTotalY >= 1 && self_los_tiles.Contains(currentPos + moveY))
                    {
                        lastPos = currentPos;
                        currentPos += moveY;
                        localTotalY--;
                        if(currentDirection != PathingDirectionEnum.Vertical) shouldSave = true;
                        currentDirection = PathingDirectionEnum.Vertical;
                    }
                }
                else
                {
                    if(localTotalY >= 1 && self_los_tiles.Contains(currentPos + moveY))
                    {
                        lastPos = currentPos;
                        currentPos += moveY;
                        localTotalY--;
                        if(currentDirection != PathingDirectionEnum.Vertical) shouldSave = true;
                        currentDirection = PathingDirectionEnum.Vertical;
                    }
                    else if(localTotalX >= 1 && self_los_tiles.Contains(currentPos + moveX))
                    {
                        lastPos = currentPos;
                        currentPos += moveX;
                        localTotalX--;
                        if(currentDirection != PathingDirectionEnum.Horizontal) shouldSave = true;
                        currentDirection = PathingDirectionEnum.Horizontal;
                    }
                }
            }
        
        }
        if(currentPos == target_center)
        {
            path.Add(Vector3Int.FloorToInt(currentPos));
            return path;
        }
        path = new();
        return path;
    }
}
