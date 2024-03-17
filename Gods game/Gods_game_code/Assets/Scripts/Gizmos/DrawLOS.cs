using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawLOS : MonoBehaviour
{
    [SerializeField] private Transform char_position;
    [SerializeField] private float radius = .2f;
    [SerializeField] private int los_radius = 5;
    [SerializeField] private Tilemap world_tilemap;
    [SerializeField] private Color los_color = Color.red;
    [SerializeField] private Color no_los_color = Color.blue;
    [SerializeField] private int corner_LOS_size = 2;

    void OnDrawGizmos()
    {
        print(Vector3Int.right - Vector3Int.right*2);
        print(Vector3Int.down - Vector3Int.down*2);
        if(world_tilemap != null)
        {
            Vector3 playerPosition = transform.position;
            Vector3Int cellPosition = world_tilemap.WorldToCell(playerPosition);

            var tile_size = 1f;
            Vector3 center_tile = new(cellPosition.x + tile_size / 2, cellPosition.y + tile_size / 2, 0);
            
            DrawLineOfSight(center_tile, cellPosition, radius, los_radius, tile_size);
        }

    }
    void DrawLineOfSight(Vector3 position, Vector3Int centerTile, float radius, int los_radius, float tile_size)
    {
        List<Vector3Int> no_los_tiles = new();
        Gizmos.color = los_color;
        Gizmos.DrawSphere(position, radius);
        var lastOrigin = position;
        for(int i = 1; i <= los_radius; i++)
        {
            lastOrigin += new Vector3(0, tile_size);
            var currentPos = lastOrigin;
            for(int j = 1; j <= i*4; j++)
            {
                
                switch (j / (i*4f))
                {
                    case <= 0.25f:
                        currentPos += new Vector3(tile_size, -tile_size);
                        CheckForWalls(currentPos, radius, centerTile, no_los_tiles);
                        break;
                    case <= 0.5f:
                        currentPos += new Vector3(-tile_size, -tile_size);
                        CheckForWalls(currentPos, radius, centerTile, no_los_tiles);
                        break;
                    case <= 0.75f:
                        currentPos += new Vector3(-tile_size, tile_size);
                        CheckForWalls(currentPos, radius, centerTile, no_los_tiles);
                        break;
                    case <= 1f:
                        currentPos += new Vector3(tile_size, tile_size);
                        CheckForWalls(currentPos, radius, centerTile, no_los_tiles);
                        break;
                }
            }
        }
    }

    void CheckForWalls(Vector3 currentpos, float radius, Vector3Int centerTile, List<Vector3Int> no_los_tiles)
    {
        var cellPos = world_tilemap.WorldToCell(currentpos);
        if(world_tilemap.HasTile(cellPos))
        {
            if(CanRoute(centerTile, cellPos, no_los_tiles))
            {
                Gizmos.color = los_color;
                Gizmos.DrawSphere(currentpos, radius);
                return;
            }
        }
        no_los_tiles.Add(cellPos);
        Gizmos.color = no_los_color;
        Gizmos.DrawSphere(currentpos, radius);
        
    }

    private bool CanRoute(Vector3Int centerTile, Vector3Int cellPos, List<Vector3Int> no_los_tiles)
    {
        var diff = centerTile - cellPos;
        
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
        
        var currentPos = cellPos;
        var xMoveValid = moveX != Vector3Int.zero;
        var yMoveValid = moveY != Vector3Int.zero;
        for(int i = 0; i <= Math.Abs(diff.x) + Math.Abs(diff.y); i++)
        {
            var prevTotalXdiff = totalXdiff;
            var prevTotalYdiff = totalYdiff;
            var moveChosen = Vector3Int.zero;
            //Moving
            if(currentPos == centerTile) return true;
            if(totalXdiff != 0 && moveX != Vector3Int.zero && xMoveValid)
            {
                currentPos += moveX;
                totalXdiff -= moveX.x;
                moveChosen = moveX;
            }
            else if(totalYdiff != 0 && moveY != Vector3Int.zero && yMoveValid)
            {
                currentPos += moveY;
                totalYdiff -= moveY.y;
                moveChosen = moveY;
            }

            //Checking to see if move is valid
            if(no_los_tiles.Contains(currentPos))
            {
                var oppositeDirection = moveChosen - (moveChosen*2);
                currentPos += oppositeDirection;
                totalXdiff = prevTotalXdiff;
                totalYdiff = prevTotalYdiff;
                
                if(moveChosen == moveX) 
                {
                    if(Math.Abs(totalYdiff) - Math.Abs(totalXdiff) >= corner_LOS_size) return false;
                    xMoveValid = false;
                }
                else
                {
                    if(Math.Abs(totalXdiff) - Math.Abs(totalYdiff) >= corner_LOS_size) return false;
                    yMoveValid = false;
                }
                i--;
            }
            else
            {
                if(moveChosen == moveY) xMoveValid = true;
                else if(moveChosen == moveX) yMoveValid = true;
                
            }

            if(!xMoveValid && !yMoveValid) return false;
        }
        return false;
    }
}
