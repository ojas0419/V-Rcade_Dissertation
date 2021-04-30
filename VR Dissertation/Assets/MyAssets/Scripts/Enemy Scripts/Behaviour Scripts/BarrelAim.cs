using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelAim : MonoBehaviour
{
    private Transform target;

    public void Start()
    {
        target = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>().targetPlayer;
    }
    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.LookAt(target);
        }
    }
}
