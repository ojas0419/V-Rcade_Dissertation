using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            var enemy = this.transform.parent.gameObject;
            enemy.SendMessage("GotHit");
        }
    }

    public void Explode()
    {
        Destroy(gameObject);
    }
}
