using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Cam_Helpers : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    // Helper method to create a Texture2D with a single color
    public Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    // Convert absolute mouse coordinates into world mouse coordinates.
    // Bloats code if we leave this in because either we're creating and deleting new vector3s constantly
    // Or we're allowing unsafe pointer code to run
    //public void MouseProject(Vector3 mousePos){
    //    newMousePos = mousePos;
    //    mousePos.z = transform.position.z - Camera.main.transform.position.z;
    //    mousePos = Camera.main.ScreenToWorldPoint(mousePos);
    //}
}
