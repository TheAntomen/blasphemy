using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

/// <summary>
/// Class for generating grid for dungeon
/// TODO: Load obstacles through tilemap from object, put into population list in Room class. (måste ändra för hand i room prefabs)
/// </summary>
public class DungeonGenerator : MonoBehaviour
{
    // Properties
    public Room currentRoom { get; set; }

    // Public variables
    public int currentFloor = 1;
    
    public string enteredFrom;  // Where the player entered from, used to calculate player spawn in new rooms
    public static DungeonGenerator instance;

    // Private variables
    [SerializeField]
    private int numberOfRooms;
    private Room[,] rooms;
    private Room startRoom;
    public Room lastRoom;
    public int count = 0;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            currentRoom = GenerateDungeon();
            startRoom = currentRoom;
        }
    }
    /// <summary>
    /// Called for every new room, updating the instance
    /// </summary>
    public void UpdateDungeon()
    {
        string roomPrefabName = instance.currentRoom.PrefabName();
        GameObject roomObject = (GameObject)Instantiate(Resources.Load("Rooms/" + roomPrefabName));
        Tilemap obstacleTiles = roomObject.transform.GetChild(1).gameObject.GetComponent<Tilemap>();
        instance.currentRoom.PopulateObstacles(obstacleTiles);
        instance.currentRoom.SetPlayerSpawn(enteredFrom);
        instance.currentRoom.enemySpawns.Clear();
        
        if (!currentRoom.visited)
        {
            if (currentRoom == lastRoom)
            {
                instance.currentRoom.SetBossSpawn();
            }
            else
            {
                instance.currentRoom.SetEnemySpawns(instance.currentFloor);
            }
        }
    }

    /// <summary>
    /// Generates the dungeon and add rooms to the rooms list
    /// </summary>
    /// <returns>The inital room</returns>
    private Room GenerateDungeon()
    {
        int gridSize = 3 * numberOfRooms;

        rooms = new Room[gridSize, gridSize];

        Vector2Int initialRoomCoordinate = new Vector2Int((gridSize / 2) - 1, (gridSize / 2) - 1);

        Queue<Room> roomsToCreate = new Queue<Room>();
        startRoom = new Room(initialRoomCoordinate);
        roomsToCreate.Enqueue(startRoom);
        List<Room> createdRooms = new List<Room>();

        while (roomsToCreate.Count > 0 && createdRooms.Count < numberOfRooms)
        {
            Room currentRoom = roomsToCreate.Dequeue();
            rooms[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y] = currentRoom;
            createdRooms.Add(currentRoom);
            AddNeighbours(currentRoom, roomsToCreate);
        }

        lastRoom = createdRooms.Last();

        foreach (Room room in createdRooms)
        {
            List<Vector2Int> neighbourCoordinates = room.NeighbourCoordinates();
            foreach (Vector2Int coordinate in neighbourCoordinates)
            {
                Room neighbour = rooms[coordinate.x, coordinate.y];
                if (neighbour != null)
                {
                    room.Connect(neighbour);
                }
            }
        }

        

        return rooms[initialRoomCoordinate.x, initialRoomCoordinate.y];
    }

    /// <summary>
    /// Method which adds the neighbours of a room and randomly adds one neighbour to the
    /// queue of rooms to be created.
    /// </summary>
    /// <param name="currentRoom"></param>
    /// <param name="roomsToCreate"></param>
    private void AddNeighbours(Room currentRoom, Queue<Room> roomsToCreate)
    {
        List<Vector2Int> neighbourCoordinates = currentRoom.NeighbourCoordinates();
        List<Vector2Int> availableNeighbours = new List<Vector2Int>();

        foreach (Vector2Int coordinate in neighbourCoordinates)
        {
            if (rooms[coordinate.x, coordinate.y] == null)
            {
                availableNeighbours.Add(coordinate);
            }
        }

        int numberOfNeighbours = Random.Range(1, availableNeighbours.Count);

        for (int neighbourIndex = 0; neighbourIndex < numberOfNeighbours; neighbourIndex++)
        {
            float randomNumber = Random.value;
            float roomFrac = 1.0f / availableNeighbours.Count; // NOTE TO SELF: superduper konstigt sätt att slumpa neighbour?
            Vector2Int chosenNeighbour = new Vector2Int(0, 0);
            foreach (Vector2Int coordinate in availableNeighbours)
            {
                if (randomNumber < roomFrac)
                {
                    chosenNeighbour = coordinate;
                }
                else
                {
                    roomFrac += 1.0f / availableNeighbours.Count;
                }
            }
            roomsToCreate.Enqueue(new Room(chosenNeighbour));
            availableNeighbours.Remove(chosenNeighbour);
        }
    }

    public void PrintGrid()
    {
        for (int rowIndex = 0; rowIndex < rooms.GetLength(1); rowIndex++)
        {
            string row = "";

            for (int columnIndex = 0; columnIndex < rooms.GetLength(0); columnIndex++)
            {
                if (rooms[columnIndex, rowIndex] == null)
                {
                    row += "X";
                }
                else
                {
                    row += "R";
                }
            }
            Debug.Log(row);
        }
    }

}
