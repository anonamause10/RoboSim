using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Text loadText;
    public Slider slider;

    void Start(){
        LoadLevel(1);
    }

    public void LoadLevel(int sceneNum){
        StartCoroutine(LoadAsynchronously(sceneNum));
    }

    IEnumerator LoadAsynchronously(int sceneNum){

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNum);
        while(!operation.isDone){
            slider.value = operation.progress/0.9f;
            loadText.text = "Loading... " + 100*operation.progress/0.9f +"%";
            yield return null;
        }
    }
}
