using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private float max_health = 100f;
    private float current_health;
    
    [SerializeField]
    private UnityEngine.UI.Image hp_image;
    void Start()
    {
        current_health = max_health;
    }

    
    void Update()
    {
        hp_image.fillAmount = UpdateHealthBar(current_health, max_health);
        
        //TakeDmg(1);
    }

    private float UpdateHealthBar(float current_health, float max_health)
    {
        return current_health / max_health;
    }

    private float TakeDmg(float dmg)
    {
        return current_health -= dmg;
    }
}
