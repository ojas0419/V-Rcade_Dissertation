using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionZone : MonoBehaviour
{
    public AudioClip slowMotionSound;
    public AudioSource source;

    public float slowMotionSpeed = 0.1f;

    private void OnTriggerEnter(Collider other)
    {
        /* when object tagged "EnemyBullet" enters... */
        if (other.CompareTag("EnemyBullet"))
        {
            // Get laser game object
            var laser = other.gameObject;

            // Get the ridigbody's velocity
            var laserSpeed = laser.GetComponent<Rigidbody>().velocity;

            // Multiply velocity by the slow motion speed
            laser.GetComponent<Rigidbody>().velocity = laserSpeed * slowMotionSpeed;

            // Play the slow motion sound once
            source.PlayOneShot(slowMotionSound);
        }
    }
}
