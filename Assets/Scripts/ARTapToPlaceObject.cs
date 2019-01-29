/******************************************************
*Project: Persistent AR
*Created by: Colton Spruill
*Date: 20190118
*Description: Used for placing ar objects within scenes.
*Copyright 2019 LeapWithAlice,LLC. All rights reserved
 ******************************************************/
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.UI;
using System.Collections.Generic;

public class ARTapToPlaceObject : MonoBehaviour {

    public PersistentAR persistentAR;
    //Array of gameobjects the user can place. 
    public GameObject[] objectsToPlace;
    //Max number to place
    public int maxObjs = 5;
    //Array indexer to display current obj and name
    public int objIndex;
    //Text reference to display name
    public Text curObjText;

    //AR references used to see if placement area is valid.
    public GameObject placementIndicator;
    ARSessionOrigin arOrigin;
    Pose placementPose;
    bool placementPoseIsValid = false;

    //Transform to place all new ar objects spawned. 
    public Transform objHolder;

	void Start ()
    {
        //Find arorigin object and set current object text to current object index within the objects to place list
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        curObjText.text = objectsToPlace[objIndex].name;
	}
	
	void Update ()
    {
        //Update pose and indicators current location
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        //Check if the area is valid, the input count is greater than 0, and touchphase is the beginning.
        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //If all parameters are met, place obj
            if (objHolder.childCount < maxObjs)
                PlaceObject();
        }
	}

    //Places an object and sets the parent to the objectHolder
    public void PlaceObject()
    {
        if (objHolder.childCount >= maxObjs)
        {
            curObjText.text = "Max number of objects placed.";
            return;
        }

        GameObject newObj = Instantiate(objectsToPlace[objIndex], placementPose.position, placementPose.rotation);
        //Debug place
       // GameObject newObj = Instantiate(objectsToPlace[objIndex], placementIndicator.transform.position, placementIndicator.transform.rotation);


        persistentAR.AddARObject(objIndex, newObj.name, newObj.transform.position, newObj.transform.rotation);
        newObj.transform.SetParent(objHolder);
    }

    private void UpdatePlacementIndicator()
    {
        //If the pose is valid, turn on, and update position rotation,
        if(placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        //Turn off the indicator.
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    //Cast array from main camera screenpoint out, tracks raycast to check if any trackable types were hit
    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arOrigin.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;

        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.main.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    //Incriments or decriments objIndex. Changing object to place.
    public void ChangeObjToPlace(int _value)
    {
        objIndex = _value;

        if(objIndex >= objectsToPlace.Length)
        {
            objIndex = 0;
        }
        if(objIndex < 0)
        {
            objIndex = objectsToPlace.Length - 1;
        }

        curObjText.text = objectsToPlace[objIndex].name;
    }
}