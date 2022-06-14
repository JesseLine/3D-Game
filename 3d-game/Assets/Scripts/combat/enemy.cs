using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public GameObject energyOrb;
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
        int orbCount = 3 + gameObject.GetComponent<enemyInfo>().challenge + (int) (worldGen.NextGaussian() * 2);
        for (int i = 0; i < orbCount; i++)
        {
            Transform t = new GameObject().transform;

            t.position = transform.position;
            t.position = new Vector3(t.position.x + worldGen.NextGaussian(), t.position.y + worldGen.NextGaussian(), t.position.z + Mathf.Abs(worldGen.NextGaussian()));
            
            print(transform.position);
            print(t.position);
            Instantiate(energyOrb, t);
        }
        GameObject.Destroy(gameObject);
    }
}
