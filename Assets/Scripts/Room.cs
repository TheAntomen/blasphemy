using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Vector2Int roomCoordinate;
    public Dictionary<string, Room> neighbours;

    public Room(int xCoordinate, int yCoordinate)
    {
        roomCoordinate = new Vector2Int(xCoordinate, yCoordinate);
        neighbours = new Dictionary<string, Room>();
    }

    public Room(Vector2Int _roomCoordinate)
    {
        roomCoordinate = _roomCoordinate;
        neighbours = new Dictionary<string, Room>();
    }

    /// <summary>
    /// Method for instantiating the positions of neighbours of the current room
    /// </summary>
    /// <returns>A list of coordinates of neighbours</returns>
    public List<Vector2Int> NeighbourCoordinates ()
    {
        List<Vector2Int> neighbourdCoordinates = new List<Vector2Int>();
        neighbourdCoordinates.Add(new Vector2Int(roomCoordinate.x, roomCoordinate.y - 1));
        neighbourdCoordinates.Add(new Vector2Int(roomCoordinate.x + 1, roomCoordinate.y));
        neighbourdCoordinates.Add(new Vector2Int(roomCoordinate.x, roomCoordinate.y + 1));
        neighbourdCoordinates.Add(new Vector2Int(roomCoordinate.x - 1, roomCoordinate.y));

        return neighbourdCoordinates;
    }

    public void Connect(Room neighbour)
    {
        string direction = "";

        if (neighbour.roomCoordinate.y < roomCoordinate.y)
        {
            direction = "N";
        }
        if (neighbour.roomCoordinate.y > roomCoordinate.y)
        {
            direction = "S";
        }
        if (neighbour.roomCoordinate.x < roomCoordinate.x)
        {
            direction = "W";
        }
        if (neighbour.roomCoordinate.x < roomCoordinate.x)
        {
            direction = "E";

        }
        neighbours.Add(direction, neighbour);
    }
}
