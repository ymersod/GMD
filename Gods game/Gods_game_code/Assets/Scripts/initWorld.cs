using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class initWorld : MonoBehaviour
{
    private static initWorld Instance;
    [SerializeField] private GameObject EnemySpawner_prefab;
    [SerializeField] private GameObject LOSManager_prefab;
    [SerializeField] private GameObject WorldGenerator_prefab;
    [SerializeField] private GameObject GridMap_prefab;
    [SerializeField] private GameObject Portal;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else Initialize();
    }

    private void Initialize()
    {
        Instantiate(LOSManager_prefab);
        Instantiate(WorldGenerator_prefab);

        GenerateWorld.instance.CreateWorld();
        LOSManager.Instance.ground_tilemap = GetTileMap(GenerateWorld.instance.ground_name);

        var portal = Instantiate(Portal);
        portal.transform.position = GetTileMap(GenerateWorld.instance.spawn_name).cellBounds.center;

        Instantiate(EnemySpawner_prefab);
   
        //SMTH can be done about this initializer
        var player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerStatus>().enabled = true;
        player.GetComponent<LOS>().enabled = true;
        
    }

    private Tilemap GetTileMap(string name) => GameObject.Find(name).GetComponent<Tilemap>();
    
}
