using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LOS : MonoBehaviour

{
    [SerializeField]
    private Tilemap world_tilemap;
    private Vector2[] los_tiles;
    private Vector2 current_center;
    [SerializeField] 
    private string los_tag;
    [SerializeField] 
    private string target_tag;

    [SerializeField]
    private int los_radius = 5;
    private readonly float tile_size = 1f;
    
    void Start()
    {
        world_tilemap = LOSManager.Instance.GetTilemap();
      
        current_center = CenterTile(tile_size);
        InitLOSGrid(current_center, los_radius, tile_size);
        los_tag = LOSManager.Instance.AddToLOS(los_tiles, los_tag);
        
        var losCheck = GetComponent<LOSCheck>();
        if(losCheck)
        {
            losCheck.los_tag = los_tag;
            losCheck.target_tag = target_tag;
        }
    }

    void Update()
    {
        var new_center = CenterTile(tile_size);
        if(current_center != new_center)
        {
            MoveGrid(new_center, current_center);
            current_center = new_center;
        }
    }

    private void MoveGrid(Vector2 new_center, Vector2 old_center)
    {
        var diff = new_center - old_center;
        for (int i = 0; i < los_tiles.Length; i++)
        {
            los_tiles[i] += diff;
        }
        LOSManager.Instance.UpdateLOS(los_tiles, los_tag);
    }

    private Vector2 CenterTile(float tile_size)
    {
        Vector3 playerPosition = transform.position;
        Vector3Int cellPosition = world_tilemap.WorldToCell(playerPosition);
        return new(cellPosition.x + tile_size / 2, cellPosition.y + tile_size / 2);
    }

    private void InitLOSGrid(Vector2 position,int los_radius, float tile_size)
    {
        //Size of array
        var total = 1;
        for(int i = 1; i <= los_radius; i++)
        {
            var temp = total + (i * 4);
            total = temp;
        }
        los_tiles = new Vector2 [total];

        los_tiles.SetValue(position, 0);
        var lastOrigin = position;
        var index = 1;
        for(int i = 1; i <= los_radius; i++)
        {
            lastOrigin += new Vector2(0, tile_size);
            var currentPos = lastOrigin;
            for(int j = 1; j <= i*4; j++)
            {
                switch (j / (i*4f))
                {
                    case <= 0.25f:
                        currentPos += new Vector2(tile_size, -tile_size);
                        break;
                    case <= 0.5f:
                        currentPos += new Vector2(-tile_size, -tile_size);
                        break;
                    case <= 0.75f:
                        currentPos += new Vector2(-tile_size, tile_size);
                        break;
                    case <= 1f:
                        currentPos += new Vector2(tile_size, tile_size);
                        break;
                }
                los_tiles.SetValue(currentPos, index);
                index++;
            }
        }
    }

    internal (string los_tag, string target_tag) GetTags()
    {
        return (los_tag, target_tag);
    }
}
