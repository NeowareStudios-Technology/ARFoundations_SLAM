using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Placenote;

public class Dragon : MonoBehaviour
{
    public Transform barrelOut;
    public GameObject projectile;
    public float projectileForce = 200;
    public int multiplier = 200;

    // Start is called before the first frame update
    void Start()
    {
        MultiplayerController.Instance.RegisterDragon(gameObject);
    }

    private void Update()
    {
        
    }

    public void Fire()
    {
        Instantiate(projectile, barrelOut, this);
        projectile.GetComponent<Rigidbody>().AddForce(barrelOut.forward * multiplier * projectileForce);
    }
}