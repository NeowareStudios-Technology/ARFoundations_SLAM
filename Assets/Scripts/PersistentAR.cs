using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PersistentAR : MonoBehaviour
{
    public ARTapToPlaceObject aRTapToPlaceObject;
    public List<ARObject> arObjs = new List<ARObject>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveARObjects(arObjs);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ClearARObjects(arObjs);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            arObjs = LoadARObjects();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Directory.Exists("ARObjects"))
                DeleteARDirectory("ARObjects");
            else
            {
                print("No ARObjects directory found");
            }
        }
    }

   public void SaveARObjects(List<ARObject> _arObjs)
    {
        print("Saving AR Objects");

        if (!Directory.Exists("ARObjects"))
            Directory.CreateDirectory("ARObjects");

        int index = 0;

        foreach (ARObject gO in _arObjs)
        {
            File.WriteAllText("ARObjects/" + index, JsonUtility.ToJson(gO));
            index++;
            print("" + index + JsonUtility.ToJson(gO));
        }
    }

    public List<ARObject> LoadARObjects()
    {
      // List<ARObject> arObjs = new List<ARObject>();

        if (Directory.Exists("ARObjects"))
        {
            print("Directory Found");
            string[] info = Directory.GetFiles("ARObjects");

            foreach (string str in info)
            {
                print("Object Found");
                arObjs.Add(JsonUtility.FromJson<ARObject>(File.ReadAllText(str)));                            
            }
        }
        else
        {
            print("No directory found.");
        }
        return arObjs;
    }

    public void ClearARObjects(List<ARObject> _arObjs)
    {
        _arObjs.Clear();

        for (int i = 0; i < aRTapToPlaceObject.objHolder.childCount; i++)
        {
           Destroy(aRTapToPlaceObject.objHolder.GetChild(i).gameObject);
        }
    }

    public void LoadARObjectsButton()
    {
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

    public void SaveARObjectsButton()
    {
        SaveARObjects(arObjs);
    }
        public void ClearARObjectsButton()
    {
        ClearARObjects(arObjs);
    }

    public void AddARObject(int _objIndex, string _name, Vector3 _location, Quaternion _rotation)
    {
        ARObject newArObj = new ARObject();
        newArObj.arObjNum = _objIndex;
        newArObj.name = _name;
        newArObj.location = _location;
        newArObj.rotation = _rotation;

        arObjs.Add(newArObj);
    }

    public void DeleteARDirectory(string target_dir)
    {
        if (target_dir == null) return; 

        string[] files = Directory.GetFiles(target_dir);
        string[] dirs = Directory.GetDirectories(target_dir);


        if (files == null)
        {
            print("No files found");
        }
        if (dirs == null)
        {
            print("No directories found");
            return;
        }
        else
        {
            if (files != null)
            {
                print("Files found");

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
            }
            if (dirs != null)
            {
                foreach (string dir in dirs)
                {
                    DeleteARDirectory(dir);
                }

                Directory.Delete(target_dir, false);
                print("Directory destroyed");
            }
        }
    }

    [System.Serializable]
    public class ARObject
    {
        public int arObjNum = 0;
        public string name = "Placeholder";
        public Vector3 location = Vector3.zero;
        public Quaternion rotation = Quaternion.identity;
    }
}