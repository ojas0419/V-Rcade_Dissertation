using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int index = 0;
    public Transform[] LocationCoords;

    public float NormalSpeed = 10f;
    private float normalSpeed = 10f;

    private int hp = 2;

    //public Transform target;

    // Shoot at these locations
    public int[] shootLocations;

    public GameObject Barrel;
    public GameObject laser;

    private float shootPower = 1000f;
    private float fireRate = 2f;
    private float nextFire = 0.0f;

    public AudioSource source;
    public AudioClip laserSound;

    void Update()
    {
        //if(target != null)
        //{
        //    transform.LookAt(target);
        //}

        // Face main camera aka player
        transform.forward = Vector3.ProjectOnPlane((Camera.main.transform.position - transform.position), Vector3.up).normalized;
        Move();
        CheckIfDead();
    }

    private void CheckIfDead()
    {
        if (hp <= 0)
        {
            //Animate explosion
            var ship = this.transform.GetChild(0);
            ship.SendMessage("Explode");
        }
    }

    private void TakeDamage()
    {
        hp--;
    }
    private void Move()
    {
        // Face player / main camera
        transform.LookAt(Camera.main.transform.position);

        // Rotate...for something?
        transform.Rotate(new Vector3(-8, 5, 0));

        // Adjust speeds depending on distance between locations
        if (Vector3.Distance(LocationCoords[index].position, LocationCoords[index + 1].position) < 0.5f)
        {
            normalSpeed = NormalSpeed / 4;
        }
        else
        {
            normalSpeed = NormalSpeed;
        }

        if (shootLocations.Contains(index) && Time.time > nextFire)
        {
            Shoot();
            nextFire = Time.time + fireRate;
        }

        // When enemy reaches last location in index, restart from the beginning
        if (index > LocationCoords.Length - 3)
        {
            index = 0;
        }

        // Move enemy to the next location within the index
        float step = normalSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, LocationCoords[index].position, step);

        if (Vector3.Distance(transform.position, LocationCoords[index].position) < 0.001f)
        {
            index++;
        }
    }

    private void Shoot()
    {
        // Instantiate laser from Barrel position and rotate laser to be straight
        var firedLaser = Instantiate(laser, Barrel.transform.position, Barrel.transform.rotation * Quaternion.Euler(90f, 0f, 0f));

        // Get rigidbody and add force to laser
        firedLaser.GetComponent<Rigidbody>().AddForce(Barrel.transform.forward * shootPower);

        // Play laser sound once
        source.PlayOneShot(laserSound);

        // Destroy laser after 5 seconds
        Destroy(firedLaser, 5f);
    }
}
