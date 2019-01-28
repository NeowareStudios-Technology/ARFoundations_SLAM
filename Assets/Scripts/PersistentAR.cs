/******************************************************
*Project: Persistent AR
*Created by: Colton Spruill
*Date: 20190128
*Description: Save and load placed objects
*Copyright 2019 LeapWithAlice,LLC. All rights reserved
 ******************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class PersistentAR : MonoBehaviour
{
    //Ref to arTap to place objects script and a new list of ar objects
    public ARTapToPlaceObject aRTapToPlaceObject;
    public List<ARObject> arObjs = new List<ARObject>();
    public Text saveSlotText;
    public int curSaveSlot;

    private void Start()
    {
        //Set cur save slot to 0, set save slot text
        curSaveSlot = 0;
        saveSlotText.text = "" + curSaveSlot;
    }

    #region Functions
    //ChangeSave Slot
    public void ChangeSaveSlot()
    {
        //Increment save slot
        curSaveSlot++;

        //Check if its out of bounds
        if (curSaveSlot >= 3)
        {
            curSaveSlot = 0;
        }
        //Update save slot text
        saveSlotText.text = "" + curSaveSlot;
    }

    //Save current list of arobjects in scene. Up to 5.
    public void SaveARObjects(List<ARObject> _arObjs)
    {
        //If no directory exist for this slot, create one
        if (!Directory.Exists("ARObjectsSlot" + curSaveSlot))
            Directory.CreateDirectory("ARObjectsSlot" + curSaveSlot);

        //Index = 0
        int index = 0;

        //Save each object into the save location with an index as a seperator.
        foreach (ARObject gO in _arObjs)
        {
            File.WriteAllText("ARObjectsSlot" + curSaveSlot + "/" + index, JsonUtility.ToJson(gO));
            index++;
            print("ARObjectsSlot" + curSaveSlot + "/" + index + JsonUtility.ToJson(gO));
        }
    }

    //Re load list of ar objects
    public List<ARObject> LoadARObjects()
    {
        //Try to find directory by using current save slot
        if (Directory.Exists("ARObjectsSlot"  + curSaveSlot))
        {
            print("Directory Found! Looking for objects.");
            //Get files from directory
            string[] info = Directory.GetFiles("ARObjectsSlot" + curSaveSlot);

            //Add these objects into the ar objects list
            foreach (string str in info)
            {
                print("Object Found");
                arObjs.Add(JsonUtility.FromJson<ARObject>(File.ReadAllText(str)));
            }
        }
        else
        {
            //If no directory found, tell console.
            print("No directory found.");
        }
        return arObjs;
    }

    //Clear the ar objects list and connected objects in scene.
    public void ClearARObjects(List<ARObject> _arObjs)
    {
        //Clear out ar objects list
        _arObjs.Clear();

        //Loop through actuall gameobjects and remove the ones that coincide with the ar objects list
        for (int i = 0; i < aRTapToPlaceObject.objHolder.childCount; i++)
        {
            Destroy(aRTapToPlaceObject.objHolder.GetChild(i).gameObject);
        }
    }

    //Add Ar object, used from artap to place object script. creates new ar obj and ads to list.
    public void AddARObject(int _objIndex, string _name, Vector3 _location, Quaternion _rotation)
    {
        //Create new blank ar object
        ARObject newArObj = new ARObject();
        //set new objects info
        newArObj.arObjNum = _objIndex;
        newArObj.name = _name;
        newArObj.location = _location;
        newArObj.rotation = _rotation;

        //Add new ar object to list
        arObjs.Add(newArObj);
    }

    //Delete ar directory by name "ARObjects"
    public void DeleteARDirectory(string target_dir)
    {
        //If the target directory is null, go away
        if (target_dir == null) return;

        //Get files and directories by string
        string[] files = Directory.GetFiles(target_dir);
        string[] dirs = Directory.GetDirectories(target_dir);

        //If no files, no files found
        if (files == null)
        {
            print("No files found");
        }
        //If no directories found, no directories found, go away
        if (dirs == null)
        {
            print("No directories found");
            return;
        }
        else
        {
            //If we do find files, tell console
            if (files != null)
            {
                print("Files found");

                //Loop and set attributes for files, then delete files
                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
            }
            //If we find a directory
            if (dirs != null)
            {
                //Loop and delete the specified directory
                foreach (string dir in dirs)
                {
                    DeleteARDirectory(dir);
                }

                Directory.Delete(target_dir, false);
                print("Directory destroyed");
            }
        }
    }
    #endregion

    #region Buttons
    //Delete directory for saves
    public void DeleteARDirectoryButton()
    {
        //If we find the directory, Destroy it.
        if (Directory.Exists("ARObjectsSlot" + curSaveSlot))
            DeleteARDirectory("ARObjectsSlot" + curSaveSlot);
        else
        {
            print("No ARObjects directory found");
        }
    }
    //Load button, pulls info and sets in into new objects
    public void LoadARObjectsButton()
    {
        if (aRTapToPlaceObject.objHolder.childCount != 0) return;

        arObjs = LoadARObjects();

        for (int i = 0; i < arObjs.Count; i++)
        {
            string newName = arObjs[i].name;
            Vector3 newLocation = arObjs[i].location;
            Quaternion newRotation = arObjs[i].rotation;

            GameObject newObj = Instantiate(aRTapToPlaceObject.objectsToPlace[arObjs[i].arObjNum], newLocation, newRotation);
            newObj.transform.SetParent(aRTapToPlaceObject.objHolder);
        }
    }

    //Save ARobjects button
    public void SaveARObjectsButton()
    {
        SaveARObjects(arObjs);
    }
    //Clear arobjects button
    public void ClearARObjectsButton()
    {
        ClearARObjects(arObjs);
    }
    #endregion

    //AR object class for saved object information.
    [System.Serializable]
    public class ARObject
    {
        public int arObjNum = 0;
        public string name = "Placeholder";
        public Vector3 location = Vector3.zero;
        public Quaternion rotation = Quaternion.identity;
    }
}