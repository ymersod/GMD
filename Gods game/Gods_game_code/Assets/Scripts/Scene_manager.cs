using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_manager : MonoBehaviour
{
    public static Scene_manager instance;
    [SerializeField] private int scene;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void OnEnable()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if(player == null) return;
        if(scene == 1)
        {
            player.GetComponent<LOS>().enabled = false;
            player.GetComponent<PlayerStatus>().enabled = false;
        }
    }
    public void LoadScene(int scene)
    {
        if(scene == 0)
        {
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
       
            foreach (GameObject go in allObjects)
            {
                Destroy(go);
            }
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

}
