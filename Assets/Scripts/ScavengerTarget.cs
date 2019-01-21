/******************************************************
*Project: AR Scavenger Hunt
*Created by: Colton Spruill
*Date: 20190118
*Description: Used to mark off currect scavenger hunt object that is scanned.
*Copyright 2019 LeapWithAlice,LLC. All rights reserved
 ******************************************************/
using UnityEngine;

public class ScavengerTarget : MonoBehaviour
{
    //Scavenger Hunt ref
    ScavengerHuntAR scavengerHuntManager;

    //Is this target unlocked and what number am i
    public bool unlocked = false;
    public int unlockNumber;

    private void Awake()
    {
        //Find scav hunt ref
        scavengerHuntManager = FindObjectOfType<ScavengerHuntAR>();
    }

    private void Update()
    {
        //If the Scavhunt manager's ar index is equal to this unlock number
        if (scavengerHuntManager.arIndex == unlockNumber)
        {
            //And if the mesh renderer is enabled, unlock this target.
            if (GetComponent<MeshRenderer>().enabled)
                Unlock();
        }

        //If this target is not unlocked, turn off mesh renderer.
        if (!unlocked)
            GetComponent<MeshRenderer>().enabled = false;

    }

    //Unlock the next target
    public void Unlock()
    {
        //If not unlocked, go away, Else unlock and turn true.
        if (unlocked) return;
        scavengerHuntManager.UnlockNextTarget();
        unlocked = true;
    }
}