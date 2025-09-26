using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartMenue : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame


    private void Update()
    {
        if (Keyboard.current[Key.Digit1].wasPressedThisFrame)
        {
            SceneManager.LoadScene(1);
        }
        if (Keyboard.current[Key.Digit2].wasPressedThisFrame)
        {
            SceneManager.LoadScene(2);
        }
    }

    public void StartButton()
    {
        int sceneToLoad = Random.Range(1, 2);
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
