using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCoordinates : MonoBehaviour
{
    private int deathAtYPoint;      // Determines how far along the Y-axis an enemy can travel before it is destroyed
    private int deathAtZPoint;      // Determines how far along the Z-axis an enemy can travel before it is destroyed

    void Start()
    {
        // Access Enemy Manager to get the coordinate at which enemies will be destroyed
        deathAtYPoint = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>().maxYAxisDistanceBeforeDeath;
        deathAtZPoint = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>().maxZAxisDistanceBeforeDeath;
    }

    private void FixedUpdate()
    {
        // Destroy the enemy when it goes beyond the Z-axis or Y-axis coordinate
        if (transform.position.z <= deathAtZPoint)
        {
            Destroy(gameObject);
        }
        if (transform.position.y <= deathAtYPoint)
        {
            Destroy(gameObject);
        }
    }
}
