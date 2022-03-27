using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float speed;
    public float gravity;
    public float sprintMult = 2;

    public int frames;

    public bool grounded = false;

    public float holdingJumpMult = .5f;

    public float jumpSpeed;

    private float radius = .5f;
    private float height = 2;

    private float gravMult = 1;

    private Vector3 prevVelocity = Vector3.zero;

    public static GameObject player;

    void Awake()
    {
        player = gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        

        jumpTime = Time.time - .1f;
        Application.targetFrameRate = frames;

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

    private float jumpTime;
    void doInputs()
    {
        //add the way we want to move to previous velocity to find the way we are trying to move now
        //if we're grounded, also jump if we try to
        float speedMult;

        if (Input.GetAxis("Run") == 1)
        {
            speedMult = sprintMult;

        }
        else
        {
            speedMult = 1;

        }

        Vector3 forward = Vector3.Normalize(new Vector3(transform.forward.x, 0, transform.forward.z));
        Vector3 right = Vector3.Normalize(new Vector3(transform.right.x, 0, transform.right.z));

        newVelocity += right * Input.GetAxis("Horizontal");
        newVelocity += forward * Input.GetAxis("Vertical");

        newVelocity = Vector3.Normalize(newVelocity);

        newVelocity *= speed * speedMult;

        
        if (Input.GetAxis("Jump") == 1)
        {
            gravMult = holdingJumpMult;
            if (grounded)
            {
                jumpTime = Time.time;
                print("setting speed");

                newVelocity += Vector3.up * jumpSpeed;

            }
        }
        else
        {
            gravMult = 1f;
        }



    }


    void doGravity()
    {
        float newYVel = newVelocity.y;
        if (newYVel == 0)
        {
            newYVel = prevVelocity.y;
        }
         newYVel += -gravity *gravMult * Time.deltaTime;

// print(newYVel);

        RaycastHit hit;
        if (Time.time - jumpTime > .1 && Physics.SphereCast(transform.position, radius, Vector3.down, out hit, height + min(newYVel, -.01f)))
        {
            //ground found
            transform.position = new Vector3(transform.position.x, hit.point.y + height, transform.position.z);
            newVelocity = new Vector3(newVelocity.x, 0, newVelocity.z);

            grounded = true;
        }
        else
        {
            grounded = false;
            newVelocity = new Vector3(newVelocity.x, newYVel, newVelocity.z);

        }
    }

    void doMove()
    {

        //do a capsule cast along where we want to move, and move as far as we can without hitting something

        RaycastHit hit;

        Vector3 top = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z);
        Vector3 bot = new Vector3(transform.position.x, transform.position.y - 1.4f, transform.position.z);

        if (Physics.CapsuleCast(top, bot, radius, newVelocity, out hit, (newVelocity * Time.deltaTime).magnitude))
        {
           // print(hit.distance);
            //print(newVelocity.magnitude);
            newVelocity *= min((hit.distance / newVelocity.magnitude) -.1f, 0);
        }

        transform.position += newVelocity * Time.deltaTime;
        prevVelocity = newVelocity;



    }
}
