using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

public class S_CursorManager : MonoBehaviour
{
    //assign cursor texture and pos
    public Texture2D CursorTexture;
    public Vector2 CursorPosition = new Vector2 (55, 25);
    
    //get a reference to the manager and set up a "new texture" that's values we adjust
    public static S_CursorManager instance { get; private set; }
    //private Texture2D newTexture;

    //we don't want this to disappear... we also want to make sure that we immediately set the cursor. 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        //lets set the texture to edit here
        //newTexture = new Texture2D(CursorTexture.width, CursorTexture.height, TextureFormat.ARGB32, false);
        //Graphics.CopyTexture(CursorTexture, newTexture);
       // RefreshCursor(); 

        //set cursor
        Cursor.SetCursor(CursorTexture, CursorPosition, CursorMode.Auto);
    }

    //apply the changes to the new texture and refresh this cursor as the new one!
    /*void RefreshCursor()
    {
        newTexture.Apply();
        Cursor.SetCursor(newTexture, CursorPosition, CursorMode.Auto);

    }

    //scale the size of the texture
    public void Size(int scale)
    {
        int newWidth = CursorTexture.width * Mathf.Max(1, scale);
        int newHeight = CursorTexture.height * Mathf.Max(1, scale);
        // use a render texture 
        var rt = RenderTexture.GetTemporary(newWidth, newHeight, 0, RenderTextureFormat.ARGB32);
        Graphics.Blit(CursorTexture, rt);
        var prev = RenderTexture.active; 
        RenderTexture.active = rt;  
        if(newTexture.width != newWidth || newTexture.height != newHeight)
        {
            newTexture.Reinitialize(newWidth, newHeight);
        }
        newTexture.ReadPixels(new Rect(0,0, newWidth, newHeight), 0, 0, false);
        RenderTexture.active = prev;
        RenderTexture.ReleaseTemporary(rt);
        RefreshCursor(); 
    }

    //adjust the texture opacity
    public void Opacity(float opacity)
    {
        var px = newTexture.GetPixels(); 
        for (int i = 0; i < px.Length; i++)
        {
            px[i] *= opacity;
        }
        newTexture.SetPixels(px);
        RefreshCursor ();
    }

    //adjust the texture color
    public void SetColor(Color color) 
    {
        var px = newTexture.GetPixels(); 
        for(int i = 0;i < px.Length; i++)
        {
            //still remember our opacity
            color.a = px[i].a;
            px[i] = color; 
        }
        newTexture.SetPixels(px);
        RefreshCursor ();
    }*/
}
