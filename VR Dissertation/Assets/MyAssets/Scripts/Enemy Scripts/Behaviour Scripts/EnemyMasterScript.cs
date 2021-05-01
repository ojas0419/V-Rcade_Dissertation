﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMasterScript : MonoBehaviour
{
    #region Enemy Data
    [Header("Enemy Stats")]
    private Transform player;        // The target to look and aim at
    public int health = 5;          // The enemy's health
    private int currentHP;

    public int pointsWorth;         // Define how many points you get for killing this enemy in inspector

    private Color originalColour;   // For grabbing material colour and changing it when hit

    [Header("Enemy Flight Path Setup")]
    private int index = 0;
    private Transform[] LocationCoords;
    private int[] shootLocations;
    public bool isMovingForward = false;

    [Header("Enemy Travel Speed")]
    public float NormalSpeed = 10f;
    private float normalSpeed = 10f;

    [Header("Enemy Attack Setup")]
    public GameObject Barrel;
    public GameObject laser;

    [SerializeField]
    private float shootPower = 1000f;
    [SerializeField]
    private float fireRate = 2f;
    [SerializeField]
    private float nextFire = 0.0f;

    public bool useAttachedAudio = false;
    public AudioSource source;
    public AudioClip laserSound;

    public DeathAnimation deathAnimation;
    #endregion Enemy Data

    public void FixedUpdate()   // ONLY FOR TESTING ENEMY DEATHS. Comment out when testing is not needed
    {
        if (health <= 0)                                    // if health is 0 or lower
        {
            ScoreScript.scoreValue += pointsWorth;          // Access ScoreScript, increase score by the points the enemy is worth
            deathAnimation.RunDeathAnimation();             // Execute explosion
        }

    }

    private void Awake()
    {
        // Add to list of enemies
        SpawnerTimed.numberOfActiveEnemies++;
    }

    void Start()
    {
        // Access Enemy Manager object and EnemyManager script to get the following variables:
        player = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>().targetPlayer;

        // Get material colour of object
        originalColour = GetComponent<Renderer>().material.color;

    }

    void Update()
    {
        // Face main camera, aka player
        transform.forward = Vector3.ProjectOnPlane((Camera.main.transform.position - transform.position), Vector3.up).normalized;

        Move();
        Shoot();
    }

    public void SetFlightPath(FlightPaths flightPaths)
    {
        // Assign the flight paths and fire locations from FlightPaths script
        LocationCoords = flightPaths.flightPath;
        shootLocations = flightPaths.firingSpots;
    }

    public void Move()
    {
        // Sets enemy to move forwards
        if (isMovingForward == true)
        {
            MoveForward();
        }
        // otherwise, follow the flight path
        else
        {
            FollowPath();
        }
    }

    public void FacePlayer()
    {
        // Face player / main camera
        transform.LookAt(Camera.main.transform.position);

        // Rotate...for something?
        transform.Rotate(new Vector3(-8, 5, 0));
    }

    public void OnDestroy()
    {
        // Remove enemy from list
        SpawnerTimed.numberOfActiveEnemies -= 1;
    }

    private void FollowPath()
    {
        FacePlayer();

        // Adjust speeds depending on distance between locations
        if (Vector3.Distance(LocationCoords[index].position, LocationCoords[index + 1].position) < 0.5f)
        {
            normalSpeed = NormalSpeed / 4;
        }
        else
        {
            normalSpeed = NormalSpeed;
        }

        // When enemy reaches last location in index...
        if (index > LocationCoords.Length - 3)
        {
            // ... restart from the beginning, or
            //index = 0;

            // set moving forward to true to enable MoveForward
            isMovingForward = true;
        }

        // Move enemy to the next location within the index
        float step = normalSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, LocationCoords[index].position, step);

        if (Vector3.Distance(transform.position, LocationCoords[index].position) < 0.001f)
        {
            index++;
        }
    }

    public void MoveForward()
    {
        FacePlayer();

        // Move forward at a constant speed along -z world axis
        transform.position += Time.deltaTime * Vector3.back * normalSpeed;
    }

    private void Shoot()
    {
        if (shootLocations.Contains(index) && Time.time > nextFire)
        {
            // Instantiate laser from Barrel position and rotate laser to be straight
            var firedLaser = Instantiate(laser, Barrel.transform.position, Barrel.transform.rotation * Quaternion.Euler(90f, 0f, 0f));
            //var firedLaser = Instantiate(laser, Barrel.transform.position, Quaternion.identity);

            // Target the player
            firedLaser.transform.LookAt(player);

            // Calculate trajectory direction
            Vector3 trajectory = player.position - firedLaser.transform.position;

            // Get rigidbody and add force to laser
            firedLaser.GetComponent<Rigidbody>().AddForce(Barrel.transform.forward * shootPower);

            // If we are using an attached AudioSource and Clip
            if(useAttachedAudio == true)
            {
                // Play laser sound once
                source.PlayOneShot(laserSound);
            }

            // Destroy laser after 2 seconds
            Destroy(firedLaser, 2f);

            nextFire = Time.time + fireRate;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")                // If object with tag Bullet hits...
        {
            if (health >= 1)                                    // ...if health is 1 or higher, execute this
            {
                Destroy(collision.gameObject);                  // Destroy the object tagged "Bullet"
                health -= 1;                                    // Reduce health by 1
                StartCoroutine("ColourFlash");                  // Run coroutine
            }
            if (health <= 0)                                    // ...if health is 0 or lower, execute this instead
            {
                Destroy(collision.gameObject);                  // Destroy the object tagged "Bullet"
                ScoreScript.scoreValue += pointsWorth;          // Access ScoreScript, increase score by the points the enemy is worth
                deathAnimation.RunDeathAnimation();             // Access DeathAnimation script to enable chosen death behaviour
            }
        }
    }

    public IEnumerator ColourFlash()
    {
        // Get material colour and make it red...
        GetComponent<Renderer>().material.color = Color.red;

        // ...for 0.1 seconds...
        yield return new WaitForSeconds(0.1f);

        // ...then reset material colour back to the original colour...
        GetComponent<Renderer>().material.color = originalColour;

        // ... and stop this coroutine
        StopCoroutine("ColourFlash");
    }
}