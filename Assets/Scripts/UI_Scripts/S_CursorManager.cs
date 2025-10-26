using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

public class S_CursorManager : MonoBehaviour
{
    //assign cursor texture and pos
    public Texture2D CursorTexture;
    public Vector2 CursorPosition = new Vector2 (55, 25);
    public static S_CursorManager instance { get; private set; }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        //set cursor
        Cursor.SetCursor(CursorTexture, CursorPosition, CursorMode.Auto);
    }

    public void Size(int width, int height)
    {
        if (CursorTexture != null) 
        {
            CursorTexture.Reinitialize(width, height);
        }
    }

    public void Opacity(float opacity)
    {
        Color[] pixels = CursorTexture.GetPixels();
        for (int i = 0; i < pixels.Length; i++) 
        {
            Color pixelColor = pixels[i];
            pixelColor.a = opacity;
            pixels[i] = pixelColor;
        }
        CursorTexture.SetPixels(pixels);
        CursorTexture.Apply();
    }

    public void SetColor(Color color) 
    {
        Color[] pixels = CursorTexture.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            Color pixelColor = pixels[i];
            pixelColor = color;
            pixels[i] = pixelColor;
        }
        CursorTexture.SetPixels(pixels);
        CursorTexture.Apply();
    }
}
