using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.Tilemaps;
public class DrawPathingLOS : MonoBehaviour{
    public Vector3 self_center;
    private List<Vector3> self_los_tiles;
    private List<Vector3> target_los_tiles;
    public Vector3 target_center;
    [SerializeField] private DrawLOS self_los_grid;
    [SerializeField] private DrawLOS target_los_grid;

    void OnDrawGizmos()
    {
        // I think self los tiles is all i need cuz we move after last seen player pos no need for his grid
        if(self_los_grid != null && target_los_grid != null)
        {
            self_los_tiles = self_los_grid.los_tiles;
            target_los_tiles = target_los_grid.los_tiles;
            self_center = self_los_grid.center;
        }

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

        var totalXdiff = diff.x;
        var totalYdiff = diff.y;

        for(int i = 0; i <= Math.Abs(diff.x) + Math.Abs(diff.y); i++)
        {
            if(totalXdiff >= 1 && totalYdiff >= 1)
            {
                CanMoveDiagonal();
            }
            else
            {
                MoveCardinal();
            }
        }
    }

    private void CanMoveDiagonal()
    {
        
    }

    private void MoveCardinal()
    {
       
    }
}