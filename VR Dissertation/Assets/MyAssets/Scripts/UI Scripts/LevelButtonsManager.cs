using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButtonsManager : MonoBehaviour
{
    public void ChangeScene(string scene_name)
    {
        SceneManager.LoadSceneAsync(scene_name);
    }
}
