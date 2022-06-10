using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entranceTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public roomController r;
    void OnTriggerEnter(Collider col)
    {
        r.triggerEntrance();
    }
}
