using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement2 : MonoBehaviour
{
    CharacterController cc;

    private Vector3 newVelocity;
    public Vector3 prevVelocity;
    public float sprintMult;
    public float speed;
    public float gravMult;
    public float holdingJumpMult;
    public float jumpSpeed;
    public float gravity;
    public Transform camTrans;

    public static GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;
        cc = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        newVelocity = Vector3.zero;
        doInputs();
        cc.Move(newVelocity);
    }

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

        Vector3 forward = Vector3.Normalize(new Vector3(camTrans.forward.x, 0, camTrans.forward.z));
        Vector3 right = Vector3.Normalize(new Vector3(camTrans.right.x, 0, camTrans.right.z));

        newVelocity += right * Input.GetAxis("Horizontal");
        newVelocity += forward * Input.GetAxis("Vertical");

        


        

        newVelocity = Vector3.Normalize(newVelocity);

        if (Input.GetAxis("Jump") == 1)
        {
            gravMult = holdingJumpMult;
            if (cc.isGrounded)
            {
                print("setting speed");

                newVelocity += Vector3.up * jumpSpeed;
            }
        }
        else
        {
            gravMult = 1f;
        }

        if (!cc.isGrounded)
        {
            newVelocity.Set(newVelocity.x, prevVelocity.y + gravity * gravMult * Time.deltaTime, newVelocity.z);
        }
        

        prevVelocity = newVelocity;

        

        newVelocity *= speed * speedMult * Time.deltaTime;

    }
}
