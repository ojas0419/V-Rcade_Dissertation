using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMasterScript : MonoBehaviour
{
    #region Enemy Data
    public int health = 5;
    private int currentHP;

    public int pointsWorth;         // Define how many points you get for killing this enemy in inspector

    private Color originalColour;   // For grabbing material colour and changing it when hit
    #endregion Enemy Data

    #region Creating Pieces
    //// If you want to create smaller copy of the object, use this: **
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

    // Start is called before the first frame update
    void Start()
    {
        // Get material colour of object
        originalColour = GetComponent<Renderer>().material.color;

        // Calculate the pivot distance
        cubesPivotDistance = cubeSize * cubesInRow / 2;

        // Use this value to create the pivot vector
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
    }

    // Update is called once per frame
    void Update()
    {
        //// Move forward at a constant speed
        //transform.position += Time.deltaTime * transform.forward * 2;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")                // If object with tag Bullet hits...
        {
            if (health >= 1)                                    // ...if health is 1 or higher, execute this
            {
                Debug.Log("Bullet has hit the enemy once");
                Destroy(collision.gameObject);                  // Destroy the object tagged "Bullet"
                health -= 1;                                    // Reduce health by 1
                StartCoroutine("ColourFlash");                  // Run coroutine
            }
            else if (health <= 0)                               // ...if health is 0 or lower, execute this instead
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

        //// Create a copy of any chosen object prefab **
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
