using System.Collections;
using static System.Math;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class worldGen : MonoBehaviour
{
    void Start()
    {
        createRooms();
    }

    public class WeightedRandomizer
    {
        class Node
        {
            public char o;
            public int weight;
            public Node(char o, int weight)
            {
                this.o = o;
                this.weight = weight;
            }
        }

        private LinkedList<Node> nodes;
        private int totalWeight;

        public WeightedRandomizer()
        {
            nodes = new LinkedList<Node>();
            totalWeight = 0;
        }

        public void Add(char o, int weight)
        {
            totalWeight += weight;
            nodes.AddLast(new Node(o, weight));
        }

        public char Pick()
        {
            int chosen = Random.Range(0,totalWeight);
            int sum = 0;
            foreach(Node n in nodes)
            {
                sum += n.weight;
                if (sum > chosen)
                {
                    return n.o;
                }
            }
            print("PICK IS BAD");
            return 'b';
        }

        public void Print()
        {
            foreach(Node n in nodes)
            {
                print("n: " + n.o + " weight: " + n.weight);
            }
        }
    }

    
    class Room
    {
        public Vector3 location;
        int size;
        public RoomType type;

        public Room north = null;
        public Room south = null;
        public Room east = null;
        public Room west = null;
        public Room up = null;
        public Room down = null;


        public Room(RoomType type)
        {
            this.type = type;
        }

        public override string ToString()
        {
            return location.ToString() + " " + type.ToString();
        }


        public void build(worldGen g)
        {
            //1. find center location in world coordinates based on location value
            //2. instantiate prefab at given location
            switch (type)
            {
                case RoomType.kStart:
                    size = 10;
                    break;
                case RoomType.k1x1Enemy:
                    size = 13;
                    break;
                case RoomType.kSmall:
                    size = 5;
                    break;
                case RoomType.kLarge:
                    size = 20;
                    break;
                case RoomType.kStore:
                    size = 10;
                    break;
                case RoomType.kEnd:
                    size = 13;
                    break;
            }

            Vector3 center = new Vector3(location.x * 30, location.y * 30, location.z * 30);

            if(type != RoomType.kStart)
            {
                center += new Vector3(Random.Range(-14 + size / 2, 14 - (size / 2)), Random.Range(-14 + size / 2, 14 - (size / 2)), 0);
            }
            //center += new Vector3(Random.Range(-14 + size / 2, 28 - (size / 2)), Random.Range(-14 + size / 2, 28 - (size / 2)), 0);

            Transform t = new GameObject().transform;
            t.position = new Vector3(center.x, center.z, center.y);
            location = t.position;
            GameObject room = null;
            switch (type)
            {
                case RoomType.kStart:
                    room = g.start;
                    print("found start");
                    break;
                case RoomType.k1x1Enemy:
                    room = g.k1x1Enemy;
                    break;
                case RoomType.kSmall:
                    room = g.small;
                    break;
                case RoomType.kLarge:
                    room = g.large;
                    break;
                case RoomType.kStore:
                    room = g.store;
                    break;
                case RoomType.kEnd:
                    room = g.end;
                    break;
            }

            Instantiate(room, t);
        }


        public Vector3 getConnection(string dir)
        {
            //only really works after build has been called (location has to be set up properly)
            if(dir == "north")
            {
                return new Vector3(location.x, location.y, location.z + size / 2);
            }
            else if(dir == "south")
            {
                return new Vector3(location.x, location.y, location.z - size / 2);
            }
            else if(dir == "west")
            {
                return new Vector3(location.x - size / 2, location.y, location.z);
            }
            else if(dir == "east")
            {
                return new Vector3(location.x + size / 2, location.y, location.z);
            }
            else if(dir == "up")
            {
                return new Vector3(location.x, location.y + 2, location.z);
            }
            else if(dir == "down")
            {
                return new Vector3(location.x, location.y - 1, location.z);
            }
            else
            {
                print("get Connection fail! bad input of " + dir + ".");
                return Vector3.zero;
            }
        }

        public void buildConnections(worldGen g)
        {
            //only do north, west, and up - others therefore will get handled

            //for each direction: if it's not null, we have to build a path to it. 
            //1. get current (direction) exit location, and its opposite on the other room
            //2. find connection points
            //  for up / down, first build "ladders" to bring to same height, then continue with new points
            // 1 plane (vert) should always be the same - so we should only need 2 more turning points
            // pick a distance between the 2 to meet (randomly), then create those junction points
            // given points, instantiate appropriate prefabs.

            float distance = Random.Range(.3f, .7f);
            Vector3 location1, location2;
            Transform inter1, inter2;
            inter1 = new GameObject().transform;
            inter2 = new GameObject().transform;

            if (north != null)
            {
                location1 = getConnection("north");
                location2 = north.getConnection("south");
                int newZ = (int) ((distance * Abs(location1.z - location2.z)) + Min(location1.z, location2.z));
                inter1.position = new Vector3(location1.x, location1.y, newZ);
                inter2.position = new Vector3(location2.x, location2.y, newZ);
                if(inter1.position.x < inter2.position.x)
                {
                    int dif = (int) (inter2.position.x - inter1.position.x);
                    if(dif == 3)
                    {
                        Instantiate(g.UpDownSmallIntRight3, inter1);
                    }
                    else if(dif == 2)
                    {
                        Instantiate(g.UpDownSmallIntRight2, inter1);
                    }
                    else if(dif == 1)
                    {
                        Instantiate(g.UpDownSmallIntRight1, inter1);
                    }
                    else
                    {
                        Instantiate(g.DownRightInter, inter1);
                        Instantiate(g.UpLeftInter, inter2);
                        constructPath(new Vector3(inter1.position.x + 2, inter1.position.y, inter1.position.z), new Vector3(inter2.position.x - 2, inter2.position.y, inter2.position.z), g);
                    }
                    constructPath(location1, new Vector3(inter1.position.x, inter1.position.y, inter1.position.z - 2), g);
                    
                    constructPath(new Vector3(inter2.position.x, inter2.position.y, inter2.position.z + 2), location2, g);
                }
                else if (inter1.position.x > inter2.position.x)
                {
                    int dif = (int)(inter1.position.x - inter2.position.x);
                    if (dif == 3)
                    {
                        Instantiate(g.UpDownSmallIntLeft3, inter1);
                    }
                    else if (dif == 2)
                    {
                        Instantiate(g.UpDownSmallIntLeft2, inter1);
                    }
                    else if (dif == 1)
                    {
                        Instantiate(g.UpDownSmallIntLeft1, inter1);
                    }
                    else
                    {
                        Instantiate(g.DownLeftInter, inter1);
                        Instantiate(g.UpRightInter, inter2);
                        constructPath(new Vector3(inter1.position.x - 2, inter1.position.y, inter1.position.z), new Vector3(inter2.position.x + 2, inter2.position.y, inter2.position.z), g);
                    }
                    constructPath(location1, new Vector3(inter1.position.x, inter1.position.y, inter1.position.z - 2), g);

                    constructPath(new Vector3(inter2.position.x, inter2.position.y, inter2.position.z + 2), location2, g);
                    //Instantiate(g.DownLeftInter, inter1);
                    //Instantiate(g.UpRightInter, inter2);
                    //constructPath(location1, new Vector3(inter1.position.x, inter1.position.y, inter1.position.z), g);
                    //constructPath(new Vector3(inter1.position.x, inter1.position.y, inter1.position.z), new Vector3(inter2.position.x, inter2.position.y, inter2.position.z), g);
                    //constructPath(new Vector3(inter2.position.x, inter2.position.y, inter2.position.z), location2, g);
                }
                else
                {
                    constructPath(location1, location2, g);
                }
                //constructPath(location1, new Vector3(inter1.position.x, inter1.position.y, inter1.position.z), g);
                //constructPath(new Vector3(inter1.position.x, inter1.position.y, inter1.position.z), new Vector3(inter2.position.x, inter2.position.y, inter2.position.z), g);
                //constructPath(new Vector3(inter2.position.x, inter2.position.y, inter2.position.z), location2, g);
            }
            if(west != null)
            {
                location1 = getConnection("west");
                location2 = west.getConnection("east");
                int newX = (int)((distance * Abs(location1.x - location2.x)) + Min(location1.x, location2.x));
                inter1.position = new Vector3(newX, location1.y, location1.z);
                inter2.position = new Vector3(newX, location2.y, location2.z);
                if (inter1.position.z < inter2.position.z)
                {
                    int dif = (int)(inter2.position.z - inter1.position.z);
                    if (dif == 3)
                    {
                        Instantiate(g.LeftRightSmallIntUp3, inter1);
                    }
                    else if (dif == 2)
                    {
                        Instantiate(g.LeftRightSmallIntUp2, inter1);
                    }
                    else if (dif == 1)
                    {
                        Instantiate(g.LeftRightSmallIntUp1, inter1);
                    }
                    else
                    {
                        Instantiate(g.UpLeftInter, inter1);
                        Instantiate(g.DownRightInter, inter2);
                        constructPath(new Vector3(inter1.position.x, inter1.position.y, inter1.position.z + 2), new Vector3(inter2.position.x, inter2.position.y, inter2.position.z - 2), g);
                    }
                    constructPath(location1, new Vector3(inter1.position.x - 2, inter1.position.y, inter1.position.z), g);

                    constructPath(new Vector3(inter2.position.x  +2, inter2.position.y, inter2.position.z),location2,  g);
                }
                else if (inter1.position.z > inter2.position.z)
                {
                    int dif = (int)(inter1.position.z - inter2.position.z);
                    if (dif == 3)
                    {
                        Instantiate(g.LeftRightSmallIntDown3, inter1);
                    }
                    else if (dif == 2)
                    {
                        Instantiate(g.LeftRightSmallIntDown2, inter1);
                    }
                    else if (dif == 1)
                    {
                        Instantiate(g.LeftRightSmallIntDown1, inter1);
                    }
                    else
                    {
                        Instantiate(g.DownLeftInter, inter1);
                        Instantiate(g.UpRightInter, inter2);
                        constructPath(new Vector3(inter1.position.x, inter1.position.y, inter1.position.z - 2), new Vector3(inter2.position.x, inter2.position.y, inter2.position.z + 2), g);
                    }
                    constructPath(location1, new Vector3(inter1.position.x - 2, inter1.position.y, inter1.position.z), g);

                    constructPath(new Vector3(inter2.position.x + 2, inter2.position.y, inter2.position.z), location2, g);
                    //Instantiate(g.DownLeftInter, inter1);
                    //Instantiate(g.UpRightInter, inter2);
                    //constructPath(location1, new Vector3(inter1.position.x, inter1.position.y, inter1.position.z), g);
                    //constructPath(new Vector3(inter1.position.x, inter1.position.y, inter1.position.z), new Vector3(inter2.position.x, inter2.position.y, inter2.position.z), g);
                    //constructPath(new Vector3(inter2.position.x, inter2.position.y, inter2.position.z), location2, g);
                }
                else
                {
                    constructPath(location1, location2, g);
                }
            }
            if(up != null)
            {
                location1 = getConnection("up");
                location2 = up.getConnection("down");
                int newY = (int)((location1.y + location2.y) / 2);
                for(int i = 0; i< newY; i++)
                {
                    Transform t = new GameObject().transform;
                    t.position = new Vector3(location1.x, i, location1.z);
                    Instantiate(g.Ladder, t);
                }
                for (int i = newY; i < location2.y; i++)
                {
                    Transform t = new GameObject().transform;
                    t.position = new Vector3(location2.x, i, location2.z);
                    Instantiate(g.Ladder, t);
                }
                location1.Set(location1.x, newY, location1.z);
                location2.Set(location2.x, newY, location2.z);
                float newX = (distance * Abs(location1.x - location2.x)) + Min(location1.x, location2.x);
                inter1.position = new Vector3(newX, location1.y, location1.z);
                inter2.position = new Vector3(newX, location2.y, location2.z);
                if (inter1.position.z > inter2.position.z)
                {
                    Instantiate(g.DownRightInter, inter1);
                    Instantiate(g.UpLeftInter, inter2);
                }
                else if (inter1.position.z < inter2.position.z)
                {
                    Instantiate(g.UpRightInter, inter1);
                    Instantiate(g.DownLeftInter, inter2);
                }
                constructPath(location1, inter1.position, g);
                constructPath(inter1.position, inter2.position, g);
                constructPath(inter2.position, location2, g);
            }


        }

        private void constructPath(Vector3 location1, Vector3 location2, worldGen g)
        {
            //precondition: both points are on the same vertical plane, and 1 of the same horizontal planes
            if(location1 == location2)
            {
                return;
            }
            if(location1.x == location2.x)
            {
                int mi = (int)Min(location1.z, location2.z);
                int ma = (int)Max(location1.z, location2.z);

                for (int i = mi; i < ma; i++)
                {
                    Transform t = new GameObject().transform;
                    t.position = new Vector3(location1.x, location1.y, i);
                    Instantiate(g.LeftRightPath, t);
                }
            }
            else if(location1.z == location2.z)
            {
                int mi = (int)Min(location1.x, location2.x);
                int ma = (int)Max(location1.x, location2.x);
                for (int i = mi; i < ma; i++)
                {
                    Transform t = new GameObject().transform;
                    t.position = new Vector3(i, location1.y, location1.z);
                    Instantiate(g.UpDownPath, t);
                }
            }
            else
            {
                print("Someone gave constructPath bad locations: ");
                print(location1);
                print(location2);
            }


        }
        
    }

    class RoomTree
    {
        public Dictionary<Vector3, Room> dict;
        private Room start;

        public RoomTree()
        {
            dict = new Dictionary<Vector3, Room>();

            this.start = new Room(RoomType.kStart);
            start.location = Vector3.zero;
            dict.Add(start.location, start);

        }

        public void Print()
        {
            foreach(Room r in dict.Values)
            {
                print(r.ToString());
            }
        }
        public void Add(Room r)
        {
            Room destination = dict.ElementAt(Random.Range(0, dict.Count)).Value;
            Room previous = null;
            

            r.location = destination.location;
            char choice = 'b';

            while(destination != null)
            {
                WeightedRandomizer wRand = new WeightedRandomizer();
                wRand.Add('n', 5 * ((destination.north == null) ? 1 : roomWeight) * ((choice == 'n') ? 4 : 1));
                wRand.Add('s', 5 * ((destination.south == null) ? 1 : roomWeight) * ((choice == 's') ? 4 : 1));
                wRand.Add('w', 5 * ((destination.west == null) ? 1 : roomWeight) * ((choice == 'w') ? 4 : 1));
                wRand.Add('e', 5 * ((destination.east == null) ? 1 : roomWeight) * ((choice == 'e') ? 4 : 1));

                wRand.Add('u', 2 * ((destination.up == null) ? 1 : roomWeight));
                wRand.Add('d', 2 * ((destination.down == null) ? 1 : roomWeight));

                //wRand.Print();
                choice = wRand.Pick();
                print(choice);

                previous = destination;
                switch (choice)
                {
                    case 'n':
                        r.location += (new Vector3(0, 1, 0));
                        destination = destination.north;
                        break;
                    case 's':
                        r.location += (new Vector3(0, -1, 0));
                        destination = destination.south;
                        break;
                    case 'w':
                        r.location +=(new Vector3(-1, 0, 0));
                        destination = destination.west;
                        break;
                    case 'e':
                        r.location += (new Vector3(1, 0, 0));
                        destination = destination.east;
                        break;
                    case 'u':
                        r.location += (new Vector3(0, 0, 1));
                        destination = destination.up;
                        break;
                    case 'd':
                        r.location += (new Vector3(0, 0, -1));
                        destination = destination.down;
                        break;
                }
            }

            dict.Add(r.location, r);

            Room other;
            dict.TryGetValue(r.location + new Vector3(0, 0, 1), out other);
            if(other != null)
            {
                r.up = other;
                other.down = r;
            }
            other = null;
            dict.TryGetValue(r.location + new Vector3(0, 0, -1), out other);
            if (other != null)
            {
                r.down = other;
                other.up = r;
            }
            other = null;
            dict.TryGetValue(r.location + new Vector3(0, 1, 0), out other);
            if (other != null)
            {
                r.north = other;
                other.south = r;
            }
            other = null;
            dict.TryGetValue(r.location + new Vector3(0, -1, 0), out other);
            if (other != null)
            {
                r.south = other;
                other.north = r;
            }
            other = null;
            dict.TryGetValue(r.location + new Vector3(1, 0, 0), out other);
            if (other != null)
            {
                r.east = other;
                other.west = r;
            }
            other = null;
            dict.TryGetValue(r.location + new Vector3(-1, 0, 0), out other);
            if (other != null)
            {
                r.west = other;
                other.east = r;
            }

            other = null;
            switch (choice)
            {
                case 'n':
                    previous.north = r;
                    break;
                case 's':
                    previous.south = r;
                    break;
                case 'w':
                    previous.west = r;
                    break;
                case 'e':
                    previous.east = r;
                    break;
                case 'u':
                    previous.up = r;
                    break;
                case 'd':
                    previous.down = r;
                    break;
            }

        }
    }

    public GameObject start;
    public GameObject k1x1Enemy;
    public GameObject small;
    public GameObject large;

    public GameObject store;
    public GameObject end;

    public GameObject DownLeftInter, DownRightInter, UpLeftInter, UpRightInter;
    public GameObject LeftRightPath, UpDownPath;
    public GameObject Ladder;
    public GameObject UpDownSmallIntRight1, UpDownSmallIntRight2, UpDownSmallIntRight3;
    public GameObject UpDownSmallIntLeft1, UpDownSmallIntLeft2, UpDownSmallIntLeft3;
    public GameObject LeftRightSmallIntUp1, LeftRightSmallIntUp2, LeftRightSmallIntUp3;
    public GameObject LeftRightSmallIntDown1, LeftRightSmallIntDown2, LeftRightSmallIntDown3;

    public enum RoomType
    {
        kStart,
        k1x1Enemy,
        kSmall,
        kLarge,
        kStore,
        kEnd
    }

    private Room getRandomRoom()
    {
        WeightedRandomizer wRand = new WeightedRandomizer();
        wRand.Add('1', 10);
        wRand.Add('s', 3);
        wRand.Add('l', 5);

        char choice = wRand.Pick();
        switch (choice)
        {
            case '1':
                return new Room(RoomType.k1x1Enemy);
                
            case 's':
                return new Room(RoomType.kSmall);
                
            case 'l':
                return new Room(RoomType.kLarge);

        }
        return new Room(RoomType.k1x1Enemy);
    }
    //creates a 3d array of room locations, along with a list of rooms in order of their appearance

    //1. generate list of rooms

    //2. add them in semi-randomly to a 3d array representing their location

    //3. while there still exists a room without any connections, connect the 2 nearest rooms
    public int minRooms = 10;
    public int maxRooms = 15;
    public static int roomWeight = 4;

    private LinkedList<Room> getRoomList()
    {
        LinkedList<Room> rooms = new LinkedList<Room>();
        int roomCount = Random.Range(minRooms, maxRooms + 1);
        //rooms.AddLast(new Room(RoomType.kStart, new Vector3(1, 1, 1)));

        for(int i = 0; i < roomCount-2; i++)
        {
            rooms.AddLast(getRandomRoom());
        }

        rooms.AddLast(new Room(RoomType.kEnd));

        return rooms;
    }


    //private static Random rand;
    public void createRooms()
    {
        //rand = new Random();

        LinkedList<Room> roomList = getRoomList();
        RoomTree tree = new RoomTree();
        foreach(Room r in roomList)
        {
            tree.Add(r);
        }
        tree.Print();
        
        foreach(Room r in tree.dict.Values)
        {
            r.build(this);
            //print("building");
        }
        foreach (Room r in tree.dict.Values)
        {
            r.buildConnections(this);
        }
    }

}
