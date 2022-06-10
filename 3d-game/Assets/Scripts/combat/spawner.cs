using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spawn;
    public float timeBetweenSpawns;

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
            Transform location = transform;
            location.position.Set(location.position.x, location.position.y - 1.5f, location.position.z);
            Instantiate(spawn, location);
            lastSpawnTime = Time.time;
        }
    }
}
