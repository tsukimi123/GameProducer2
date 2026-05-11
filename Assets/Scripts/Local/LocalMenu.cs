using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalMenu : MonoBehaviour
{
    public string nextScene;
    public void loadNextSceene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
