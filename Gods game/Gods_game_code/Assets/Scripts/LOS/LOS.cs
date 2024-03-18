using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LOS : MonoBehaviour

{
    [SerializeField]
    private Tilemap world_tilemap;
    private List<Vector3> los_tiles;
    private List<Vector3Int> no_los_tiles;
    private Vector3 current_center;
    [SerializeField] 
    private string los_tag;
    [SerializeField] 
    private string target_tag;

    [SerializeField]
    private int los_radius = 5;
    private readonly float tile_size = 1f;
    public int var = 0;
    
    void OnEnable()
    {
        InitializeLOS();
    }

    void Update()
    {
        var (centerTile, position) = CenterTile(tile_size);
        if(current_center != centerTile)
        {
            SetLOSGrid(centerTile, position, los_radius, tile_size);
            current_center = centerTile;
            LOSManager.Instance.UpdateLOS(los_tiles, los_tag);
        }
    }

    /* private void MoveGrid(Vector2 new_center, Vector2 old_center)
    {
        var diff = new_center - old_center;
        for (int i = 0; i < los_tiles.Length; i++)
        {
            los_tiles[i] += diff;
        }
        LOSManager.Instance.UpdateLOS(los_tiles, los_tag);
    } */

    private (Vector3 centerTile, Vector3Int position) CenterTile(float tile_size)
    {
        Vector3 playerPosition = transform.position;
        Vector3Int cellPosition = world_tilemap.WorldToCell(playerPosition);
        return (new(cellPosition.x + tile_size / 2, cellPosition.y + tile_size / 2, 0), cellPosition);
    }

    internal (string los_tag, string target_tag) GetTags()
    {
        return (los_tag, target_tag);
    }

    public void InitializeLOS()
    {
        world_tilemap = LOSManager.Instance.ground_tilemap;
      
        var (centerTile, position) = CenterTile(tile_size);
        SetLOSGrid(centerTile, position, los_radius, tile_size);

        los_tag = LOSManager.Instance.AddToLOS(los_tiles, los_tag);
        
        var losCheck = GetComponent<LOSCheck>();
        if(losCheck)
        {
            losCheck.los_tag = los_tag;
            losCheck.target_tag = target_tag;
        }
    }

    private void SetLOSGrid(Vector3 position, Vector3Int centerTile, int los_radius, float tile_size)
    {
        los_tiles = new();
        no_los_tiles = new();

        los_tiles.Add(position);
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
                        CheckForWalls(currentPos, centerTile, no_los_tiles, los_tiles);
                        break;
                    case <= 0.5f:
                        currentPos += new Vector3(-tile_size, -tile_size);
                        CheckForWalls(currentPos, centerTile, no_los_tiles, los_tiles);
                        break;
                    case <= 0.75f:
                        currentPos += new Vector3(-tile_size, tile_size);
                        CheckForWalls(currentPos, centerTile, no_los_tiles, los_tiles);
                        break;
                    case <= 1f:
                        currentPos += new Vector3(tile_size, tile_size);
                        CheckForWalls(currentPos, centerTile, no_los_tiles, los_tiles);
                        break;
                }
            }
        }
    }
    void CheckForWalls(Vector3 currentpos, Vector3Int centerTile, List<Vector3Int> no_los_tiles, List<Vector3> los_tiles)
    {
        var cellPos = world_tilemap.WorldToCell(currentpos);
        if(world_tilemap.HasTile(cellPos))
        {
            if(CanRoute(centerTile, cellPos, no_los_tiles))
            {
                los_tiles.Add(cellPos);
                return;
            }
        }
        no_los_tiles.Add(cellPos);
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
        
        if(Math.Abs(totalXdiff) - Math.Abs(totalYdiff) >= 2)
        {
            if(IsInvalidCorner(cellPos, no_los_tiles)) return false;
        }
        else if(Math.Abs(totalYdiff) - Math.Abs(totalXdiff) >= 2)
        {
            if(IsInvalidCorner(cellPos, no_los_tiles)) return false;
        }
        else if(Math.Abs(totalXdiff) - Math.Abs(totalYdiff) == 0)
        {
            if(IsInvalidDiagonal(cellPos, no_los_tiles, centerTile, moveX, moveY, totalXdiff)) return false;
        }

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
                
                if(moveChosen == moveX) xMoveValid = false;
                else yMoveValid = false;
                
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
    private bool IsInvalidDiagonal(Vector3Int cellPos, List<Vector3Int> no_los_tiles, Vector3Int centerTile, Vector3Int moveX, Vector3Int moveY, float distance)
    {
        var current_testing_cell = cellPos;
        for(int i = 0; i < distance; i++)
        {
            current_testing_cell += moveX;
            current_testing_cell += moveY;
            if(no_los_tiles.Contains(current_testing_cell)) return true;
        }
        return false;
    }

    private bool IsInvalidCorner(Vector3Int cellPos, List<Vector3Int> no_los_tiles)
    {
        var arr = new Vector3Int[4]{
            Vector3Int.up, 
            Vector3Int.right, 
            Vector3Int.down, 
            Vector3Int.left
        };
        
        for(int i = 0; i < arr.Length; i++)
        {
             var current_testing_cell = cellPos;
            if(no_los_tiles.Contains(current_testing_cell + arr[i]))
            {
                return true;
            }
        }
        return false;
    }
}
