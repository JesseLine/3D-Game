using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public int hp;
    // Update is called once per frame
    void takeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            die();
        }
    }
    void die()
    {
        GameObject.Destroy(gameObject);
    }
}
