using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float speed;
    public float gravity;
    public bool grounded = false;

    private float radius = .5f;
    private float height = 2;

    private Vector3 prevVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame

    Vector3 newVelocity;

    void Update()
    {
        newVelocity = Vector3.zero;

        doInputs();

        doGravity();

        doMove();

    }

    float min(float v1, float v2)
    {
        if(v1 < v2)
        {
            return v1;
        }
        return v2;
    }


    void doInputs()
    {
        //add the way we want to move to previous velocity to find the way we are trying to move now
        //if we're grounded, also jump if we try to
        Vector3 forward = Vector3.Normalize(new Vector3(transform.forward.x, 0, transform.forward.z));
        Vector3 right = Vector3.Normalize(new Vector3(transform.right.x, 0, transform.right.z));

        newVelocity += right * Input.GetAxis("Horizontal");
        newVelocity += forward * Input.GetAxis("Vertical");

        newVelocity = Vector3.Normalize(newVelocity);

        newVelocity *= speed * Time.deltaTime;


    }

    void doGravity()
    {
        float newYVel = prevVelocity.y - gravity * Time.deltaTime;

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, radius, Vector3.down, out hit, height + min(newYVel, -.01f)))
        {
            //ground found
            transform.position = new Vector3(transform.position.x, hit.point.y + height, transform.position.z);
            grounded = true;
        }
        else
        {
            grounded = false;
            newVelocity += Vector3.up * newYVel;

        }
    }

    void doMove()
    {

        //do a capsule cast along where we want to move, and move as far as we can without hitting something

        RaycastHit hit;

        Vector3 top = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z);
        Vector3 bot = new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z);

        if (Physics.CapsuleCast(top, bot, radius, newVelocity, out hit, newVelocity.magnitude))
        {
           // print(hit.distance);
            //print(newVelocity.magnitude);
            newVelocity *= (hit.distance / newVelocity.magnitude) -.1f;
        }

        transform.position += newVelocity;
        prevVelocity = newVelocity;



    }
}
