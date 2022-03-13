using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("The game has been quit");
    }    

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronouly(sceneIndex));
        
    }

    IEnumerator LoadAsynchronouly(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log(progress);
            yield return null;
        }
    }
}
