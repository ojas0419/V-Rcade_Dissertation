using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Determine what the enemy will target")]
    public Transform targetPlayer;

    public int maxYAxisDistanceBeforeDeath = -10;       // Determines how far along the Y-axis an enemy can travel before it is destroyed

    public int maxZAxisDistanceBeforeDeath = 0;         // Determines how far along the Z-axis an enemy can travel before it is destroyed

    private float SpawnerTimedBeat;
    //public void Start()
    //{
    //    SpawnerTimedBeat = GameObject.Find("SpawnerManager").GetComponent<SpawnerTimed>().beat;

    //    var enemyHealth = GameObject.Find("Enemy Manager").GetComponent<EnemyMasterScript>().health;
    //    var enemyPoints = GameObject.Find("Enemy Manager").GetComponent<EnemyMasterScript>().pointsWorth;
    //    var enemyMoveSpeed = GameObject.Find("Enemy Manager").GetComponent<EnemyMasterScript>().NormalSpeed;

    //    //EnemyMasterScript[] enemyMasterScripts = FindObjectsOfType(typeof(EnemyMasterScript)) as EnemyMasterScript[];
    //    //foreach(EnemyMasterScript enemyMasterScript in GetComponentsInChildren<EnemyMasterScript>())
    //    //{
    //    //    enemyMasterScript.GetComponent<EnemyMasterScript>().health;
    //    //}

    //    switch (DifficultyValues.Difficulty)
    //    {
    //        case DifficultyValues.Difficulties.easy:
    //            SpawnerTimedBeat /= 2;
    //            enemyHealth /= 2;
    //            enemyPoints /= 2;
    //            enemyMoveSpeed /= 2;
    //            break;

    //        case DifficultyValues.Difficulties.normal:
    //            SpawnerTimedBeat *= 1;
    //            enemyHealth *= 1;
    //            enemyPoints *= 1;
    //            enemyMoveSpeed *= 1;
    //            break;

    //        case DifficultyValues.Difficulties.hard:
    //            SpawnerTimedBeat *= 2;
    //            enemyHealth *= 2;
    //            enemyPoints *= 2;
    //            enemyMoveSpeed *= 2;
    //            break;
    //    }
    //}
}
