using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class energyOrb : MonoBehaviour
{
    // Start is called before the first frame update
    public float energy = 1;
    public float force;
    
    Rigidbody rb;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    void Update()
    {
        transform.LookAt(movement2.player.transform);

        rb.AddForce(transform.forward * Time.deltaTime * force);

    }
    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        //print(col.gameObject.name);
        if(col.gameObject.name == "Player")
        {
            playerController.AddEnergy(energy);
            GameObject.Destroy(gameObject);
        }
    }
}
