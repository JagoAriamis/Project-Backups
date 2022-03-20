using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;

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

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
    }
}
