using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class for changing between scenes
/// </summary>
public class EnterDoor : MonoBehaviour
{
    [SerializeField]
    string direction;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameController controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            
            if (controller.enemies.Count <= 0)
            {
                GameObject dungeon = controller.transform.GetChild(0).gameObject;
                DungeonGenerator dungeonGenerator = dungeon.GetComponent<DungeonGenerator>();
                Room room = dungeonGenerator.currentRoom;
                dungeonGenerator.currentRoom = room.GetNeighbour(direction); // Change room to load
                dungeonGenerator.enteredFrom = direction;   // Tell the door direction
                SceneManager.LoadScene("TestScene");
            }
        }
    }

}
