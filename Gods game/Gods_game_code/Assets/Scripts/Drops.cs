using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drops : MonoBehaviour
{
    [SerializeField] private int[] drop_weight;
    [SerializeField] private GameObject[] drop_items;
    public List<(int weight, GameObject item)> drops = new();

    void Awake()
    {
        for (int i = 0; i < drop_weight.Length; i++)
        {
            drops.Add((drop_weight[i], drop_items[i]));
        }
    }
}
