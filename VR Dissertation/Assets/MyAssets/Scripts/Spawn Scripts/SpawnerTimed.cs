using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SpawnerTimed : MonoBehaviour
{
    public float enemyLifetime;
    public float enemyScale = 1.0f;     // Set the scale for enemies
    public static int numberOfActiveEnemies = 0;

    private int maxEnemyTypes;          // Number of maximum types of enemies
    public GameObject[] enemies;        // Array of enemy gameObjects defined in the Unity inspector    

    private int maxSpawnLocations;      // Number of maximum spawn locations
    public SpawnPoints[] spawner;       // Array of locations to instantiate objects    

    public float beat = (60/130)*2;     // Time when enemies are instantiated = (one minute / beats per minute) x 2
    private float timer;                // Timer between beats and spawning of enemies
    public AudioClip audioClip;         // Get Audio Clip    
    public AudioSource audioSource;    // Get Audio Source    

    private ScoreScript scoreScript;

    public void Start()
    {
        maxEnemyTypes = enemies.Length;
        maxSpawnLocations = spawner.Length;
        //audioSource.pitch = 1.0f;

        scoreScript = GameObject.Find("Score_Canvas").GetComponentInChildren<ScoreScript>();
    }

    private void Awake()
    {
        Init();
        StartCoroutine(WaitForSound());
    }

    private void FixedUpdate()
    {
        if (audioSource.isPlaying == false && numberOfActiveEnemies == 0)
        {
            //SceneManager.LoadScene("Main_Menu_Scene", LoadSceneMode.Additive);
            SaveScore();
            SceneManager.LoadSceneAsync("Main_Menu_Scene");
            Debug.Log("changed scenes");
        }

        Debug.Log("Number of enemies = " + numberOfActiveEnemies);
    }

    public void SaveScore()
    {
        // If the score is greater than the saved high score value (0 as default score)
        if(scoreScript.scoreValue > PlayerPrefs.GetInt(SceneManager.GetActiveScene().name, 0))
        {
            // Save score with scene name as the key and the score as the value
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, scoreScript.scoreValue);
        }
    }

    public void Init()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    IEnumerator WaitForSound()
    {
        while (audioSource.isPlaying == true)
        {
            if (timer > beat)
            {
                int randomSpawn = Random.Range(0, maxSpawnLocations);

                // Get random enemy within the range and instantiate it at random position
                GameObject enemy = Instantiate(enemies[Random.Range(0, maxEnemyTypes)], spawner[randomSpawn].transform);

                // Set scale of enemies to whatever value we want
                enemy.transform.localScale = new Vector3(enemyScale, enemyScale, enemyScale);

                // Associate the spawner with a specific flight path
                enemy.GetComponent<EnemyMasterScript>().SetFlightPath(spawner[randomSpawn].flightPaths);

                // To make enemy not move, check its localPosition and set it to 0
                enemy.transform.localPosition = Vector3.zero;

                // Add rotation to enemy so that it is a random rotation - up, down, left or right - but this might not be needed
                //enemy.transform.Rotate(transform.forward, 90 * Random.Range(0, 4));

                // Destroy enemies after lifetime timer ends. Changing spawn location or map size will mean changing this timer
                Destroy(enemy, enemyLifetime);

                // Update timer
                timer -= beat;
            }

            // Add the Time.deltaTime every frame to the timer
            timer += Time.deltaTime;

            yield return null;
        }
    }
}
