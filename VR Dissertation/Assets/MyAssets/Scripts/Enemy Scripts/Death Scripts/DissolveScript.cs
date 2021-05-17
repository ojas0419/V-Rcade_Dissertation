using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveScript : DeathAnimation
{
    [SerializeField] private float noiseStrength = 0.25f;
    [SerializeField] private float objectHeight = 1.0f;

    private Material material;
    public Renderer renderer2Dissolve;

    public float startCutoff = 1.0f;
    public float deathCutoff = -1.0f;
    private float cutOffTimer;

    public EnemyMasterScript masterScript;
    private bool isDissolving = false;

    private void Start()
    {
        // Grab the EnemyMasterScript
        masterScript = GetComponent<EnemyMasterScript>();

        //material = GetComponent<Renderer>().material;
        //material = GetComponentInChildren<Renderer>().material;
        material = renderer2Dissolve.material;
        cutOffTimer = startCutoff;
        SetHeight(startCutoff);
    }

    public void FixedUpdate()
    {
        if (isDissolving == true)
        {
            // have cutOffTimer decrease over time gradually
            cutOffTimer -= Time.deltaTime;

            SetHeight(cutOffTimer);

            // when cutOffTimer becomes less than the deathCutOff value, destroy enemy
            if (cutOffTimer < deathCutoff)
            {
                Destroy(gameObject);
            }
        }
    }

    public override void RunDeathAnimation()
    {
        isDissolving = true;

        // find all colliders in children and destroy them
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            Destroy(collider);
        }
    }

    private void SetHeight(float height)
    {
        // Reference field of shader graph property can be used to set a name which can be used in scripts to pass data to this shader at runtime
        material.SetFloat("_CutoffHeight", height);
        material.SetFloat("_NoiseStrength", noiseStrength);
    }
}
