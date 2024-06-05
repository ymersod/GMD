using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Ui_image_controller : MonoBehaviour
{
    [SerializeField] private GameObject map;
    private SpriteRenderer map_renderer;
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    InputAction ui_actions;
    private OpenUIEnum currentUI;
    private bool isDrawing = false;
    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();
        var playerInput = GetComponent<PlayerInput>();
        Events.instance.Map += () => HandleUIEnabling("map_placeholder");
        Events.instance.Inventory += () => HandleUIEnabling("inventory_image");
        UIEvents.instance.Click += () => BagClick();
        UIEvents.instance.Close += () => HandleUIEnabling("NoUiOpen");

        map = Instantiate(map);
        map_renderer = map.GetComponent<SpriteRenderer>();


        currentUI = OpenUIEnum.Bag;

        var actionMap = playerInput.actions.FindActionMap("UI");
        ui_actions = actionMap.FindAction("Click");

        //TODO: use for other actions later
        // Drawing doesnt work atm - dunno what made it stop working :( will fix later :)
        // Trying new approach with the UIEvents
        ui_actions.started += context =>
        {
            if (currentUI == OpenUIEnum.Map)
            {
                isDrawing = true;
                StartCoroutine(StartDraw());
            }
        };

        ui_actions.performed += context =>
        {
            switch (currentUI)
            {
                case OpenUIEnum.Bag:
                    BagClick(); break;
                default: break;
            }
        };

        ui_actions.canceled += context => { isDrawing = false; };

        m_PointerEventData = new PointerEventData(m_EventSystem);
        DontDestroyOnLoad(transform.gameObject);
        HandleUIEnabling(OpenUIEnum.NoUiOpen.ToString());
    }

    void BagClick()
    {
        print("bag click");
        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        foreach (RaycastResult result in UiRayCast())
        {
            if (result.gameObject.CompareTag("bag_item") || result.gameObject.CompareTag("bag_use_item"))
            {
                result.gameObject.transform.GetChild(0).GetComponent<inventory_element>().Click(result.gameObject.tag);
            }
        }
    }

    void ActivateItem()
    {
        print("activate item");
        if (currentUI == OpenUIEnum.Bag)
            InventoryManager.instance.AcitvateItem();
    }

    private List<RaycastResult> UiRayCast()
    {
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);
        return results;
    }
    IEnumerator StartDraw()
    {
        DrawManager.instance.StartBrush();
        while (isDrawing)
        {
            var topLeft = new Vector2(map_renderer.bounds.min.x, map_renderer.bounds.max.y);
            var bottomRight = new Vector2(map_renderer.bounds.max.x, map_renderer.bounds.min.y);
            DrawManager.instance.Draw(topLeft, bottomRight, map);
            yield return new WaitForEndOfFrame();
        }
        DrawManager.instance.ResetBrush(map);
        yield return null;
    }

    private void HandleUIEnabling(string ui_to_open)
    {
        var active_ui = null as GameObject;
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(false);
            }
            else if (child.gameObject.name == ui_to_open)
            {
                var is_inventory_active = child.gameObject.activeInHierarchy;
                child.gameObject.SetActive(!is_inventory_active);
                if (child.gameObject.activeInHierarchy) active_ui = child.gameObject;
            }
        }
        HandleMap(SetUiStatus(active_ui));
    }

    private OpenUIEnum SetUiStatus(GameObject active_ui)
    {
        if (active_ui == null)
        {
            currentUI = OpenUIEnum.NoUiOpen;
            return currentUI;
        }
        currentUI = active_ui.name switch
        {
            "inventory_image" => OpenUIEnum.Bag,
            "map_placeholder" => OpenUIEnum.Map,
            _ => OpenUIEnum.NoUiOpen
        };
        return currentUI;
    }

    private void HandleMap(OpenUIEnum active_ui)
    {
        if (active_ui == OpenUIEnum.Map)
        {
            map.SetActive(true);
        }
        else map.SetActive(false);
    }
}
