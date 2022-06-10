using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateAround : MonoBehaviour
{
    

    public UnityEngine.AI.NavMeshAgent agent;

    public float fleeDistance;
    public float wigglyness;

    private float lastSwapTime = 0;
    public int direction = 0;

    // Start is called before the first frame update
    void Start()
    {

        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        lastSwapTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Transform playerT = movement2.player.transform;
        transform.LookAt(playerT);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        Vector3 movement = transform.forward;
        float distance = Vector3.Distance(playerT.position, transform.position);

        if(wigglyness > 0 && Time.time - lastSwapTime > (1 / wigglyness))
        {
            //print("swapping!");
            if(Random.Range(0,10) > 5)
            {
                direction = 0;
            }
            else
            {
                direction = 3;
            }
            lastSwapTime = Time.time;
        }

        
        if(distance < fleeDistance - 1)
        {
            direction = (3 * (direction / 3)) + 1;
        }
        if(distance > fleeDistance + 1)
        {
            direction = (3 * (direction / 3)) + 2;
        }
        

        switch (direction)
        {
            case 0:
                movement = Quaternion.Euler(0, 90, 0) * movement;
                break;
            case 1:
                movement = Quaternion.Euler(0, 135, 0) * movement;
                break;
            case 2:
                movement = Quaternion.Euler(0, 45, 0) * movement;
                break;
            case 3:
                movement = Quaternion.Euler(0, -90, 0) * movement;
                break;
            case 4:
                movement = Quaternion.Euler(0, -135, 0) * movement;
                break;
            case 5:
                movement = Quaternion.Euler(0, -45, 0) * movement;
                break;

        }

        movement = Vector3.Normalize(movement);

        movement *= 5;

        Vector3 dest = new Vector3(transform.position.x + movement.x, transform.position.y + movement.y, transform.position.z + movement.z);


        agent.destination = dest;



    }
}
