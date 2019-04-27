using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Intro : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            LoadNextScene();
        }

    }

    void LoadNextScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.buildIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(currentScene.buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings);
        }
    }

}
