using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOSCheck : MonoBehaviour
{
    public string los_tag { get; set; }
    public string target_tag { get; set; }

    public (bool isInLOS, Vector3 playerPos) IsInLOS()
    {
        return LOSManager.Instance.IsInLOS(los_tag, target_tag);
    }

    public bool ShouldPursuit()
    {
        return LOSManager.Instance.ShouldPursuit(los_tag, target_tag);
    }

    public List<Vector3Int> Pursuit(Vector3 targetPos)
    {
        return LOSManager.Instance.Pursuit(los_tag, targetPos);
    }
}
