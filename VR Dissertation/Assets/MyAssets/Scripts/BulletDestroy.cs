using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Enemy")
        {
            // Add score
            ScoreScript.scoreValue += 10;

            // Destroys object tagged "Enemy"
            Destroy(other.gameObject);

            // Hides bullet gameObject as we destroy it in other scripts
            gameObject.SetActive(false);
        }
    }
}
