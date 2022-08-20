using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject _MainMenu;
    public GameObject LoadingInterface;
    public Image loadingProgressBar;
    public TextMeshProUGUI _text;

    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    public void StartGame()
    {
        ShowLoadingScreen();
        scenesToLoad.Add(SceneManager.LoadSceneAsync("GamePlay"));
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Transitions", LoadSceneMode.Additive));
        //scenesToLoad.Add(SceneManager.LoadSceneAsync("SECTOR J", LoadSceneMode.Additive));
        //scenesToLoad.Add(SceneManager.LoadSceneAsync("SECTOR L", LoadSceneMode.Additive));
        StartCoroutine(LoadingScreen());
    }

    public void ShowLoadingScreen()
    {
        LoadingInterface.SetActive(true);
    }

    IEnumerator LoadingScreen()
    {
        float totalProgress=0;
        for(int i=0;i<scenesToLoad.Count;++i)
        {
            while(!scenesToLoad[i].isDone)
            {
                totalProgress +=scenesToLoad[i].progress;
                loadingProgressBar.fillAmount = totalProgress/scenesToLoad.Count;
                yield return null;
            }
        }
    }

    /*public void PlayGame()
    {
        ///SceneManager.LoadScene(1);
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        LoadingInterface.SetActive(true);
        _MainMenu.SetActive(false);
        AsyncOperation AsyncOperation= SceneManager.LoadSceneAsync(1);
        AsyncOperation.allowSceneActivation=true;
        AsyncOperation.allowSceneActivation=false;
        while(!AsyncOperation.isDone)
        {
            loadingProgressBar.fillAmount = AsyncOperation.progress;
            _text.text="Loading " + AsyncOperation.progress + "%";
            if(AsyncOperation.progress >=0.9f)
            {
                _text.text = "Press the space bar to continue.";
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    AsyncOperation.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }*/
    public void ExitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
