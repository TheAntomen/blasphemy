using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Handles the overall pace and state of the game, such as spawning enemies and
/// keeping track on win condition
/// </summary>
public class GameController : MonoBehaviour
{
    // Public variables
    public List<GameObject> enemies;
    public int currentWave;
    public int difficulty;

    // Private variables
    private DungeonGenerator dungeon;
    private SpawnManager spawnManager;
    private GameObject knight;
    private GameObject boss;
    private static GameController instance;

    [SerializeField]
    private bool spawnEnemies = true;


    // Awake is called when the script instance is being loaded
    private void Awake()
    {

        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            GameInfo.difficulty = 1;
            instance = this;           
        }else
        {
            dungeon = DungeonGenerator.instance;  // Reference to dungeon
            dungeon.UpdateDungeon();

            spawnManager = GetComponent<SpawnManager>();
            spawnManager.Init(dungeon.currentRoom, 1);

            // Spawn player
            instance.knight = spawnManager.SpawnPlayer();

            // Spawn enemies
            if (spawnEnemies)
            {
                instance.enemies = spawnManager.SpawnEnemies();
            }
            // Spawn boss
            if (dungeon.currentRoom == dungeon.lastRoom)
            {
                instance.boss = spawnManager.SpawnBoss();
            }

            dungeon.currentRoom.visited = true;

            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {        
        dungeon = DungeonGenerator.instance;  // Reference to dungeon
        dungeon.UpdateDungeon();

        spawnManager = GetComponent<SpawnManager>();
        spawnManager.Init(dungeon.currentRoom, 1);

        // Spawn player
        knight = spawnManager.SpawnPlayer();

        dungeon.currentRoom.visited = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (dungeon.currentRoom == dungeon.lastRoom)
        {

        }
    }


    public void FloorComplete()
    {
        SceneManager.LoadScene("Menu");
    }

}



