using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public AudioSource source;
    public AudioClip ShieldSound;

    private void OnTriggerEnter(Collider other)
    {
        /* when object tagged "EnemyBullet" hits...*/
        if (other.CompareTag("EnemyBullet"))
        {
            // Play a sound clip once
            source.PlayOneShot(ShieldSound);

            // Destroy the bullet
            Destroy(other.gameObject);
        }
    }

}
