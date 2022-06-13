using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spawn;
    public float timeBetweenSpawns;
    public roomController rc;
    private float lastSpawnTime;

    void Start()
    {
        lastSpawnTime = Time.time;

    }
    // Update is called once per frame
    void Update()
    {
        

        if(Time.time - lastSpawnTime > timeBetweenSpawns)
        {
            Transform location = new GameObject().transform;
            location.position = new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z);
            rc.activeEnemies.Add(Instantiate(spawn, location));
            lastSpawnTime = Time.time;
        }
    }
}
