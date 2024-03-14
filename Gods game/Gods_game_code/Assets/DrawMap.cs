using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrawMap : Graphic
{
    public static DrawMap instance;
    [SerializeField] private GameObject brush_prefab;
    public Vector2Int gridSize;
    public List<List<Vector2>> points = new() { new()};
    float width;
    float height;
    float unitWidth;
    float unitHeight;
    public float thickness = 10f;
    protected override void Awake()
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

    //TODO: fix z values when getting urp
    public void Draw(List<RaycastResult> ui_raycast)
    {
        Vector2 mousePosition = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), mousePosition, canvas.worldCamera, out Vector2 localPoint);
        print(localPoint);
        points[^1].Add(localPoint);
        print(points.Count);
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        print("Drawing");
        vh.Clear();

        var cur_drawing = points[^1];
        
        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        unitWidth = width / gridSize.x;
        unitHeight = height / gridSize.y;


        if(cur_drawing.Count < 2)
        {
            return;
        }
        for(int i=0; i<cur_drawing.Count; i++)
        {
            Vector2 point = cur_drawing[i];
            DrawVerticesForPoint(vh, point);
        }
        for(int i=0; i<cur_drawing.Count-1; i++)
        {
            int index = i * 2;
            vh.AddTriangle(index + 0, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index + 0);
        }
        
    }

    private void DrawVerticesForPoint(VertexHelper vh, Vector2 point)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = new Vector3(-thickness / 2 , 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);

        vertex.position = new Vector3(thickness / 2 , 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);
    }

    public void ResetBrush()
    {
        points.Add(new List<Vector2>());
    }
}
