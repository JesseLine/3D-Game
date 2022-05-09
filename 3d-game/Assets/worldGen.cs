using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public GameObject start;
    public GameObject k1x1Enemy;
    public GameObject store;
    public GameObject end;
    class Room
    {
        public Vector3 location;
        public Vector3 size;
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

            int size = 10;
            switch (type)
            {
                case RoomType.kStart:
                    size = 10;
                    break;
                case RoomType.k1x1Enemy:
                    size = 10;
                    break;
                case RoomType.kStore:
                    size = 10;
                    break;
                case RoomType.kEnd:
                    size = 20;
                    break;
            }

            Vector3 center = new Vector3(location.x * 30, location.y * 30, location.z * 30);
            //center += new Vector3(Random.Range(-14 + size / 2, 28 - (size / 2)), Random.Range(-14 + size / 2, 28 - (size / 2)), 0);

            Transform t = new GameObject().transform;
            t.position = new Vector3(center.x, center.z, center.y);
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
                case RoomType.kStore:
                    room = g.store;
                    break;
                case RoomType.kEnd:
                    room = g.end;
                    break;
            }

            Instantiate(room, t);

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
            r.location = Vector3.zero;
            Room previous = null;
            Room destination = this.start;
            char choice = 'b';

            while(destination != null)
            {
                WeightedRandomizer wRand = new WeightedRandomizer();
                wRand.Add('n', 5 * ((destination.north == null) ? 1 : roomWeight));
                wRand.Add('s', 5 * ((destination.south == null) ? 1 : roomWeight));
                wRand.Add('w', 5 * ((destination.west == null) ? 1 : roomWeight));
                wRand.Add('e', 5 * ((destination.east == null) ? 1 : roomWeight));

                wRand.Add('u', 2 * ((destination.up == null) ? 1 : roomWeight));
                wRand.Add('d', 2 * ((destination.down == null) ? 1 : roomWeight));

                choice = wRand.Pick();

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
    public enum RoomType
    {
        kStart,
        k1x1Enemy,
        kStore,
        kEnd
    }
    //creates a 3d array of room locations, along with a list of rooms in order of their appearance

    //1. generate list of rooms

    //2. add them in semi-randomly to a 3d array representing their location

    //3. while there still exists a room without any connections, connect the 2 nearest rooms
    public int minRooms = 10;
    public int maxRooms = 15;
    public static int roomWeight = 2;

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

    private Room getRandomRoom()
    {
        return new Room(RoomType.k1x1Enemy);
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
        //connectRooms(roomArray, roomList);
        foreach(Room r in tree.dict.Values)
        {
            r.build(this);
            //print("building");
        }
    }

}
