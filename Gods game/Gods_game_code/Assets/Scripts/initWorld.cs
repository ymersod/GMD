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
        var world = GenerateWorld.Instance.GenerateWorlds();
        
        LOSManager.Instance.ground_tilemap = world.transform.GetChild(0).GetComponent<Tilemap>();

        Instantiate(EnemySpawner_prefab);
        
        //SMTH can be done about this initializer
        var player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerStatus>().enabled = true;
        player.GetComponent<LOS>().enabled = true;
    }
}
