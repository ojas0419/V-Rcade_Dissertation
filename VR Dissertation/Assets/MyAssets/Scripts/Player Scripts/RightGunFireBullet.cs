using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightGunFireBullet : MonoBehaviour
{
    // Drag the Bullet Emitter into the Component Inspector
    public GameObject Bullet_Emitter;

    // Drag the Bullet Prefab into the Component Inspector
    public GameObject Bullet;

    // Set the speed of the Bullet in the Component Inspector
    public float Bullet_Forward_Force;

    // Declare Audio Clip
    public AudioClip clip;

    // Declare Audio Source
    public AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        // Get reference to Audio Source
        audioSource = GetComponent<AudioSource>();

        // Load Audio Source with desired Audio Clip
        audioSource.clip = clip;
    }

    // Update is called once per frame
    void Update()
    {
        // Trigger code when RIGHT hand trigger on Oculus Controller gets pressed
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            // Instantiate Bullet as GameObject
            GameObject Temporary_Bullet_Handler;
            Temporary_Bullet_Handler = Instantiate(Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;

            // This line will correct the rotation of the instantiated Bullet, but may not be needed
            //Temporary_Bullet_Handler.transform.Rotate(Vector3.left * 90);

            // Get a reference to the Bullet's RigidBody Component and control it
            Rigidbody Temporary_RigidBody;
            Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody>();

            // Tell the Bullet to be "pushed" forward by the value set by Bullet_Forward_Force
            Temporary_RigidBody.AddForce(transform.forward * Bullet_Forward_Force);

            // Play audio clip
            audioSource.Play();

            // Clean up Bullets by destroying them after 5 seconds
            Destroy(Temporary_Bullet_Handler, 5.0f);
        }
    }
}
