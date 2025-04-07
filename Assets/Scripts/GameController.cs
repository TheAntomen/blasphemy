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
    // Properties
    public List<Enemy> Enemies { get; set; } = new List<Enemy>();

    // Private variables
    private DungeonGenerator dungeon;
    private SpawnManager spawnManager;
    private GameObject knight;
    private Enemy boss;
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
            spawnManager.Init(dungeon.CurrentRoom, dungeon.CurrentFloor);

            // Spawn player
            instance.knight = spawnManager.SpawnPlayer();

            // Spawn enemies
            if (spawnEnemies)
            {
                instance.Enemies = spawnManager.SpawnEnemies();
            }
            // Spawn boss
            if (dungeon.CurrentRoom == dungeon.lastRoom)
            {
                instance.boss = spawnManager.SpawnBoss();
                instance.Enemies.Add(instance.boss);
            }

            dungeon.CurrentRoom.visited = true;

            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {        
        dungeon = DungeonGenerator.instance;  // Reference to dungeon
        dungeon.UpdateDungeon();

        spawnManager = GetComponent<SpawnManager>();
        spawnManager.Init(dungeon.CurrentRoom, dungeon.CurrentFloor);

        // Spawn player
        knight = spawnManager.SpawnPlayer();

        dungeon.CurrentRoom.visited = true;
    }

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            FloorComplete();
        }
        if (Input.GetButtonDown("Jump"))
        {
            FloorComplete();
        }
    }

    public void FloorComplete()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Menu");
    }

}



