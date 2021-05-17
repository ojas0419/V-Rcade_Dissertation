using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public bool useAttachedAudio = false;
    public AudioSource source;
    public AudioClip ShieldSound;

    //private void OnTriggerEnter(Collider other)
    //{
    //    /* when object tagged "EnemyBullet" hits...*/
    //    if (other.CompareTag("EnemyBullet"))
    //    {
    //        // Play a sound clip once
    //        source.PlayOneShot(ShieldSound);

    //        // Destroy the bullet
    //        Destroy(other.gameObject);
    //    }
    //}

    public void OnCollisionEnter(Collision collision)
    {
        /* when object tagged "EnemyBullet" hits...*/
        if (collision.gameObject.CompareTag(("EnemyBullet")))
        {
            // if we are using the attached audio components
            if(useAttachedAudio == true)
            {
                // Play a sound clip once
                source.PlayOneShot(ShieldSound);
            }

            // Destroy the bullet
            Destroy(collision.gameObject);
            Debug.Log("Shield has blocked a bullet");
        }
    }
}
