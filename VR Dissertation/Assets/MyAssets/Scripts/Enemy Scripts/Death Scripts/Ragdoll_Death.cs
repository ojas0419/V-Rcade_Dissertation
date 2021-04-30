using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll_Death : DeathAnimation
{
    public EnemyMasterScript masterScript;

    // Torque values for ragdoll behaviour
    public float torqueRandomMin = -1.0f;
    public float torqueRandomMax = 1.0f;
        public void Start()
    {
        // Grab the EnemyMasterScript
        masterScript = GetComponent<EnemyMasterScript>();
    }

    public override void RunDeathAnimation()
    {
        // destroy our master script
        Destroy(masterScript);

        //Destroy(GetComponent<Collider>());

        // find all colliders in children and destroy them
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            Destroy(collider);
        }

        // Add a new rigidbody so object is affected by gravity
        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();

        // Apply random torque values on every axis
        rigidbody.AddTorque(new Vector3(Random.Range(torqueRandomMin, torqueRandomMax), Random.Range(torqueRandomMin, torqueRandomMax), Random.Range(torqueRandomMin, torqueRandomMax)));
    }
}
