using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableDestroyOnLoad : MonoBehaviour
{
    void Awake() => DontDestroyOnLoad(gameObject);
    
}
