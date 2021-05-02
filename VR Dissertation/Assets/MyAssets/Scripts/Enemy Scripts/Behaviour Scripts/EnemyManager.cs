using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Determine what the enemy will target")]
    public Transform targetPlayer;

    public int maxZAxisDistanceBeforeDeath = 0;         // Determines how far along the Z-axis an enemy can travel before it is destroyed
}
