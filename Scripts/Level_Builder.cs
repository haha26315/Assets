using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class LevelBuilder : MonoBehaviour
{
    public GameObject gameManagerPrefab;
    public GameObject levelSpawnerPrefab;

    private string[] Placable_Objects; 
    private string[] Placable_Traps;

    private GameObject[] gameObjs;

    // Variables for radial menus.
    private int radialNum;

    // Start is called before the first frame update
    void Start()
    {
        gameObjs = (Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]);

        //Placable_Objects = AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/Prefabs/Objects"});
        //Placable_Traps = AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/Prefabs/Objects"});
        Debug.Log("Start fired");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update fired");
    }
}
