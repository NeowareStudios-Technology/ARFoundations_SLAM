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
using UnityEngine.EventSystems;

public class ARTapToPlaceObject : MonoBehaviour
{
    //Ref to persistent ar
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
    public bool isDetecting;
    ARSessionOrigin arOrigin;
    Pose placementPose;
    bool placementPoseIsValid = false;
    public Text poseUpdateText;
    public Text posUpdateTex;

    //Transform to place all new ar objects spawned. 
    public Transform objHolder;

	void Start ()
    {
        //Find arorigin object and set current object text to current object index within the objects to place list
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        curObjText.text = objectsToPlace[objIndex].name;
        isDetecting = true;
	}

    void Update()
    {
                //Update pose and indicators current location
                UpdatePlacementPose();
                UpdatePlacementIndicator();

                //Check if the area is valid, the input count is greater than 0, and touchphase is the beginning.
                if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && isDetecting && !IsPointerOverUIObject())
                {
                    //If all parameters are met, place obj
                    if (objHolder.childCount < maxObjs)
                        PlaceObject();
                }
    }

    //Places an object and sets the parent to the objectHolder
    public void PlaceObject()
    {
        //If objholder child count is greater than max obj number. Go away.
        if (objHolder.childCount >= maxObjs)
        {
            curObjText.text = "Max number of objects placed.";
            return;
        }

        //If we are still under max obj numbers, create new object based on current obj index.
        GameObject newObj = Instantiate(objectsToPlace[objIndex], placementPose.position, placementPose.rotation);
        //Create a new ar object that corresponds to this new obj
        persistentAR.AddARObject(objIndex, newObj.name, newObj.transform.position, newObj.transform.rotation);
        //Set the parent to the objholder
        newObj.transform.SetParent(objHolder);
    }

    private void UpdatePlacementIndicator()
    {
        //If the pose is valid, turn on, and update position rotation,
        if(placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            poseUpdateText.text = "Valid Pose? " + placementPoseIsValid;
            posUpdateTex.text = "" + placementIndicator.transform.position + " / " + placementIndicator.transform.rotation;
        }
        //Turn off the indicator.
        else
        {
            placementIndicator.SetActive(false);
            poseUpdateText.text = "Valid Pose? " + placementPoseIsValid;
        }
    }

    //Cast array from main camera screenpoint out, tracks raycast to check if any trackable types were hit
    private void UpdatePlacementPose()
    {
        //Screen center is in middle of actual viewport
        Vector3 screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        //create list of arraycast hits
       List<ARRaycastHit> hits = new List<ARRaycastHit>();
        //AR origin raycast, does it hit a plane?
        arOrigin.Raycast(screenCenter, hits, TrackableType.Planes);

        //If hits are not 0, there is a valid placement location
        placementPoseIsValid = hits.Count > 0;

        //If true
        if (placementPoseIsValid)
        {
            //Set placement pose to hit location
            placementPose = hits[0].pose;

            //Set camera forward and bearing
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            //Set actual rotation of object to camera rotation
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    //Incriments or decriments objIndex. Changing object to place.
    public void ChangeObjToPlace(int _value)
    {
        //Index equals new value
        objIndex = _value;

        //If larger than index, set to 0
        if(objIndex >= objectsToPlace.Length)
        {
            objIndex = 0;
        }
        //If less than 0 set to largest index in array
        if(objIndex < 0)
        {
            objIndex = objectsToPlace.Length - 1;
        }

        //Set the text to current object name
        curObjText.text = objectsToPlace[objIndex].name;
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void SetDetection(bool _value)
    {
        isDetecting = _value;
    }
}