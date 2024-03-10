using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawLOS : MonoBehaviour
{
    [SerializeField]
    private Transform char_position;
    [SerializeField]
    private float radius = .2f;
    [SerializeField]
    private int los_radius = 5;
    [SerializeField]
    private Tilemap world_tilemap;
    [SerializeField]
    private Color color = Color.red;

    void OnDrawGizmos()
    {
        if(world_tilemap != null)
        {
            Vector3 playerPosition = transform.position;
            Vector3Int cellPosition = world_tilemap.WorldToCell(playerPosition);

            var tile_size = 1f;
            Vector3 center_tile = new(cellPosition.x + tile_size / 2, cellPosition.y + tile_size / 2, 0);
            DrawLineOfSight(center_tile, radius, los_radius, tile_size);
        }

    }
    void DrawLineOfSight(Vector3 position, float radius, int los_radius, float tile_size)
    {
        Gizmos.color = color;
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
                        Gizmos.DrawSphere(currentPos, radius);
                        break;
                    case <= 0.5f:
                        currentPos += new Vector3(-tile_size, -tile_size);
                        Gizmos.DrawSphere(currentPos, radius);
                        break;
                    case <= 0.75f:
                        currentPos += new Vector3(-tile_size, tile_size);
                        Gizmos.DrawSphere(currentPos, radius);
                        break;
                    case <= 1f:
                        currentPos += new Vector3(tile_size, tile_size);
                        Gizmos.DrawSphere(currentPos, radius);
                        break;
                }
            }
        }
    }
}
