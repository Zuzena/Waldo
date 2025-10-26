using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.Port;

public class S_CursorController : MonoBehaviour
{
    public static S_CursorManager CursorM; 
    public Slider CursorOpacitySlider;
    public Slider CursorSizeSlider;
    public Dropdown CursorColor;

    private void Start()
    {
        //get and set the cursor information for the first time
        CursorM = S_CursorManager.instance;
        float savedCursorOpacity = PlayerPrefs.GetFloat("Opacity", 1.0f);
        float savedCursorSize = PlayerPrefs.GetFloat("Size", 0.5f); 
        CursorOpacitySlider.value = savedCursorOpacity;
        CursorSizeSlider.value = savedCursorSize;

        //listen for changes
        CursorOpacitySlider.onValueChanged.AddListener(SetOpacity);
        CursorSizeSlider.onValueChanged.AddListener(SetSize);
        CursorColor.onValueChanged.AddListener(delegate
        {
            string selectedText = CursorColor.options[CursorColor.value].text;
            SetColor(selectedText); 
        }); 

        //set values for size slider
        CursorSizeSlider.minValue = 1;
        CursorSizeSlider.maxValue = 3; 
    }

    public void SetOpacity(float opacity)
    {
        if (CursorM != null)
        {
            CursorM.Opacity(opacity);
            PlayerPrefs.SetFloat("Opacity", opacity);
            PlayerPrefs.Save();
        }
    }

    public void SetSize(float size)
    {
        if (CursorM != null)
        {
            CursorM.Size((int)size, (int)size);
            PlayerPrefs.SetFloat("Size", size);
            PlayerPrefs.Save();
        }
    }

    public void SetColor(string colorName)
    {

    }
}
