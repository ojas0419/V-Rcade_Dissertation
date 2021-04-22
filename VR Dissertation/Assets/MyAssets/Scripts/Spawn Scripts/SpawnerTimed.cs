using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SpawnerTimed : MonoBehaviour
{
    public float enemyLifetime;
    public static int numberOfActiveEnemies;

    public int maxEnemyTypes;
    public GameObject[] enemies;        // Array of enemy gameObjects defined in the Unity inspector    

    public int maxSpawnLocations;
    public Transform[] points;          // Array of locations to instantiate objects    

    public float beat = (60/130)*2;     // Time when enemies are instantiated = (one minute / beats per minute) x 2
    private float timer;                // Timer between beats and spawning of enemies
    public AudioClip audioClip;         // Get Audio Clip    
    private AudioSource audioSource;    // Get Audio Source    
    private float duration;             // Duration of audio    

    private void Awake()
    {
        Init();
        StartCoroutine(WaitForSound());
    }

    public void Init()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
        duration = audioClip.length;
    }

    IEnumerator WaitForSound()
    {
        while (audioSource.isPlaying == true)
        {
            if (timer > beat)
            {
                // Get random enemy within the range and instantiate it at random position
                GameObject enemy = Instantiate(enemies[Random.Range(0, maxEnemyTypes)], points[Random.Range(0, maxSpawnLocations)]);

                numberOfActiveEnemies++;

                // To make enemy not move, check its localPosition and set it to 0
                enemy.transform.localPosition = Vector3.zero;

                // Add rotation to enemy so that it is a random rotation - up, down, left or right - but this might not be needed
                //enemy.transform.Rotate(transform.forward, 90 * Random.Range(0, 4));

                // Destroy enemies after 21 seconds. Changing spawn location or map size will mean changing this timer
                Destroy(enemy, enemyLifetime);

                numberOfActiveEnemies--;

                // Update timer
                timer -= beat;
            }

            // Add the Time.deltaTime every frame to the timer
            timer += Time.deltaTime;

            yield return null;
        }

        // THIS CODE DOES NOT WORK FULLY
        //if (audioSource.isPlaying == false && numberOfActiveEnemies == 0)
        //{
        //    SceneManager.LoadScene("Main_Menu_Scene", LoadSceneMode.Additive);
        //}
    }
}
