/******************************************************
*Project: AR Scavenger Hunt
*Created by: Colton Spruill
*Date: 20190118
*Description: Controls the flow of the tips and ar objects unlocking in order.
*Copyright 2019 LeapWithAlice,LLC. All rights reserved
 ******************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScavengerHuntAR : MonoBehaviour
{
    public GameObject map;
    public GameObject[] arMarkers;
    public string[] arHints;
    public string shComplete = "Thank you for playing!";
    public Text hintText;
    public int arIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        hintText.text = arHints[arIndex];
    }


    public void UnlockNextTarget()
    {
        arIndex ++;

        if(arIndex >= arHints.Length)
        {
       hintText.text = shComplete;
            return;
        }

       hintText.text = arHints[arIndex];
    }
}