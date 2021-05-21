using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Inspired but largely adapted and debugged from https://developpaper.com/unity-realizes-the-effect-of-writing-on-the-blackboard-in-vr/
public class Notepad : MonoBehaviour
{
    public float lerp = 0.05f;

    private int lastPaintX;
    private int lastPaintY;
    private Texture2D textureInitialized;

    private Vector2 paintPos;
    private int textureWidth = 1980;
    private int textureHeight = 1080;
    private int painterTipsWidth = 15;
    private int painterTipsHeight = 10;
    private Color32[] painterColor;
    private bool isDrawing = false; // is the current brush on the palette
    // Start is called before the first frame update

    private bool isOpen = false;

    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    private bool openClose()
    {
        return OVRInput.GetDown(OVRInput.RawButton.Y);
    }


    void Start()
    {
        textureInitialized = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false, true);
        textureInitialized.SetPixels32(Enumerable.Repeat<Color32>(new Color32(255, 255, 255, 255), textureWidth * textureHeight).ToArray<Color32>());
        textureInitialized.Apply();

        //Assign to blackboard
        GetComponent<MeshRenderer>().material.mainTexture = textureInitialized;

        painterColor = Enumerable.Repeat<Color32>(new Color32(0, 0, 0, 255), painterTipsWidth * painterTipsHeight).ToArray<Color32>();


        meshRenderer = this.GetComponent<MeshRenderer>();
        meshCollider = this.GetComponent<MeshCollider>();
        meshRenderer.enabled = false;
        meshCollider.convex = true;
        meshCollider.isTrigger = true;
    }

    private void LateUpdate()
    {

        if(openClose()){
            isOpen = !isOpen;
            meshRenderer.enabled = isOpen;

            //When opening the Notepad, we need to first disable the trigger and then disable the convex collider
            if(isOpen){
                meshCollider.isTrigger = false;
                meshCollider.convex = false;
            }
            //Otherwise when closing, we first enable the convex collider and then only activate the trigger
            else{
                meshCollider.convex = true;
                meshCollider.isTrigger = true;
            }
            
        }

        if(!isOpen) return;
        //Calculate the starting point of the color block represented by the current brush
        int texPosX = (int)(paintPos.x * (float)textureWidth - (float)(painterTipsWidth / 2));
        int texPosY = (int)(paintPos.y * (float)textureHeight - (float)(painterTipsHeight / 2));

        if (isDrawing)
        {
            StartCoroutine(VibrationManager.vibrate(true, 0.05f, 0.05f, 0.5f));
            textureInitialized.SetPixels32(texPosX, texPosY, painterTipsWidth, painterTipsHeight, painterColor);
            //If you move the brush quickly, there will be intermittent phenomenon, so interpolation is needed
            if (lastPaintX != 0 && lastPaintY != 0)
            {
                int lerpCount = (int)(1 / lerp);
                for (int i = 0; i <= lerpCount; i++)
                {
                    float percentage = (float)i / (float)lerpCount;
                    int x = (int)Mathf.Lerp((float)lastPaintX, (float)texPosX, percentage);
                    int y = (int)Mathf.Lerp((float)lastPaintY, (float)texPosY, percentage);
                    textureInitialized.SetPixels32(x, y, painterTipsWidth, painterTipsHeight, painterColor);
                }
            }
            textureInitialized.Apply();
            lastPaintX = texPosX;
            lastPaintY = texPosY;
        }
        else
        {
            lastPaintX = lastPaintY = 0;
        }

    }


    public void SetPainterPositon(float x, float y)
    {
        paintPos.Set(x, y);
    }

    public bool IsDrawing
    {
        get
        {
            return isDrawing;
        }
        set
        {
            isDrawing = value;
        }
    }
}
