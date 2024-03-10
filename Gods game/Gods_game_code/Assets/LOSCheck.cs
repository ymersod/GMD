using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOSCheck : MonoBehaviour
{
    public string los_tag { get; set; }
    public string target_tag { get; set; }

    public bool IsInLOS()
    {
        return LOSManager.Instance.IsInLOS(los_tag, target_tag);
    }
}
