using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject NorthEntrance, SouthEntrance, WestEntrance, EastEntrance;
    public bool northConnection, southConnection, westConnection, eastConnection;
    public GameObject spawnParticles;
    public int size;
    public Vector3 location;
    private bool started = false;
    public bool endRoom = false;
    public int difficulty;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldEndRoom())
        {
            Open();
        }
    }

    bool shouldEndRoom()
    {
        if (waitingForSpawns)
        {
            return false;
        }
        if(activeEnemies == null)
        {
            return false;
        }
        foreach(GameObject g in activeEnemies)
        {
            if(g != null)
            {
                return false;
            }
            

        }
        return true;
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

    public List<GameObject> enemyList;
    public List<GameObject> activeEnemies;
    private bool waitingForSpawns = false;
    public void triggerEntrance()
    {
        if (!started)
        {
            if(difficulty < 3)
            {
                difficulty = 3;
            }
            started = true;
            Close();
            enemyList = GenerateEnemies();
            waitingForSpawns = true;
            activeEnemies = new List<GameObject>();
            foreach (GameObject e in enemyList)
            {
                Generate(e);
            }

        }

        


    }

    private void Generate(GameObject e)
    {
        Transform t = new GameObject().transform;
        t.position = new Vector3(Random.Range(-(size / 2) + 3, (size / 2) - 3) + location.x, location.y + 8, location.z + Random.Range(-(size / 2) + 3, (size / 2) - 3));

        RaycastHit hit;
        if (Physics.Raycast(t.position, Vector3.down, out hit, 10))
        {
            
            t.position = hit.point;
            StartCoroutine(Spawn(e, t));
            
            
        }
        else
        {
            print("SPAWN FAIL. RAYCAST DIDN'T HIT");
            print(t.position);
        }
    }

    private IEnumerator Spawn(GameObject e, Transform t)
    {
        Instantiate(spawnParticles, t);
        yield return new WaitForSeconds(.5f);
        //generate haze
        //spawn object
        waitingForSpawns = false;

        GameObject g = Instantiate(e, t);
        if (g.GetComponent<spawner>())
        {
            g.GetComponent<spawner>().rc = this;
        }
        activeEnemies.Add(g);
        yield return 0;
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
