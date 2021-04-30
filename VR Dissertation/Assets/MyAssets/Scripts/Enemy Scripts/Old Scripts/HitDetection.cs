using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
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

    void Start()
    {
        // Calculate the pivot distance
        cubesPivotDistance = cubeSize * cubesInRow / 2;

        // Use this value to create the pivot vector
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Bullet")
        {
            //// Destroy this object, aka enemy
            //Destroy(gameObject);
            Explode();
        }
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
            if(rb != null)
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
