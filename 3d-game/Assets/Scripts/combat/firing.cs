using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firing : MonoBehaviour
{
    public GameObject bullet;
    private float _nextShotTime;

    public float fireSpeed = .3f;
    public float bulletSpeed = 13;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Fire1") == 1 && Time.time > _nextShotTime)
        {
            _nextShotTime = Time.time + fireSpeed;
            createBullet();

        }
    }

    void createBullet()
    {
        Vector3 lookDirection = gameObject.transform.forward;
        Vector3 position = gameObject.transform.position + lookDirection;
        Vector3 velocity = bulletSpeed * lookDirection;
        GameObject newBullet = Instantiate(bullet, position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().velocity = velocity;

    }
}
