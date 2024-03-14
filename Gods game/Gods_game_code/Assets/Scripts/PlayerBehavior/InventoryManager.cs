using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    private readonly List<GameObject> items = new();
    private readonly List<GameObject> equipped_items = new();
    private GameObject inventory_bag_grid;
    private GameObject equipped_weapon_grid;
    public GameObject player { get; set;}
    [SerializeField] private GameObject grid_element_prefab;
    [SerializeField] private Canvas inventory;

    private void Awake()
    {
        Events.instance.Canvas_inventory = Instantiate(inventory).gameObject;
        inventory_bag_grid = GameObject.Find("Bag_grid");
        equipped_weapon_grid = GameObject.Find("Equipped_grid");

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(transform.gameObject);
    }
    public void AddItemToBag(GameObject item, Sprite item_sprite)
    {
        items.Add(item);
        InitItemToGrid(item, inventory_bag_grid, item_sprite, "bag_item");
    }

    public void RemoveItemFromBag(GameObject item)
    {
        items.Remove(item);
    }

    public void InitEquippedItem(GameObject equipped_weap, Sprite equipped_sprite)
    {
        equipped_items.Add(equipped_weap);
        InitItemToGrid(equipped_weap, equipped_weapon_grid, equipped_sprite, "equipped_item");
    }

    private void InitItemToGrid(GameObject item, GameObject grid, Sprite item_sprite, string tag)
    {
        var item_to_add = Instantiate(grid_element_prefab, null);
        item_to_add.tag = tag;
        
        var child = item_to_add.transform.GetChild(0);
        child.GetComponent<inventory_element>().item_object = item;
        child.GetComponent<Image>().sprite = item_sprite;
        item_to_add.transform.SetParent(grid.transform);
    }

    //TODO: Bug after equipping slime
    public void EquipItem(GameObject item_object, GameObject cell)
    {
        var parent = cell.transform.parent;
        parent.transform.SetParent(equipped_weapon_grid.transform);
        parent.tag = "equipped_item";

        var currently_equipped = equipped_weapon_grid.transform.GetChild(0);
        currently_equipped.transform.SetParent(inventory_bag_grid.transform);
        currently_equipped.tag = "bag_item";

        var player_body = player.transform.GetChild(0);
        Destroy(player_body.GetChild(0).gameObject);
        
        var new_weap = Instantiate(item_object, player_body);
        new_weap.transform.localPosition = Vector3.zero;
    }
}
