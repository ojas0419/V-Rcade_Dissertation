using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Determine what the enemy will target")]
    public Transform targetPlayer;

    [Header("Assign flight path locations and locations at which enemies will fire")]
    public Transform[] flightPath;
    public int[] firingSpots;

}
