using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject NorthEntrance, SouthEntrance, WestEntrance, EastEntrance;
    public bool northConnection, southConnection, westConnection, eastConnection;
    private bool started = false;
    public bool endRoom = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (endRoom)
        {
            Open();
        }
    }

    public void SetEntrances(bool northOpen, bool southOpen, bool westOpen, bool eastOpen)
    {
        NorthEntrance.SetActive(!northOpen);
        SouthEntrance.SetActive(!southOpen);
        WestEntrance.SetActive(!westOpen);
        EastEntrance.SetActive(!eastOpen);
    }

    public void Close()
    {
        SetEntrances(false, false, false, false);
    }

    public void Open()
    {
        SetEntrances(northConnection, southConnection, westConnection, eastConnection);
    }

    public void triggerEntrance()
    {
        if (!started)
        {
            started = true;
            Close();
        }
    }
}
