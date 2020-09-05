using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Get game object
    public GameObject[] enemies;

    // Declare locations to instantiate objects
    public Transform[] points;

    // Time when enemies are instantiated
    public float beat = (60/130)*2;

    // Timer between beats and spawning of enemies
    private float timer;
    
    // Update is called once per frame
    void Update()
    {
        if (timer > beat)
        {
            // Get random enemy within the range and instantiate it at random position
            GameObject enemy = Instantiate(enemies[Random.Range(0, 2)], points[Random.Range(0, 4)]);

            // To make enemy not move, check its localPosition and set it to 0
            enemy.transform.localPosition = Vector3.zero;

            // Add rotation to enemy so that it is a random rotation - up, down, left or right - but this might not be needed
            enemy.transform.Rotate(transform.forward, 90 * Random.Range(0, 4));

            // Destroy enemies after 21 seconds. Changing spawn location or map size will mean changing this timer
            Destroy(enemy, 21.0f);

            // Update timer
            timer -= beat;
        }

        // Add the Time.deltaTime every frame to the timer
        timer += Time.deltaTime;
    }
}
