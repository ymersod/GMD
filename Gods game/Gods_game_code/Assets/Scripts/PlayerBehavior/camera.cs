using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class camera : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<CinemachineVirtualCamera>().Follow = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
