using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float LevelLoadDelay = 2f;
    [SerializeField] float LevelExitSloMoFactor = 0.4f;
    // Start is called before the first frame update
 

    public void LoadNextLevel()
    {
        //StartCoroutine(LoadNextLevelNumerator());
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex+1);
    }
    //IEnumerator LoadNextLevelNumerator()
    //{
    //    Time.timeScale = LevelExitSloMoFactor;
    //    yield return new WaitForSecondsRealtime(LevelLoadDelay);
    //    Time.timeScale = 1f;
    //    var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    //    SceneManager.LoadScene(currentSceneIndex+1);
    //}

    public void PlayTutorial()
    {
        
        SceneManager.LoadScene("Tutorial");
    }

    public void PlayFirstLevel()
    {
        SceneManager.LoadScene("1-1");
    }

    public void ResetGameSesssion()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
