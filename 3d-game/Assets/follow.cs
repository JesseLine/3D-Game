using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class follow : MonoBehaviour
{
    public NavMeshAgent agent;

    private Transform t;
    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        agent.destination = movement.player.transform.position;
    }
}
