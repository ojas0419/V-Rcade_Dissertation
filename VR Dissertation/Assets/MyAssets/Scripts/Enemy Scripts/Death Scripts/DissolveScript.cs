using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class DissolveScript : DeathAnimation
{
    [SerializeField] private float noiseStrength = 0.25f;
    [SerializeField] private float objectHeight = 1.0f;

    private Material material;

    public float startCutoff = 1.0f;
    public float deathCutoff = -1.0f;
    private float cutOffTimer;

    public EnemyMasterScript masterScript;

    private void Start()
    {
        // Grab the EnemyMasterScript
        masterScript = GetComponent<EnemyMasterScript>();

        //foreach(Renderer renderer in GetComponentsInChildren<Renderer>())
        //{
        //    material = GetComponent<Renderer>().material;
        //}

        //material = GetComponent<Renderer>().material;
        material = GetComponentInChildren<Renderer>().material;
        cutOffTimer = startCutoff;
        SetHeight(startCutoff);
    }

    public override void RunDeathAnimation()
    {
        // find all colliders in children and destroy them
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            Destroy(collider);
        }

        // have cutOffTimer decrease over time gradually
        cutOffTimer -= Time.deltaTime;

        SetHeight(cutOffTimer);

        // when cutOffTimer becomes less than the deathCutOff value, destroy enemy
        if (cutOffTimer < deathCutoff)
        {
            Destroy(gameObject);
        }
    }

    private void SetHeight(float height)
    {
        // Reference field of shader graph property can be used to set a name which can be used in scripts to pass data to this shader at runtime
        material.SetFloat("_CutoffHeight", height);
        material.SetFloat("_NoiseStrength", noiseStrength);
    }
}
