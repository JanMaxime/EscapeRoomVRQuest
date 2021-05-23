using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Inspired but largely adapted and debugged from https://developpaper.com/unity-realizes-the-effect-of-writing-on-the-blackboard-in-vr/
public class Notepad : MonoBehaviour
{
    [Header("Lerp")]
    [Range(0.001f, 1f)]
    public float lerp = 0.05f;

    //Position of the pen on the notepad
    private Vector2 penPos;

    //Position of the pen on the notepad during the last frame
    private int lastPenX;
    private int lastPenY;

    //Texture of the notepad
    private Texture2D textureInitialized;


    //Resolution of the notepad
    private int textureWidth = 1980;
    private int textureHeight = 1080;

    //Size of the pen's brush
    private int painterTipsWidth = 15;
    private int painterTipsHeight = 10;

    //Color of the pen
    private Color32[] painterColor;

    //State of the pen. Is true when the player is drawing something
    private bool isDrawing = false;

    //State of the notepad. It starts closed
    private bool isOpen = false;

    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    /// <summary>
    /// Checks the input to know if the notepad needs to be opened or closed
    /// </summary>
    /// <returns>true when the notepad needs to be opened or closed, false otherwise</returns>
    private bool openClose()
    {
        return OVRInput.GetDown(OVRInput.RawButton.Y);
    }


    /// <summary>
    /// Initializes all parameters at the start of the game
    /// </summary>
    void Start()
    {
        //Create and apply a white texture to the notepas
        textureInitialized = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false, true);
        textureInitialized.SetPixels32(Enumerable.Repeat<Color32>(new Color32(255, 255, 255, 255), textureWidth * textureHeight).ToArray<Color32>());
        textureInitialized.Apply();
        GetComponent<MeshRenderer>().material.mainTexture = textureInitialized;

        //Creates the texture of the paint brush
        painterColor = Enumerable.Repeat<Color32>(new Color32(0, 0, 0, 255), painterTipsWidth * painterTipsHeight).ToArray<Color32>();

        meshRenderer = this.GetComponent<MeshRenderer>();
        meshCollider = this.GetComponent<MeshCollider>();
        meshRenderer.enabled = false;
        meshCollider.convex = true;
        meshCollider.isTrigger = true;
    }

    /// <summary>
    /// Updates the notepad by opening/closing it when needed and by drawing on it.
    /// </summary>
    private void LateUpdate()
    {
        //Handles the opening or closing of the notepad
        if (openClose())
        {
            isOpen = !isOpen;
            meshRenderer.enabled = isOpen;

            //When opening the Notepad, we need to first disable the trigger and then disable the convex collider
            if (isOpen)
            {
                meshCollider.isTrigger = false;
                meshCollider.convex = false;
            }
            //Otherwise when closing, we first enable the convex collider and then only activate the trigger
            else
            {
                meshCollider.convex = true;
                meshCollider.isTrigger = true;
            }

        }

        //If the notepad is closed, nothing remains to be done
        if (!isOpen) return;

        //Calculate the starting point of the color block represented by the current brush
        int texPosX = (int)(penPos.x * (float)textureWidth - (float)(painterTipsWidth / 2));
        int texPosY = (int)(penPos.y * (float)textureHeight - (float)(painterTipsHeight / 2));

        //Handles the drawing on the notepad
        if (isDrawing)
        {
            StartCoroutine(VibrationManager.vibrate(true, 0.05f, 0.05f, 0.5f));
            textureInitialized.SetPixels32(texPosX, texPosY, painterTipsWidth, painterTipsHeight, painterColor);
            //If you move the brush quickly, only sparse points will be drawn. Therefore the following lines interpolate the two positions to draw
            //a complete line between then
            if (lastPenX != 0 && lastPenY != 0)
            {
                int lerpCount = (int)(1 / lerp);
                for (int i = 0; i <= lerpCount; i++)
                {
                    float percentage = (float)i / (float)lerpCount;
                    int x = (int)Mathf.Lerp((float)lastPenX, (float)texPosX, percentage);
                    int y = (int)Mathf.Lerp((float)lastPenY, (float)texPosY, percentage);
                    textureInitialized.SetPixels32(x, y, painterTipsWidth, painterTipsHeight, painterColor);
                }
            }

            //Applies the new texture to the notepas
            textureInitialized.Apply();

            //Stores the previous positions of the pen
            lastPenX = texPosX;
            lastPenY = texPosY;
        }
        //By definition, the last positions of the pen are zero when the user did not draw during the last frame
        else
        {
            lastPenX = lastPenY = 0;
        }

    }


    /// <summary>
    /// Sets the position of the pen on the notepas
    /// </summary>
    /// <param name="x">The x position of the pen</param>
    /// <param name="y">The y position of the pen</param>
    public void SetPainterPositon(float x, float y)
    {
        penPos.Set(x, y);
    }

    /// <summary>
    /// Set or get the drawing status
    /// </summary>
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
