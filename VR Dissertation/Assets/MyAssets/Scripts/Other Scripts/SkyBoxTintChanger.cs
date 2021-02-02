using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxTintChanger : MonoBehaviour
{
    public Material skyMaterial;
    Color targetColour;
    float timeLeft;

    float lerpValue = 0;
    bool countUp = true;

    void FixedUpdate()
    {
        //if (timeLeft <= Time.deltaTime)
        //{
        //    skyMaterial.SetColor("_Tint", targetColour);
        //    targetColour = new Color(Random.value, Random.value, Random.value);
        //    timeLeft = 2.0f;
        //}
        //else
        //{
        //    skyMaterial.SetColor("_Tint", Color.Lerp(skyMaterial.color, targetColour, Time.deltaTime / timeLeft));
        //    timeLeft -= Time.deltaTime;
        //}

        targetColour = new Color(Random.value, Random.value, Random.value);

        if(lerpValue >= 1)
        {
            lerpValue = 1f;
            countUp = false;
        }

        if(lerpValue <= 0)
        {
            lerpValue = 0f;
            countUp = true;
        }

        if (countUp)
            lerpValue += Time.deltaTime;
        else
            lerpValue -= Time.deltaTime;

        skyMaterial.SetColor("_Tint", Color.Lerp(skyMaterial.color, targetColour, lerpValue));
    }
}
