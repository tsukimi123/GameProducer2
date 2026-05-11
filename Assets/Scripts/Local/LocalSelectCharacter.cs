using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LocalSelectCharacter : MonoBehaviour
{
    public string nextScene;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void loadNextSceene()
    {
        SceneManager.LoadScene(nextScene);
    }
}