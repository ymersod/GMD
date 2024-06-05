using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
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
    public GameObject player { get; set; }
    private string weapon_bag_tag = "bag_item";
    private string weapon_equipped_tag = "equipped_item";
    private string usable_bag_tag = "bag_use_item";
    private string blessing = "blessing";
    [SerializeField] private GameObject grid_element_prefab;
    [SerializeField] private Canvas inventory;

    private void Awake()
    {
        Events.instance.Canvas_inventory = Instantiate(inventory).gameObject;
        inventory_bag_grid = GameObject.Find("Bag_grid");
        equipped_weapon_grid = GameObject.Find("Equipped_grid");

        if (instance == null)
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

        if (item.CompareTag(usable_bag_tag))
        {
            InitItemToGrid(item, inventory_bag_grid, item_sprite, usable_bag_tag);
        }
        else if (item.CompareTag(blessing))
        {
            InitItemToGrid(item, inventory_bag_grid, item_sprite, blessing);
        }
        else
        {
            InitItemToGrid(item, inventory_bag_grid, item_sprite, weapon_bag_tag);
        }
    }

    public void RemoveItemFromBag(GameObject item)
    {
        items.Remove(item);
    }

    public void InitEquippedItem(GameObject equipped_weap, Sprite equipped_sprite)
    {
        equipped_items.Add(equipped_weap);
        InitItemToGrid(equipped_weap, equipped_weapon_grid, equipped_sprite, weapon_equipped_tag);
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

    public void EquipItem(GameObject item_object, GameObject cell)
    {
        //TEMP
        Destroy(equipped_weapon_grid.transform.GetChild(0).gameObject);

        var parent = cell.transform.parent;
        parent.transform.SetParent(equipped_weapon_grid.transform);
        parent.tag = weapon_equipped_tag;

        //TODO: Unequipping aint working kek
        /* var currently_equipped = equipped_weapon_grid.transform.GetChild(0);
        currently_equipped.transform.SetParent(inventory_bag_grid.transform);
        currently_equipped.tag = weapon_bag_tag; */


        var player_body = player.transform.GetChild(0);
        player_body.GetComponentInChildren<player_attack>().Unsub();
        Destroy(player_body.GetChild(0).gameObject);

        var new_weap = Instantiate(item_object, player_body);
        new_weap.transform.localPosition = Vector3.zero;
    }

    public void UseItem(GameObject item_object, GameObject cell)
    {
        player.transform.GetChild(0).GetComponent<PlayerStatus>().Heal(50);
        RemoveItemFromBag(item_object);
        Destroy(cell.transform.parent.gameObject);
    }

    //TODO: This is not how i want to implement bag, however i dont have time to do inventory navigation on joystick
    public void AcitvateItem()
    {
        var childCount = inventory_bag_grid.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            var firstTag = inventory_bag_grid.transform.GetChild(i).tag;
            var firstItem = inventory_bag_grid.transform.GetChild(i).GetChild(0);
            var item_object = firstItem.GetComponent<inventory_element>().item_object;

            var equippedItem = equipped_weapon_grid.transform.GetChild(0).GetChild(0);
            var equipped_item_object = equippedItem.GetComponent<inventory_element>().item_object;

            if (firstTag == usable_bag_tag)
            {
                UseItem(firstItem.gameObject.GetComponent<inventory_element>().item_object, firstItem.gameObject);
                break;
            }
            else if (firstTag == blessing) continue;
            else if (item_object.GetComponentInChildren<player_attack>().dmg >
                equipped_item_object.GetComponentInChildren<player_attack>().dmg
                && firstTag == weapon_bag_tag)
            {
                EquipItem(item_object, firstItem.gameObject);
                break;
            }
        }
    }

    public bool CountBlessings()
    {
        var count = 0;
        foreach (var item in items)
        {
            if (item.CompareTag(blessing))
            {
                count++;
            }
        }
        print("Blessings: " + count);

        if (count == 5)
        {
            return true;
        }
        return false;
    }
}
