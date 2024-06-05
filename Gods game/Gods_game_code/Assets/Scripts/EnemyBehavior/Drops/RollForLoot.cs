using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollForLoot : MonoBehaviour
{
    public static RollForLoot Instance { get; private set;}
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public IEnumerator DropLoot(Transform enemyTransform, List<(int weight, GameObject item)> drops)
    {
        var random = Random.Range(0, 11); //0 - 10
        foreach (var (weight, item) in drops)
        {
            if  (random >= weight)
            {
                Instantiate(item, enemyTransform.position, Quaternion.identity);
            }
        }
        yield return null;
    }
}
