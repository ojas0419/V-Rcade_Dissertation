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

    private void Awake()
    {
        targetColour = new Color(Random.value, Random.value, Random.value);
    }

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


        if(lerpValue >= 1)
        {
            lerpValue = 1f;
            countUp = false;
        }

        if(lerpValue <= 0)
        {
            //targetColour = new Color(Random.value, Random.value, Random.value);
            targetColour = new Color(Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f));
            lerpValue = 0f;
            countUp = true;
        }

        if (countUp)
            lerpValue += Time.deltaTime;
        else
            lerpValue -= Time.deltaTime;

        skyMaterial.SetColor("_Tint", Color.Lerp(Color.black, targetColour, lerpValue));
    }
}
