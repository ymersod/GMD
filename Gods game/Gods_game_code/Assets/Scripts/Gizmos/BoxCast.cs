using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoxCastVisualizer : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D boxCollider;
    void OnDrawGizmos()
    {
        DrawBoxCast(boxCollider.bounds.min, boxCollider.bounds.max);
    }
    void DrawBoxCast(Vector2 start, Vector2 end)
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(start, new Vector2(start.x, end.y));
        Gizmos.DrawLine(new Vector2(start.x, end.y), end);
        Gizmos.DrawLine(end, new Vector2(end.x,start.y));
        Gizmos.DrawLine(new Vector2(end.x, start.y), start);
    }
}
