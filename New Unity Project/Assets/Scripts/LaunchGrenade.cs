using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchGrenade : MonoBehaviour
{
    public GameObject projectile;
    private Transform myTransform;
    // Start is called before the first frame update
    void Start()
    {
        SetInitialReferences();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            SpawnGrenade();
        }
    }

    void SpawnGrenade()
    {
        Instantiate(projectile, myTransform.transform.TransformPoint(0f, 0f, 2f), myTransform.rotation);
    }

    void SetInitialReferences()
    {
        myTransform = transform;
    } 
}
