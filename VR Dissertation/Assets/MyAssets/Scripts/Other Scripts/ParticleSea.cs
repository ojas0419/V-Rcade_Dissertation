using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSea : MonoBehaviour
{
    public ParticleSystem particleSystem;               // creates public particle system variable
    private ParticleSystem.Particle[] particlesArray;   // creates array for particles

    public Gradient colourGradient;                     // Create a colour gradient for the particles

    [Range(0, 250)]public int seaResolution = 25;       // creates value to handle resolution of the sea
    public float spacing = 0.25f;                       // sets spacing between each particle
    public float noiseScale = 0.2f;                     // change noise scale
    public float heightScale = 3f;                      // change height scale

    // Floats to change value with time for perlin noise function
    private float perlinNoiseAnimX = 0.01f;
    private float perlinNoiseAnimY = 0.01f;
    public float xAnimationSpeed;
    public float yAnimationSpeed;
    public Vector2 perlinNoiseOffset;

    public void XAnimationSpeed(float speed)
    {
        xAnimationSpeed = speed;
    }

    public void YAnimationSpeed(float speed)
    {
        yAnimationSpeed = speed;
    }
    private void Start()
    {
        // Set up new array with suitable space for all the particles
        particlesArray = new ParticleSystem.Particle[seaResolution * seaResolution];

        // Set max particle count = seaResolution squared
        particleSystem.maxParticles = seaResolution * seaResolution;

        // Spawn desired amount of particles
        particleSystem.Emit(seaResolution * seaResolution);

        // Assign spawned particles to the array
        particleSystem.GetParticles(particlesArray);

        StartCoroutine("Animate");
    }

    private void Update()
    {
        // Loop through all particles and place them based on their positions (seaRes = 25 particles in a row & every n-th 25 particles in the next row)
        for (int i = 0; i < seaResolution; i++)
        {
            for (int j = 0; j < seaResolution; j++)
            {
                // Calculate zPos using Mathf.PerlinNoise and position of currently processed particle
                float zPos = Mathf.PerlinNoise(i * noiseScale + perlinNoiseAnimX, j * noiseScale + perlinNoiseAnimY);

                // Assign colour of particle by evaluating gradient by zPos variable
                particlesArray[i * seaResolution + j].color = colourGradient.Evaluate(zPos);

                particlesArray[i * seaResolution + j].position = new Vector3(i * spacing, zPos * heightScale, j * spacing);
            }
        }

        // Add animations to X and Y coords
        perlinNoiseAnimX += 0.01f;
        perlinNoiseAnimY += 0.01f;

        // Assign this array back to the particle system
        particleSystem.SetParticles(particlesArray, particlesArray.Length);
    }

    IEnumerator Animate()
    {
        while (true)
        {
            perlinNoiseOffset = new Vector2(perlinNoiseOffset.x + xAnimationSpeed, perlinNoiseOffset.y + yAnimationSpeed);
            yield return new WaitForSeconds(0.02f);
        }
    }
}
