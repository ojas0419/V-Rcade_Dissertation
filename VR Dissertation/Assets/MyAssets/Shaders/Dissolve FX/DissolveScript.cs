using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class DissolveScript : MonoBehaviour
{
    [SerializeField] private float noiseStrength = 0.25f;
    [SerializeField] private float objectHeight = 1.0f;

    private Material material;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        var time = Time.time * Mathf.PI * 0.25f;

        float height = transform.position.y;
        height += Mathf.Sin(time) * (objectHeight / 2.0f);  // modify height cutoff threshold based on position of the object + a sine wave
        SetHeight(height);
    }

    private void SetHeight(float height)
    {
        // Reference field of shader graph property can be used to set a name which can be used in scripts to pass data to this shader at runtime
        material.SetFloat("_CutoffHeight", height);
        material.SetFloat("_NoiseStrength", noiseStrength);
    }
}
