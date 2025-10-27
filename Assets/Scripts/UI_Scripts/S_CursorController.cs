using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.Port;

public class S_CursorController : MonoBehaviour
{
    //Cursor variables 
    /*public static S_CursorManager CursorM; 
    public Slider CursorOpacitySlider;
    public Slider CursorSizeSlider;
    public TMP_Dropdown CursorColor;

    private void Start()
    {
        //get and set the cursor information for the first time
        CursorM = S_CursorManager.instance;
        float savedCursorOpacity = PlayerPrefs.GetFloat("Opacity", 1.0f);
        float savedCursorSize = PlayerPrefs.GetFloat("Size", 1);
        string savedColor = PlayerPrefs.GetString("Color", "White");
        CursorOpacitySlider.value = savedCursorOpacity;
        CursorSizeSlider.value = savedCursorSize;

        //listen for changes
        CursorOpacitySlider.onValueChanged.AddListener(SetOpacity);
        CursorSizeSlider.onValueChanged.AddListener(SetSize);
        CursorColor.onValueChanged.AddListener(_=>
        {
            string name = CursorColor.options[CursorColor.value].text;
            SetColor(name); 
        }); 

        //set values for size slider
        CursorSizeSlider.minValue = 1;
        CursorSizeSlider.maxValue = 3; 

        //actually set the info 
        //SetOpacity(savedCursorOpacity);
       // SetSize(savedCursorSize);
       // SetColor(savedColor);
    }

    
    //set the opacity calling to the manager
    public void SetOpacity(float opacity)
    {
        if (CursorM == null) return;
        else if (CursorM != null)
        {
            CursorM.Opacity(opacity);
            PlayerPrefs.SetFloat("Opacity", opacity);
            PlayerPrefs.Save();
        }
    }


    //set the size calling to the manager
    public void SetSize(float size)
    {
        if (CursorM == null) return;
        else if(CursorM != null)
        {
            CursorM.Size((int)size);
            PlayerPrefs.SetFloat("Size", size);
            PlayerPrefs.Save();
        }
    }


    //set the color calling to the manager
    public void SetColor(string colorName)
    {
        if (CursorM == null) return;
        else if (CursorM != null)
        {
            Color color = Color.white;
            switch (colorName.ToLowerInvariant())
            {
                case "White": color = Color.white; break;
                case "Red": color = Color.red; break;
                case "Blue": color = Color.blue; break;
                case "Green": color = Color.green; break;
                case "Yellow": color= Color.yellow; break;
                case "Purple": color = Color.purple; break;
                case "Pink": color = Color.pink; break; 
            }
            CursorM.SetColor(color); 
            PlayerPrefs.SetString("Color", colorName);
            PlayerPrefs.Save(); 
        }
    }*/
}
