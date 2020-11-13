using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelAim : MonoBehaviour
{
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        if (target != null)
        {
            transform.LookAt(target);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
