/******************************************************
*Project: Persistent AR
*Created by: David Lee Ramirez
*Date: 2/10/2019
*Description: select and delete objects
*Copyright 2019 LeapWithAlice,LLC. All rights reserved
 ******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAndDeleteObject : MonoBehaviour
{

    //holds the gameobject of the 3d model to delete
    private GameObject selectedModel;


    //checks for if raycast click hits a 3d model, if it does, set selectedModel as that 3d model
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.name == "HP(Clone)" || hit.transform.name == "Boar(Clone)" || hit.transform.name == "GirlHead(Clone)")
            {
                selectedModel = hit.transform.gameObject;
                Debug.Log("selected this model: "+hit.transform.name);
            }
        }
    }

    //Destroy selected 3d model
    public void DeleteSelectedModel()
    {
        Destroy(selectedModel);
        selectedModel = null;
    }
}
