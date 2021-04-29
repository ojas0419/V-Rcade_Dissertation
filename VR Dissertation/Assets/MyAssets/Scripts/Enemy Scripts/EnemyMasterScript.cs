using System.Collections;
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

    public AudioSource source;
    public AudioClip laserSound;

    #endregion Enemy Data

    #region Creating Pieces
    [Header("Explode Into Cubes")]
    //// If you want to create smaller copy of the object, use this: !!
    //public GameObject piecePrefab;

    public float cubeSize = 0.2f;
    public float cubesInRow = 5;

    float cubesPivotDistance;
    Vector3 cubesPivot;

    public float explosionForce = 50f;
    public float explosionRadius = 4f;
    public float explosionUpward = 0.4f;

    //public Material material;   // this is only needed if changing the colour of pieces created
    #endregion Creating Pieces

    private void Awake()
    {
        // Add to list of enemies
        SpawnerTimed.numberOfActiveEnemies++;
    }

    void Start()
    {
        // Access Enemy Manager object and EnemyManager script to get the following variables:
        player = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>().targetPlayer;
        LocationCoords = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>().flightPath;
        shootLocations = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>().firingSpots;

        // Get material colour of object
        originalColour = GetComponent<Renderer>().material.color;

        // Calculate the pivot distance
        cubesPivotDistance = cubeSize * cubesInRow / 2;

        // Use this value to create the pivot vector
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
    }

    void Update()
    {
        // Face main camera, aka player
        transform.forward = Vector3.ProjectOnPlane((Camera.main.transform.position - transform.position), Vector3.up).normalized;

        Move();
    }

    public void OnDestroy()
    {
        // Remove enemy from list
        SpawnerTimed.numberOfActiveEnemies -= 1;
    }

    private void Move()
    {
        // Face player / main camera
        transform.LookAt(Camera.main.transform.position);

        // Rotate...for something?
        transform.Rotate(new Vector3(-8, 5, 0));

        // Adjust speeds depending on distance between locations
        if (Vector3.Distance(LocationCoords[index].position, LocationCoords[index + 1].position) < 0.5f)
        {
            normalSpeed = NormalSpeed / 4;
        }
        else
        {
            normalSpeed = NormalSpeed;
        }

        if (shootLocations.Contains(index) && Time.time > nextFire)
        {
            Shoot();
            nextFire = Time.time + fireRate;
        }

        // When enemy reaches last location in index...
        if (index > LocationCoords.Length - 3)
        {
            // ... restart from the beginning, or
            //index = 0;

            // ...Move forward at a constant speed
            transform.position += Time.deltaTime * transform.forward * 2;
        }

        // Move enemy to the next location within the index
        float step = normalSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, LocationCoords[index].position, step);

        if (Vector3.Distance(transform.position, LocationCoords[index].position) < 0.001f)
        {
            index++;
        }
    }

    private void Shoot()
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

        // Play laser sound once
        source.PlayOneShot(laserSound);

        // Destroy laser after 2 seconds
        Destroy(firedLaser, 2f);
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
                Explode();                                      // Execute explosion
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

    public void Explode()
    {
        Destroy(gameObject);

        // Loop 3 times to create 5x5x5 pieces along x, y and z coordinates
        for (int x = 0; x < cubesInRow; x++)
        {
            for (int y = 0; y < cubesInRow; y++)
            {
                for (int z = 0; z < cubesInRow; z++)
                {
                    createPiece(x, y, z);
                }
            }
        }

        // Get explosion position
        Vector3 explosionPos = transform.position;

        // Get colliders in that position and radius
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);

        // Add explosion force to all colliders in that overlap sphere
        foreach (Collider hit in colliders)
        {
            // Get rigidbody from that collider object
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Add explosion force to this body with given parameters
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
            }
        }
    }

    void createPiece(int x, int y, int z)
    {
        // Create a piece in shape of cube
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //// Create a copy of any chosen object prefab !!
        //piece = Instantiate(piecePrefab, new Vector3(0, 0, 0), Quaternion.identity);

        // Set piece position and scale
        piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        // Add rigidbody and set mass
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;

        // Change colour of the cubes made to match original object
        piece.transform.GetComponent<Renderer>().material = gameObject.GetComponent<Renderer>().material;

        //// or change colour of the cubes manually
        //material = piece.GetComponent<Renderer>().material;
        //material.color = Color.red;

        // Grab each piece made and randomly destroy it
        Destroy(piece, UnityEngine.Random.Range(0.5f, 5f));
    }
}
