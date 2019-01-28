using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Placenote;

public class Dragon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MultiplayerController.Instance.RegisterDragon(gameObject);
    }
}