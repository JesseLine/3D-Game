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
    public int difficulty;
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

        List<GameObject> enemyList = GenerateEnemies();

    }

    public GameObject[] PossibleEnemies;

    private List<GameObject> GenerateEnemies()
    {
        //Given difficulty, create a list of enemies that is pretty close to that difficulty
        //from the list of enemies, pull 1 that has a difficulty less than or equal to remaining difficulty, until difficulty is less than like 2
        List<GameObject> enemyList = new List<GameObject>();
        while(difficulty > 2)
        {
            GameObject e = PossibleEnemies[(int)(Random.value * PossibleEnemies.Length)];
            while(e.GetComponent<enemyInfo>().challenge > difficulty)
            {
                e = PossibleEnemies[(int)(Random.value * PossibleEnemies.Length)];
            }
            difficulty -= e.GetComponent<enemyInfo>().challenge;
            enemyList.Add(e);
        }
        return enemyList;

    }
}
