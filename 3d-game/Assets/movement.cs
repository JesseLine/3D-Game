using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float speed;
    public float gravity;

    CharacterController cc;

    private Vector3 prevVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();


    }

    // Update is called once per frame
    void Update()
    {
        doVelocity();

    }

    void doVelocity()
    {
        Vector3 velocity = Vector3.zero;
        velocity += transform.right * Input.GetAxis("Horizontal");
        velocity += transform.forward * Input.GetAxis("Vertical");

        velocity = Vector3.Normalize(velocity);

        velocity *= speed;

        velocity += (Vector3.down * gravity);



        cc.Move(velocity * Time.deltaTime);
    }
}
