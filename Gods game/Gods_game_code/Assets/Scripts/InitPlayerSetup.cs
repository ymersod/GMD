using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPlayerSetup : MonoBehaviour
{
    private static InitPlayerSetup Instance;
    [SerializeField] private GameObject player_prefab;
    [SerializeField] private GameObject starterWeap_prefab;
    [SerializeField] private GameObject eventManager;
    [SerializeField] private GameObject uiEventManager;
    [SerializeField] private GameObject inventoryManager;
    [SerializeField] private GameObject BaseMap;
    [SerializeField] private GameObject SceneManager;
    [SerializeField] private GameObject mapManager;
    [SerializeField] private GameObject combatSpawner;

    //TODO Needs to handle loading at some point
    //TODO Handle player not needing to stay active through scenes
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Instantiate(BaseMap);
        }
    }

    void Initialize()
    {
        //Player stuff
        var player = Instantiate(player_prefab);
        var new_weap = Instantiate(starterWeap_prefab, player.transform.GetChild(0).transform);
        new_weap.transform.localPosition = Vector3.zero;

        //Managers - i wanted the eventmanagers to act seperately from the player but this inputsystem is garbage ...
        //Instantiate(eventManager);
        //Instantiate(uiEventManager);
        Instantiate(inventoryManager);
        Instantiate(SceneManager);
        Instantiate(mapManager);
        Instantiate(combatSpawner);
        Events.instance.player = player;
        UIEvents.instance.player = player;


        InventoryManager.instance.player = player;
        InventoryManager.instance.InitEquippedItem(new_weap, new_weap.transform.GetComponentInChildren<SpriteRenderer>(true).sprite);

        DrawManager.instance.player = player;

        Instantiate(BaseMap);

        DontDestroyOnLoad(transform.gameObject);

    }


}
