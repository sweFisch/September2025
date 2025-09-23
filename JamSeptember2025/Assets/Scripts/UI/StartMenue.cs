using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenue : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame




public void StartButton(){

    SceneManager.LoadScene(0);

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
