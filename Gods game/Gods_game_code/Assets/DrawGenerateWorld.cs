using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawGenerateWorld : MonoBehaviour
{
    [SerializeField] private List<Tilemap> world_segments;
    [SerializeField] private Tilemap center;
    [SerializeField] private float world_segments_count = 1;
    [SerializeField] private Tile floor_tile;
    [SerializeField] private Tile wall_tile;
    private Grid world;
    void OnDrawGizmos()
    {
        if (world_segments != null)
        {
            CreateWorld();
        } 
    }

    private void CreateWorld()
    {   
        if(world == null)
        {
            world = new GameObject("World").AddComponent<Grid>();
            
            var segments = new List<Tilemap>();
            var spawn = Instantiate(center);
            spawn.CompressBounds();
            segments.Add(spawn);
            
            //Bottom left => Counterclock wise
            var bounds = new (Vector3Int, DiagonalDirections)[4]{
                (new(spawn.cellBounds.xMin, spawn.cellBounds.yMin, 0), DiagonalDirections.UpRight),
                (new(spawn.cellBounds.xMax, spawn.cellBounds.yMin, 0), DiagonalDirections.UpLeft),
                (new(spawn.cellBounds.xMax, spawn.cellBounds.yMax, 0), DiagonalDirections.DownLeft),
                (new(spawn.cellBounds.xMin, spawn.cellBounds.yMax, 0), DiagonalDirections.DownRight)
            };
            FixWalls(spawn, (Vector3Int.zero, DiagonalDirections.Zero));
            

            ShuffleList(world_segments);
            for (int i = 0; i < world_segments_count; i++)
            {
                 /*  var segment = Instantiate(world_segments[UnityEngine.Random.Range(0, world_segments.Count)]); */
                var segment = Instantiate(world_segments[i]);
                FixWalls(segment, bounds[i]);
                segments.Add(segment);
            }
            var ground = CreateGround("Ground");
            var ground_cosmetic = CreateGround("Ground_cosmetic");

           /*  FillGround(spawn, ground, ground_cosmetic); */
            var worldMaxBoundsX = 0;
            var worldMaxBoundsY = 0;
            var worldMinBoundsX = 0;
            var worldMinBoundsY = 0;
            foreach (var seg in segments)
            {
                if(seg.cellBounds.yMax > worldMaxBoundsY) worldMaxBoundsY = seg.cellBounds.yMax;
                if(seg.cellBounds.xMax > worldMaxBoundsX) worldMaxBoundsX = seg.cellBounds.xMax;
                if(seg.cellBounds.yMin < worldMinBoundsY) worldMinBoundsY = seg.cellBounds.yMin;
                if(seg.cellBounds.xMin < worldMinBoundsX) worldMinBoundsX = seg.cellBounds.xMin;
            }
            
            var worldMinBounds = new Vector3Int(worldMinBoundsX, worldMinBoundsY);
            var worldMaxBounds = new Vector3Int(worldMaxBoundsX, worldMaxBoundsY);
            FillGround(worldMinBounds, worldMaxBounds, ground, ground_cosmetic, segments);

            CreateEdgeWalls(worldMinBounds, worldMaxBounds, wall_tile);
        }
    }

    private void CreateEdgeWalls(Vector3Int worldMinBounds, Vector3Int worldMaxBounds, Tile edgeWall)
    {
        var edge_walls = new GameObject("EdgeWalls");
        edge_walls.AddComponent<Tilemap>();
        edge_walls.AddComponent<TilemapRenderer>();
        var edge_tilemap = edge_walls.GetComponent<Tilemap>();

        SetupComponentsWalls(edge_tilemap);

        var route = new (Vector3Int, Vector3Int)[4]{
            (new Vector3Int(worldMinBounds.x, worldMinBounds.y), Vector3Int.right),
            (new Vector3Int(worldMaxBounds.x, worldMinBounds.y), Vector3Int.up),
            (new Vector3Int(worldMaxBounds.x, worldMaxBounds.y), Vector3Int.left),
            (new Vector3Int(worldMinBounds.x, worldMaxBounds.y), Vector3Int.down)
        };

        for(int i = 0; i < route.Length; i++)
        {
            var (start, direction) = route[i];
            var next = Vector3Int.zero;
            if (i == route.Length - 1)
            {
                var (nextP, _) = route[0];
                next = nextP;
            }
            else {
                var (nextP, _) = route[i+1];
                next = nextP;
            }
            
            var diff = 0;
            if(direction == Vector3Int.right) diff = next.x - start.x;
            if(direction == Vector3Int.left) diff = start.x - next.x;
            if(direction == Vector3Int.up) diff = next.y - start.y;
            if(direction == Vector3Int.down) diff = start.y - next.y;

            var currentPos = start;
            for(int j = 0; j < diff; j++)
            {
                currentPos += direction;
                edge_tilemap.SetTile(currentPos, edgeWall);
            }
        }
    }

    private void FillGround(Vector3Int minBounds, Vector3Int maxBounds, Tilemap ground, Tilemap ground_cosmetic, List<Tilemap> segments)
    {
        for(int x = minBounds.x; x < maxBounds.x; x++)
        {
            for(int y = minBounds.y; y < maxBounds.y; y++)
            {
                var pos = new Vector3Int(x, y, 0);
                if (!IsWall(segments, pos))
                {
                    ground.SetTile(pos, floor_tile);
                }
                else
                {
                    ground_cosmetic.SetTile(pos, floor_tile);
                }
            }
        }
    }

    private bool IsWall(List<Tilemap> segment, Vector3Int pos)
    {
        foreach (var seg in segment)
        {
            if (seg.HasTile(pos))
            {
                return true;
            }
        }
        return false;
    }

    private Tilemap CreateGround(string Name)
    {
        var ground = new GameObject(Name).AddComponent<Tilemap>();
        ground.AddComponent<TilemapRenderer>();

        ground.transform.SetParent(world.transform);
        ground.gameObject.GetComponent<TilemapRenderer>().sortingLayerName = "Ground";
        return ground;
    }
    private Tilemap FixWalls(Tilemap segment_prefab, (Vector3Int point, DiagonalDirections self_corner) bounds)
    {
            var segment = SetupComponentsWalls(segment_prefab);

      /*   if(bounds.self_corner == DiagonalDirections.Zero) return segment; */

        var corner_pos = bounds.self_corner switch
        {
            DiagonalDirections.UpRight => new Vector3Int(segment.cellBounds.xMax, segment.cellBounds.yMax, 0),
            DiagonalDirections.UpLeft => new Vector3Int(segment.cellBounds.xMin, segment.cellBounds.yMax, 0),
            DiagonalDirections.DownLeft => new Vector3Int(segment.cellBounds.xMin, segment.cellBounds.yMin, 0),
            DiagonalDirections.DownRight => new Vector3Int(segment.cellBounds.xMax, segment.cellBounds.yMin, 0),
            _ => Vector3Int.zero
        };

        var difference = bounds.point - corner_pos;
        print(difference);
        foreach (var pos in segment.cellBounds.allPositionsWithin)
        {
            if (segment.HasTile(pos))
            {
                var new_pos = pos + difference;
                // Store the tile temporarily before moving it
                var tile = segment.GetTile(pos);
                // Clear the original tile position
                segment.SetTile(pos, null);
                // Set the tile at the new position
                segment.SetTile(new_pos, tile);
            }
        }
        segment.CompressBounds();
        return segment;
    }

    private Tilemap SetupComponentsWalls(Tilemap tilemap)
    {
        tilemap.CompressBounds();
        tilemap.gameObject.AddComponent<TilemapCollider2D>();
        tilemap.gameObject.AddComponent<Rigidbody2D>();
        tilemap.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        tilemap.gameObject.AddComponent<CompositeCollider2D>();
        tilemap.gameObject.GetComponent<TilemapCollider2D>().usedByComposite = true;

        tilemap.transform.SetParent(world.transform);
        tilemap.gameObject.GetComponent<TilemapRenderer>().sortingLayerName = "Ground_top";

        return tilemap;
    }

    void OnDestroy()
    {
        DestroyImmediate(GameObject.Find("World"));
    }

    private List<Tilemap> ShuffleList(List<Tilemap> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
        return list;
    }
}
