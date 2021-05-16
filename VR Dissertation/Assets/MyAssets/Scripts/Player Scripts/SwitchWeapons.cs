using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeapons : MonoBehaviour
{
    public GameObject head;
    public bool rightController;
    public bool leftController;
    private float switchRate = 1f;
    private float nextSwitch = 1.0f;

    // Update is called once per frame
    void Update()
    {
        CheckIfSwitch();
    }

    private void CheckIfSwitch()
    {
        Vector3 controllerPosition;

        /* Associate left and right controllers with correct locations */
        if(rightController == true)
        {
            controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        } else
        {
            controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        }

        /* when the controller's angle is large enough along the Y OR X axis, switch guns */
        if (Vector3.Angle(transform.up, Vector3.up) > 80 && controllerPosition.y > head.transform.position.y - 0.4f && controllerPosition.x < head.transform.position.x && Time.time > nextSwitch)
        {
            nextSwitch = Time.time + switchRate;
            var mainGun = this.gameObject.transform.GetChild(0);
            var shield = this.gameObject.transform.GetChild(1);

            if (mainGun.gameObject.activeSelf == true)
            {
                mainGun.gameObject.SetActive(false);
                shield.gameObject.SetActive(true);
            }
            else if (mainGun.gameObject.activeSelf == false)
            {
                shield.gameObject.SetActive(false);
                mainGun.gameObject.SetActive(true);
            }
        }
    }
}
