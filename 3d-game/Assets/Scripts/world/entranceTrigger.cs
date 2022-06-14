using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entranceTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public roomController r;
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            r.triggerEntrance();
        }
        
    }
}
