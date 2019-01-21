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
    ScavengerHuntAR scavengerHuntManager;

    public bool unlocked = false;
    public int unlockNumber;

    private void Awake()
    {
        scavengerHuntManager = FindObjectOfType<ScavengerHuntAR>();
    }

    private void Update()
    {
        if (scavengerHuntManager.arIndex == unlockNumber && GetComponent<MeshRenderer>().enabled)
            Unlock();
    }

    public void Unlock()
    {
        if (unlocked) return;
        scavengerHuntManager.UnlockNextTarget();
        unlocked = true;
    }
}
