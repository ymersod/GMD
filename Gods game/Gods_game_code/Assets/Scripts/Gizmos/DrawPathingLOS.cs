using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Tilemaps;
public class DrawPathingLOS : MonoBehaviour{
    private Vector3 self_center;
    public List<Vector3> self_los_tiles;
    public Vector3 target_center;
    [SerializeField] private Color color = Color.red;
    [SerializeField] private float size = .1f;
    public List<Vector3> path;

    internal void Setup(List<Vector3> self_los_tiles, Vector3 self_center)
    {
        this.self_los_tiles = self_los_tiles;
        this.self_center = self_center;
        PathingLOS();
        if(path.Count != 0)DrawPath();
    }

    private void DrawPath()
    {
        var prev_tile = path[0];
        Gizmos.color = color;
        for(int i = 1; i < path.Count; i++)
        {
            Gizmos.DrawLine(ConvertToGizmo(prev_tile), ConvertToGizmo(path[i]));
            prev_tile = path[i];
        }
    }

    private void PathingLOS()
    {
        path = new();

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
        if(!CanPath(totalXdiff, totalYdiff, moveX, moveY, horizontalPriority)) CanPath(totalXdiff, totalYdiff, moveY, moveX, !horizontalPriority);
    }

    private bool CanPath(float totalXdiff, float totalYdiff, Vector3Int moveX, Vector3Int moveY, bool horizontalPriority)
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
            if(shouldSave)
            {
                path.Add(lastPos);
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
            path.Add(currentPos);
            return true;
        }
        path = new();
        return false;
    }

    private Vector3 ConvertToGizmo(Vector3 toConvert)
    {
        var tile_size = 1f;
        return new Vector3(toConvert.x + tile_size / 2, toConvert.y + tile_size / 2, 0);
    }
}