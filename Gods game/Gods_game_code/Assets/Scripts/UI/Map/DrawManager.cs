using System;
using System.Collections;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    public static DrawManager instance;
    private Camera mainCamera;
    [SerializeField] GameObject brushPrefab;
    public GameObject player;
    private GameObject currentBrush;
    public const float RESOLUTION = .1f;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [SerializeField] private GameObject BrushGUIEditFlags;


    internal void StartBrush()
    {
        mainCamera = Camera.main;
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        currentBrush = Instantiate(brushPrefab, mousePosition, Quaternion.identity);

    }

    internal void Draw(Vector2 topLeft, Vector2 bottomRight, GameObject map)
    {  
        print(player.transform.position);
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if(
            mousePos.x > topLeft.x &&
            mousePos.x < bottomRight.x &&
            mousePos.y < topLeft.y &&
            mousePos.y > bottomRight.y)
        {
            var brush = currentBrush.GetComponent<Brush_logic>();
            brush.SetPosition(mousePos);
        }
    }

    internal void ResetBrush(GameObject map)
    {
        currentBrush.transform.SetParent(map.transform);
    }
}
