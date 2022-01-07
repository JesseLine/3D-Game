using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotation : MonoBehaviour
{
    // Start is called before the first frame update

    public float sensitivity = 1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float newX = transform.eulerAngles.x + Time.timeScale * -sensitivity * mouseY;
        
        while(newX > 360)
        {
            newX -= 360;
        }
        if (newX > 180){
            newX -= 360;
        }
        if (newX < -85) {
            newX = -85;
        }
        else if(newX > 85){
            print(newX);
            newX = 85;
        }
        transform.eulerAngles = new Vector3(newX, transform.eulerAngles.y + Time.timeScale * sensitivity * mouseX, 0);



    }
}
