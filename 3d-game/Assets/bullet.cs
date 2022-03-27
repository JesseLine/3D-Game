using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage;

    void OnTriggerEnter(Collider col)
    {
        col.gameObject.SendMessage("takeDamage", damage);
        GameObject.Destroy(gameObject);

    }
}
