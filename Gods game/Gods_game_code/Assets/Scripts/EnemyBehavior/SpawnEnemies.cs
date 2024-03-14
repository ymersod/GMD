using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] private GameObject slime_prefab;
    [SerializeField] private GameObject pig_prefab;
    void Awake()
    {
        Instantiate(slime_prefab);
    }
}
