using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionZone : MonoBehaviour
{
    //public AudioClip slowMotionSound;
    //public AudioSource source;

    public float slowMotionSpeed = 0.1f;
    public float slowDownAudio = 0.1f;
    private AudioSource LevelMusic;

    public void Start()
    {
        //// Get the audio clip for the level's music
        //LevelMusic = GameObject.Find("SpawnerManager").GetComponent<SpawnerTimed>().audioSource;
    }

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

            //// Get all audio sources in children and slow down their pitch
            //foreach (AudioSource audioSource in GetComponentsInChildren<AudioSource>())
            //{
            //    audioSource.pitch *= slowDownAudio;
            //}

            //LevelMusic.pitch *= slowDownAudio;

            // Use this to return pitch back to original value (assign it to something first)
            //1 / slowDownAudio;

            //// Play the slow motion sound once
            //source.PlayOneShot(slowMotionSound);
        }
    }
}
