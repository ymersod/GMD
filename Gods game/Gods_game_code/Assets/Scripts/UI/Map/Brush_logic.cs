using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Brush_logic : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    public void SetPosition(Vector2 position)
    {
        if(!CanAppend(position)) return;

        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
    }

    private bool CanAppend(Vector2 position)
    {
        if(lineRenderer.positionCount == 0) return true;
        
        return Vector2.Distance(lineRenderer.GetPosition(lineRenderer.positionCount -1), position) > DrawManager.RESOLUTION;
    }
    public void FollowMap(Vector2 direction)
    {
        Vector3[] positions = new Vector3[lineRenderer.positionCount];

        lineRenderer.GetPositions(positions);

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] += (Vector3)direction;
        }

        lineRenderer.SetPositions(positions);
    }
}
