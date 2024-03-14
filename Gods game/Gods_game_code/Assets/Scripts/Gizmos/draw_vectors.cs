using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;

public class draw_vectors : MonoBehaviour
{  
    [SerializeField] private Transform target;
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 direction = target.position - transform.position;

        // Step 2: Rotate the translated vector by 90 degrees
        Vector3 rotatedVector = new Vector2(-direction.y, direction.x);
        Vector3 rotatedVectorOpposite = new Vector2(direction.y, -direction.x);

        // Step 3: Translate back to the original position
        Vector3 finalVector = rotatedVector + transform.position;
        Vector3 finalVectorOpposite = rotatedVectorOpposite + transform.position;
        Gizmos.DrawLine(finalVectorOpposite, finalVector);
        Gizmos.DrawLine(target.position, transform.position);
    }
}
