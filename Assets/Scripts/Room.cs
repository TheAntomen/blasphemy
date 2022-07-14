using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Class containing coordinate and neighbours of a room
/// TODO: Ändra koordinatsystem för att slippa addera och subtrahera ROOM_WIDTH och ROOM_HEIGHT????
/// </summary>
public class Room
{
    // Public variables
    public bool visited;
    public Vector2Int roomCoordinate;
    public Vector3 playerSpawn;
    public Vector3 bossSpawn;
    public List<Vector3> enemySpawns;
    public Dictionary<string, Room> neighbours;

    // Private variables
    private string[,] population;
    private const int ROOM_WIDTH = 20;
    private const int ROOM_HEIGHT = 16;
    private const int SPAWN_DIST = 5;

    /// <summary>
    /// Constructor for this class
    /// </summary>
    /// <param name="_roomCoordinate"></param>
    public Room(Vector2Int _roomCoordinate)
    {
        roomCoordinate = _roomCoordinate;
        neighbours = new Dictionary<string, Room>();
        population = new String[ROOM_WIDTH, ROOM_HEIGHT];
        enemySpawns = new List<Vector3>();

        visited = false;
    }

    /// <summary>
    /// Method for instantiating the positions of neighbours of the current room
    /// </summary>
    /// <returns>A list of coordinates of neighbours</returns>
    public List<Vector2Int> NeighbourCoordinates()
    {
        List<Vector2Int> neighbourdCoordinates = new List<Vector2Int>();
        neighbourdCoordinates.Add(new Vector2Int(roomCoordinate.x, roomCoordinate.y - 1));
        neighbourdCoordinates.Add(new Vector2Int(roomCoordinate.x + 1, roomCoordinate.y));
        neighbourdCoordinates.Add(new Vector2Int(roomCoordinate.x, roomCoordinate.y + 1));
        neighbourdCoordinates.Add(new Vector2Int(roomCoordinate.x - 1, roomCoordinate.y));

        return neighbourdCoordinates;
    }

    /// <summary>
    /// Method that populates the list "population" based on obstacles read from tilemap
    /// </summary>
    /// <param name="obstacleTiles"></param>
    public void PopulateObstacles(Tilemap obstacleTiles)
    {
        BoundsInt bounds = obstacleTiles.cellBounds;
        for (int yCoord = 0; yCoord < ROOM_HEIGHT; yCoord++)
        {
            for (int xCoord = 0; xCoord < ROOM_WIDTH; xCoord++)
            {

                Vector3Int cellPos = new Vector3Int(xCoord + bounds.xMin, bounds.yMax - (yCoord + 1), 0);
                string cellValue = "";

                if (obstacleTiles.HasTile(cellPos))
                {
                    cellValue = "Obstacle";
                }

                population[xCoord, yCoord] = cellValue;
            }
        }
    }

    /// <summary>
    /// Method for setting the players spawn in a new room based on the direction the player entered from
    /// </summary>
    /// <param name="enteredDirection"></param>
    public void SetPlayerSpawn(string enteredDirection)
    {
        Vector3Int position = new Vector3Int();

        switch (enteredDirection)
        {
            case "N":
                position.x = (int)Mathf.Floor(ROOM_WIDTH / 2);
                position.y = 3;
                break;
            case "E":
                position.x = 2;
                position.y = (int)Mathf.Floor(ROOM_HEIGHT / 2) + 1;
                break;
            case "S":
                position.x = (int)Mathf.Floor(ROOM_WIDTH / 2);
                position.y = ROOM_HEIGHT - 2;
                break;
            case "W":
                position.x = ROOM_WIDTH - 2;
                position.y = (int)Mathf.Floor(ROOM_HEIGHT / 2) + 1;

                break;
            default:
                position.x = ROOM_WIDTH / 2;
                position.y = ROOM_HEIGHT / 2;
                break;
        }

        population[position.x, position.y] = "Player";
        playerSpawn.x = position.x - (ROOM_WIDTH / 2);
        playerSpawn.y = position.y - (ROOM_HEIGHT / 2);
    }

    /// <summary>
    /// Method for setting enemy spawns where a spawning point is allowed
    /// </summary>
    /// <param name="currentFloor"></param>
    public void SetEnemySpawns(int currentFloor)
    {
        // Determine how many spawns for the current floor (May modify this in later stage)
        int spawnerCount = 1;

        for (int spawnIndex = 0; spawnIndex < spawnerCount; spawnIndex++)
        {
            // Find a free tile for an Enemy
            while (true)
            {
                List<Vector2Int> region = FindFreeRegion(new Vector2Int(1, 1));
                Vector2 regionWorldCoord = new Vector2(region[0].x - (ROOM_WIDTH / 2), region[0].y - (ROOM_HEIGHT / 2));
                float distance = Vector2.Distance(regionWorldCoord, new Vector2(playerSpawn.x, playerSpawn.y));

                // Check so that enemies do not spawn too close the player
                if (distance > SPAWN_DIST)
                {
                    population[region[0].x, region[0].y] = "Enemy";
                    enemySpawns.Add(regionWorldCoord);
                    break;
                }
            }
        }
    }

    public void SetBossSpawn()
    {
        population[ROOM_WIDTH / 2, ROOM_HEIGHT / 2] = "Boss";
        bossSpawn = new Vector3(ROOM_WIDTH / 2, ROOM_HEIGHT / 2);
    }


    /// <summary>
    /// Connects a neighbour with direction to the room
    /// </summary>
    /// <param name="neighbour"></param>
    public void Connect(Room neighbour)
    {
        string direction = "";

        if (neighbour.roomCoordinate.y < roomCoordinate.y)
        {
            direction = "N";
        }
        if (neighbour.roomCoordinate.x > roomCoordinate.x)
        {
            direction = "E";
        }
        if (neighbour.roomCoordinate.y > roomCoordinate.y)
        {
            direction = "S";
        }
        if (neighbour.roomCoordinate.x < roomCoordinate.x)
        {
            direction = "W";

        }
        neighbours.Add(direction, neighbour);
    }

    /// <summary>
    /// Returns the prefab name of this room
    /// </summary>
    /// <returns>Name of prefab</returns>
    public string PrefabName()
    {
        string name = "Room_";
        foreach (KeyValuePair<string, Room> neighbourPair in neighbours)
        {
            name += neighbourPair.Key;
        }
        return name;
    }

    /// <summary>
    /// Simple method for returning the neighbour matching the direction
    /// </summary>
    /// <param name="direction"></param>
    /// <returns>The Room of neighbour</returns>
    public Room GetNeighbour(string direction)
    {
        return neighbours[direction];
    }

    /// <summary>
    /// Finds a free region inside of the room based on desired size
    /// </summary>
    /// <param name="sizeInTiles"></param>
    /// <returns></returns>
    private List<Vector2Int> FindFreeRegion(Vector2Int sizeInTiles)
    {
        List<Vector2Int> region = new List<Vector2Int>();

        do
        {
            region.Clear();

            // Choose a random tile in the room
            Vector2Int centerTile = new Vector2Int(UnityEngine.Random.Range(1, ROOM_WIDTH - 3), UnityEngine.Random.Range(2, ROOM_HEIGHT - 3));

            int initialXCoordinate = (centerTile.x - (int)MathF.Floor(sizeInTiles.x / 2));
            int initialYCoordinate = (centerTile.y - (int)MathF.Floor(sizeInTiles.y / 2));

            for (int xCoord = initialXCoordinate; xCoord < initialXCoordinate + sizeInTiles.x; xCoord++)
            {
                for (int yCoord = initialYCoordinate; yCoord < initialYCoordinate + sizeInTiles.y; yCoord++)
                {
                    region.Add(new Vector2Int(xCoord, yCoord));
                }
            }
        } while (!IsFree(region));

        return region;
    }

    /// <summary>
    /// Checks if a region is free from other tiles
    /// </summary>
    /// <param name="region"></param>
    /// <returns>True if no obstacle in region</returns>
    private bool IsFree(List<Vector2Int> region)
    {
        foreach (Vector2Int tile in region)
        {
            if (population[tile.x, tile.y] != "")
            {
                return false;
            }
        }
        return true;
    }

    private void PrintPopulation()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < population.GetLength(1); i++)
        {
            for (int j = 0; j < population.GetLength(0); j++)
            {
                sb.Append(population[j, i]);
                sb.Append(' ');
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());
    }

}
