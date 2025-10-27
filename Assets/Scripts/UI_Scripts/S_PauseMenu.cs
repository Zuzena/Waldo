using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class S_PauseMenu : MonoBehaviour
{
    //get a reference to the pause menu
    public GameObject pauseMenu;
    public static S_PauseMenu instance;
    private bool pauseActive = false; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            ExistingEventSystem(); 
        }
    }

    //pause menu implementation
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                return;
            }
            else
            {
                pauseMenu.SetActive(!pauseMenu.activeSelf);
            }
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void ExistingEventSystem()
    {
        if (EventSystem.current != null)
        {
            return; 
        }
        var go = new GameObject("EventSystem", typeof(EventSystem));
#if ENABLE_INPUT_SYSTEM
        go.AddComponent<InputSystemUIInputModule>();
#endif
        DontDestroyOnLoad (go);
    }
}
