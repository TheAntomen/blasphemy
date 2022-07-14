using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for handling the logic for spawning enemies
/// </summary>
public class SpawnManager : MonoBehaviour
{
    // Properties
    public bool Spawning { get; private set; }

    // Public variables
    public GameObject regularGhoul;
    public GameObject fastGhoul;
    public GameObject strongGhoul;
    public GameObject floorBoss;
    public GameObject knight;

    // Private variables
    private const int START_MONEY = 10;
    private int currentMoney;
    private int count;
    private List<Vector3> enemySpawns;
    private Vector3 playerSpawn;
    private Vector3 bossSpawn;
    private GameObject[] enemyList;
   
    public void Init(Room currentRoom, int floor)
    {
        currentMoney = START_MONEY + floor*2;
        Spawning = false;

        enemyList = new GameObject[] {regularGhoul, fastGhoul, strongGhoul};

        enemySpawns = currentRoom.enemySpawns;
        playerSpawn = currentRoom.playerSpawn;
        bossSpawn = currentRoom.bossSpawn;

        count = enemySpawns.Count;
    }

    /// <summary>
    /// Method for spawning enemies while accounting for the current wallet and cost of spawning individual enemies
    /// </summary>
    public List<GameObject> SpawnEnemies()
    {
        List<GameObject> enemies = new List<GameObject>();

        Spawning = true;

        foreach(Vector3 spawn in enemySpawns)
        {
            int enemyIndex = Random.Range(0, enemyList.Length);
            enemies.Add(Instantiate(enemyList[enemyIndex], spawn, Quaternion.identity));
        }

        return enemies;
    }

    public GameObject SpawnPlayer()
    {
        return Instantiate(knight, playerSpawn, Quaternion.identity);
    }

    public GameObject SpawnBoss()
    {
        return Instantiate(floorBoss, bossSpawn, Quaternion.identity);
    }
}
