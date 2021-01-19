using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Enemy", menuName = "New Enemy/Enemy")]
public class EnemySO : ScriptableObject
{
    public GameObject enemyModel;
    public string enemyName;
    public int enemyHealth = 2;
    public float enemySpeed = 10f;
    public float shootPower = 1000f;
    public float fireRate = 2f;
    public float nextFire = 0.0f;
}
