using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public GameObject livesCanvas;
    TextMeshProUGUI livesText;
    public int playerHealth = 3;

    public SpawnerTimed spawnerManager;

    public void Start()
    {
        // Find the Lives_Canvas object and assign the livesText to it's TMP UI component
        livesCanvas = GameObject.Find("Lives_Canvas");
        livesText = livesCanvas.GetComponentInChildren<TextMeshProUGUI>();

        // Find SpawnerManager and acquire it's SpawnerTimed script
        spawnerManager = GameObject.Find("SpawnerManager").GetComponent<SpawnerTimed>();
    }

    public void Update()
    {
        // Update our lives text with the current playerHealth
        livesText.text = "Lives: " + playerHealth;

        // If we have 0 or less lives left, begin game over sequence
        if (playerHealth <= 0)
        {
            GameOver();
            SceneManager.LoadSceneAsync("Main_Menu_Scene");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        // Reduce player health whenever hit by an object with the tag
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            playerHealth--;
            //Debug.Log("Player has been hit");
        }
    }

    public void GameOver()
    {
        // Access SaveScore function to save our current score in the scene
        spawnerManager.SaveScore();
    }
}
