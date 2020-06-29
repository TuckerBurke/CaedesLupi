using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyPatrol : MonoBehaviour
{
    NavNodeMeshScript navMesh;

    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponentInChildren<NavNodeMeshScript>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
