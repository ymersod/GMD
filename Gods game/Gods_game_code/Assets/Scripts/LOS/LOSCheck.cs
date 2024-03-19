using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOSCheck : MonoBehaviour
{
    public string los_tag { get; set; }
    public string target_tag { get; set; }

    public (bool IsInLOS, Vector3 targetPos) IsInLOS()
    {
        return LOSManager.Instance.IsInLOS(los_tag, target_tag);
    }

    public bool ShouldPursuit()
    {
        return LOSManager.Instance.ShouldPursuit(los_tag, target_tag);
    }

    public Vector3[] Pursuit()
    {
        return LOSManager.Instance.Pursuit(los_tag, target_tag, transform.position);
    }
}
