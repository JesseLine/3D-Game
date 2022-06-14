using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage;

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag != "BulletIgnore")
        {
            col.gameObject.SendMessage("takeDamage", damage, SendMessageOptions.DontRequireReceiver);
            GameObject.Destroy(gameObject);
        }
        

    }
}
