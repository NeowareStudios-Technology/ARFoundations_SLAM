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
            LoadARObjects();
        }
    }

    void SaveARObjects(List<ARObject> _arObjs)
    {
        print("saving");

        if (!Directory.Exists("ARObjects"))
            Directory.CreateDirectory("ARObjects");

        int index = 0;

        foreach (ARObject gO in _arObjs)
        {
            File.WriteAllText("ARObjects/" + index, JsonUtility.ToJson(gO));
            index++;
           // print(index);
        }
    }

    List<ARObject> LoadARObjects()
    {
        List<ARObject> arObjs = new List<ARObject>();

        if (Directory.Exists("ARObjects"))
        {
            string[] info = Directory.GetFiles("ARObjects", "*.arObj");

            foreach (string str in info)
            {
            print("Directory Found");
                arObjs.Add(JsonUtility.FromJson<ARObject>(File.ReadAllText(str)));                
            }
        }
        return arObjs;
    }

    void ClearARObjects(List<ARObject> _arObjs)
    {
        _arObjs.Clear();

        for (int i = 0; i < aRTapToPlaceObject.objHolder.childCount; i++)
        {
           Destroy(aRTapToPlaceObject.objHolder.GetChild(i).gameObject);
        }
    }

    public void AddARObject()
    {
        ARObject newArObj = new ARObject();
        arObjs.Add(newArObj);
    }

    [System.Serializable]
    public class ARObject
    {
        public string name;
        public Vector3 location;
    }
}