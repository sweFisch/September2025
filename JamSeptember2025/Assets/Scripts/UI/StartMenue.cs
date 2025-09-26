using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartMenue : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame


    private void Update()
    {
        if (Keyboard.current[Key.Q].wasPressedThisFrame)
        {
            SceneManager.LoadScene(1);
        }
        if (Keyboard.current[Key.W].wasPressedThisFrame)
        {
            SceneManager.LoadScene(2);
        }
        if (Keyboard.current[Key.E].wasPressedThisFrame)
        {
            SceneManager.LoadScene(3);
        }
        if (Keyboard.current[Key.R].wasPressedThisFrame)
        {
            SceneManager.LoadScene(4);
        }
    }

    public void StartButton()
    {
        int sceneToLoad = Random.Range(1, 5);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void NextLevel(){


    }

    public void ExitGame(){

    Application.Quit();

    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif

    }

}
