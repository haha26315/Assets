using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelBuilder : MonoBehaviour
{
    public GameObject gameManagerPrefab;
    public GameObject levelSpawnerPrefab;

    private string[] Placable_Objects; 
    private string[] Placable_Traps;

    // Start is called before the first frame update
    void Start()
    {
        Placable_Objects = AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/Prefabs/Objects"});
        Placable_Traps = AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/Prefabs/Objects"});
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
