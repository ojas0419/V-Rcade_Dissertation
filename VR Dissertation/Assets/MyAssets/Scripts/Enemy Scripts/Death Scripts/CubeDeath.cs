using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDeath : DeathAnimation
{
    #region Creating Pieces
    [Header("Explodes Object Into Cubes or Copies of Itself")]
    public bool createMiniCopies = false;
    // If you want to create smaller copy of the object, use this: !!
    public GameObject piecePrefab;

    public float cubeSize = 0.2f;
    public float cubesInRow = 5;

    float cubesPivotDistance;
    Vector3 cubesPivot;

    public float explosionForce = 50f;
    public float explosionRadius = 4f;
    public float explosionUpward = 0.4f;

    public bool useOriginalPrefabMaterial = true;
    private Material material;   // this is only needed if changing the colour of pieces created
    public Color newMaterialColour;

    private EnemyMasterScript masterScript;
    #endregion Creating Pieces

    public override void RunDeathAnimation()
    {
        masterScript.sourceDeath.PlayOneShot(masterScript.deathSound);
        Explode();
    }

    public void Start()
    {
        masterScript = GetComponent<EnemyMasterScript>();

        // Calculate the pivot distance
        cubesPivotDistance = cubeSize * cubesInRow / 2;

        // Use this value to create the pivot vector
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
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
                    if(createMiniCopies == false)
                    {
                        CreateCube(x, y, z);
                    }
                    else if (createMiniCopies == true)
                    {
                        CreateMiniCopies(x, y, z);
                    }
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

    void CreateCube(int x, int y, int z)
    {
        // Create a piece
        GameObject piece;

        // Assign piece to create a copy of a cube
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Set piece position and scale
        piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        // Add rigidbody and set mass
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;

        if(useOriginalPrefabMaterial == true)
        {
            // Change colour of the cubes made to match original object
            piece.transform.GetComponent<Renderer>().material = gameObject.GetComponent<Renderer>().material;
        }
        else
        {
            // or change colour of the cubes manually
            material = piece.GetComponent<Renderer>().material;
            //material.color = Color.red;
            material.color = newMaterialColour;
        }

        // Grab each piece made and randomly destroy it
        Destroy(piece, UnityEngine.Random.Range(0.5f, 5f));
    }

    void CreateMiniCopies(int x, int y, int z)
    {
        // Create a piece
        GameObject piece;

        // Assign piece to create a copy of any chosen object prefab
        piece = Instantiate(piecePrefab, new Vector3(0, 0, 0), Quaternion.identity);

        // Set piece position and scale
        piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        // Add rigidbody and set mass
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;

        if (useOriginalPrefabMaterial == true)
        {
            // Change colour of the cubes made to match original object
            piece.transform.GetComponent<Renderer>().material = gameObject.GetComponent<Renderer>().material;
        }
        else
        {
            // or change colour of the cubes manually
            material = piece.GetComponent<Renderer>().material;
            //material.color = Color.red;
            material.color = newMaterialColour;
        }

        // Grab each piece made and randomly destroy it
        Destroy(piece, UnityEngine.Random.Range(0.5f, 5f));
    }

}
