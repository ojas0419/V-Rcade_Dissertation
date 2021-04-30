using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightPaths : MonoBehaviour
{

    [Header("Assign flight path locations and locations at which enemies will fire")]
    public Transform[] flightPath;
    public int[] firingSpots;
}
