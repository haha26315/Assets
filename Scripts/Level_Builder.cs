using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Level_Builder : GUI_Cam_Helpers
{
    public GameObject gameManagerPrefab;
    public GameObject levelSpawnerPrefab;

    public float distFromCenter;

    public int buttonWidth;
    public int buttonHeight;

    //private GameObject[] gameObjs;
    private string[] SubFolders;
    private List<string> Object_Categories;
    private List<List<string>> Placeables;
    private List<List<string>> Placeable_Names;

    // Sentinel value of -1.
    private int currentCategory;
    private int currentPlaceable;

    private GameObject currentSelectedObj;

    // Level builder menu toggle
    private bool toggle = false;

    // Variables for radial menus.
    private int radialNum;

    // Start is called before the first frame update
    void Start()
    {
        // Initializing all of our lists.
        SubFolders = AssetDatabase.GetSubFolders("Assets/Resources/Prefabs");

        Object_Categories = new List<string>();
        Placeable_Names = new List<List<string>>();
        Placeables = new List<List<string>>();

        // Adds to our list of Placeable object each found asset within the folders we parsed.
        for(int i = 0; i < SubFolders.Length; i++){
            string folder = SubFolders[i];
            
            // Gets the last bit of the filename. Ex; Assets/Prefabs/Objects = Objects. 
            // These become the names of each category of Placeable objects.
            string[] split = folder.Split('/');
            Object_Categories.Add(split[split.Length-1]);

            // Get the GUIDs of assets at our current folder
            string[] Assets = AssetDatabase.FindAssets("t:prefab", new string[] {folder});

            // Make sure we have the next dimension of lists properly set up beforehand
            Placeables.Add(new List<string>());
            Placeable_Names.Add(new List<string>());

            // Add the backbutton for our radial submenu in each category for navigation.
            Placeable_Names[i].Add("Back");

            foreach(string Asset in Assets){

                // Gets and adds the file path of the asset to our list of placable assets.
                string AssetPath = AssetDatabase.GUIDToAssetPath(Asset);
                AssetPath = AssetPath.Split("Assets/Resources/")[1];
                Placeables[i].Add(AssetPath);

                // Make sure we don't include the rest of the file path or the file type what we display to player.
                // Reuse split variable because it's convenient and the previous info isn't needed past this point.
                split = AssetPath.Split('.');
                split = split[0].Split('/');

                // Add to our list of names of items that can be placed
                Placeable_Names[i].Add(split[split.Length-1]);
            }
        }

        currentCategory = -1;
        currentPlaceable = -1;
    }


    private void Update(){
        
        // Player is activating our levelbuilder.
        if (Input.GetKeyDown(KeyCode.E))
        {
            toggle = !toggle;
            //currentCategory = -1;
            //Time.timeScale = isPaused ? 0 : 1;

            // Disable the controls of the player while paused, re-enable them when unpaused
            // Made this capable of gathering collections in case there is a level
            // with multiple controllable player objects.
            /*
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players){

                // If we're paused, controls are disabled.
                player.GetComponent<Player_Movement>().controlsDisabled = isPaused;
            }
            */
        }

        bool leftClick = Input.GetMouseButtonDown(0);

        // We have a selected object! Check for key inputs and apply our selected permutations to them.
        if(currentSelectedObj is not null){
            
            // Simple movement of the object
            bool up = Input.GetKeyDown(KeyCode.UpArrow);
            bool down = Input.GetKeyDown(KeyCode.DownArrow);
            bool left = Input.GetKeyDown(KeyCode.LeftArrow);
            bool right = Input.GetKeyDown(KeyCode.RightArrow);

            // Deselection of our current object
            bool enter = Input.GetKeyDown(KeyCode.KeypadEnter);

            // Self evident deletion of our object
            bool delete = Input.GetKeyDown(KeyCode.Delete);

            // The object needs special permutations applied to work, as with our polar coordinate blocks
            if(currentSelectedObj.name == "Level Block (Clone)"){
                
            }else{  // Our object can be moved normally in the editor.
                
            }

            // Global permutations like deletion
            if (delete){
                GameObject.Destroy(currentSelectedObj);
            }
        }

        if(currentCategory != -1 && currentPlaceable != -1){

            bool rightClick = Input.GetMouseButtonDown(1);
            
            /*
            var Asset = AssetDatabase.LoadObjectAsync(Placeables[currentPlaceable]);
            while(!Asset.isDone){
                yield return null;
            }
            */

            if(rightClick){

                //Debug.Log("Current Category; " + currentCategory + "   Current Placeable;  " + currentPlaceable);
                
                string[] split = Placeables[currentCategory][currentPlaceable].Split('.');

                string filepath = split[0];
                //split = split[0].Split('/');

                GameObject Asset = Resources.Load<GameObject>(filepath) as GameObject;

                //Debug.Log("Asset; " + Asset + "   Path  " + filepath);

                Vector3 mousePos = Input.mousePosition;
                mousePos.z = transform.position.z - Camera.main.transform.position.z;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);

                currentSelectedObj = Instantiate(Asset, mousePos, Quaternion.identity);
            }
            
        }
        

    }

    void OnGUI(){
        
        if(toggle){

            // Calculate the center of the screen
            float centerX = Screen.width / 2;
            float centerY = Screen.height / 2;

            // Get the number of objects for our radial menu.
            int radialMenuNum;
            if(currentCategory == -1){
                radialMenuNum = Object_Categories.Count;
            }else{
                //Debug.Log("Current Category:  " + currentCategory);
                radialMenuNum = Placeable_Names[currentCategory].Count;
            }

            // Create a GUIStyle with a semi-transparent dark background
            GUIStyle backgroundStyle = new GUIStyle(GUI.skin.box);
            backgroundStyle.normal.background = MakeTex(2, 2, new Color(0.2f, 0.2f, 0.2f, 0.8f));

            // Create a GUIStyle for the text with white lettering
            GUIStyle textStyle = new GUIStyle(GUI.skin.label);
            textStyle.normal.textColor = Color.white;
            textStyle.alignment = TextAnchor.MiddleCenter;

            // Create a GUIStyle for the button with white lettering and a custom background
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.normal.background = MakeTex(2, 2, new Color(0.2f, 0.2f, 0.2f, 0.8f));
            buttonStyle.hover.background = MakeTex(2, 2, new Color(0.3f, 0.3f, 0.3f, 0.8f));


            for(int i = 0; i < radialMenuNum; i++){

                int halfWidth = buttonWidth / 2;
                int halfHeight = buttonHeight / 2;
                
                float radians = (((float)i / ((float)radialMenuNum)) * 2f * Mathf.PI - (Mathf.PI / 2));
                float xCoord = distFromCenter * Mathf.Cos(radians) - halfWidth;
                float yCoord = distFromCenter * Mathf.Sin(radians) - halfHeight;

                //Debug.Log("radians  " +  radians + "  xCoord " + xCoord + "  yCoord " + yCoord);

                if(currentCategory == -1){
                    // We've selected a category.
                    if(GUI.Button(new Rect(centerX + xCoord, centerY + yCoord, buttonWidth, buttonHeight), Object_Categories[i], buttonStyle)){
                        currentCategory = i;
                        radialMenuNum = Placeable_Names[currentCategory].Count;
                    }
                }else{

                    // We've selected an object.
                    if(GUI.Button(new Rect(centerX + xCoord, centerY + yCoord, buttonWidth, buttonHeight), Placeable_Names[currentCategory][i], buttonStyle)){

                        // We've hit the back button
                        if(i == 0){
                            currentCategory = -1;
                            currentPlaceable = -1;
                            radialMenuNum = Object_Categories.Count;
                        }else{ // We've hit another button
                            currentPlaceable = i - 1;
                            toggle = false;
                        }
                    }
                }
                
            }
        }
    }
}
