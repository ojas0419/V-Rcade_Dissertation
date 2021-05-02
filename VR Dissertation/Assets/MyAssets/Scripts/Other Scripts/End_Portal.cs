using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End_Portal : MonoBehaviour
{    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            Debug.Log("An enemy has hit the portal and has been destroyed");
        }
    }
}
