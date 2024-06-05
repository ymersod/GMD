using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] private GameObject slime_prefab;
    [SerializeField] private GameObject pig_prefab;
    [SerializeField] private int minCount = 10;
    [SerializeField] private int maxCount = 30;
    [SerializeField] private int spawn_spawn_count = 2;
    private List<Tilemap> spawn_areas;
    private Tilemap spawn_spawn_area;
    private Grid world;
    void Awake()
    {
        spawn_areas = new();
        world = GameObject.Find(GenerateWorld.instance.world_name).GetComponent<Grid>();
        DetermineSpawnAreas(world);
        
        var parent = new GameObject();
        foreach (var area in spawn_areas)
        {
            Spawn(area, parent);
        }
        SlimesInSpawn(spawn_spawn_area, parent);
    }

    private void Spawn(Tilemap spawn_area, GameObject parent)
    {
        spawn_area.CompressBounds();
        var amount = UnityEngine.Random.Range(minCount, maxCount);
        for (int i = 0; i < amount; i++)
        {
            var position = new Vector3Int(UnityEngine.Random.Range(spawn_area.cellBounds.xMin, spawn_area.cellBounds.xMax), UnityEngine.Random.Range(spawn_area.cellBounds.yMin, spawn_area.cellBounds.yMax), 0);
            var tile = spawn_area.GetTile(position);

            if (tile == null && SurroundingSpaceIsValid(spawn_area))
            {
                var enemy = Instantiate(UnityEngine.Random.Range(0, 2) == 0 ? slime_prefab : pig_prefab);
                enemy.transform.position = spawn_area.GetCellCenterWorld(position);
                enemy.transform.parent = parent.transform;
            }
            else i--;
        }
    }
    private void SlimesInSpawn(Tilemap spawn_spawn_area, GameObject parent)
    {
        spawn_spawn_area.CompressBounds();

        var amount = spawn_spawn_count;
        for (int i = 0; i < amount; i++)
        {
            var position = new Vector3Int(UnityEngine.Random.Range(spawn_spawn_area.cellBounds.xMin, spawn_spawn_area.cellBounds.xMax), UnityEngine.Random.Range(spawn_spawn_area.cellBounds.yMin, spawn_spawn_area.cellBounds.yMax), 0);
            var tile = spawn_spawn_area.GetTile(position);

            if (tile == null && SurroundingSpaceIsValid(spawn_spawn_area))
            {
                var enemy = Instantiate(slime_prefab);
                enemy.transform.position = spawn_spawn_area.GetCellCenterWorld(position);
                enemy.transform.parent = parent.transform;
            }
            else i--;
        }
    }

    //TODO: I dont know what my plan is but i will figure it out
    private bool SurroundingSpaceIsValid(Tilemap spawn_area)
    {
        return true;   
    }

    void DetermineSpawnAreas(Grid world)
    {
        foreach(Transform child in world.transform)
        {
            print(child.name);
            if(child.CompareTag("EnemySpawnArea"))
            {
                var child_tilemap = child.gameObject.GetComponent<Tilemap>();
                spawn_areas.Add(child_tilemap);
            }
            if(child.CompareTag("spawn"))
            {
                spawn_spawn_area = child.gameObject.GetComponent<Tilemap>();
                
            }
        }
    }
}
