using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    public GameObject DifficultyToggles;

    // Start is called before the first frame update
    void Start()
    {
        DifficultyToggles.transform.GetChild((int)DifficultyValues.Difficulty).GetComponent<Toggle>().isOn = true;
    }

    #region Difficulty
    public void SetEasyDifficulty(bool isOn)
    {
        if (isOn)
        {
            DifficultyValues.Difficulty = DifficultyValues.Difficulties.easy;
        }
    }

    public void SetNormalDifficulty(bool isOn)
    {
        if (isOn)
        {
            DifficultyValues.Difficulty = DifficultyValues.Difficulties.normal;
        }
    }

    public void SetHardDifficulty(bool isOn)
    {
        if (isOn)
        {
            DifficultyValues.Difficulty = DifficultyValues.Difficulties.hard;
        }
    }

    #endregion
}
