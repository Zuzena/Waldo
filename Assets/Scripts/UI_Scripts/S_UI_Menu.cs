using UnityEngine;
using UnityEngine.SceneManagement; 

public class S_UI_Menu : MonoBehaviour 
{
    //Play button
    public void PlayGame()
    {
        SceneManager.LoadScene(1); 
    }

    //Quit Button
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    //Options -- bgm slider, sfx slider, cursor opacity, cursor color


}
