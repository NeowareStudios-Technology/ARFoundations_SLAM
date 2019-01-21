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

    void Start()
    {
        //Set initial ar hint to the current index, 0
        hintText.text = arHints[arIndex];
    }

    //Unlock the next target in the array
    public void UnlockNextTarget()
    {
        //Increment index
        arIndex++;

        //if index is greater or equal to max hints
        if (arIndex >= arHints.Length)
        {
            //Set hint text to complete text, go away
            hintText.text = shComplete;
            return;
        }

        //Else just update the hint text
        hintText.text = arHints[arIndex];
    }
}