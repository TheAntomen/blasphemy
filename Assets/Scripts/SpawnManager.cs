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
    public int costRegularGhoul = 1;
    public int costFastGhoul = 2;
    public int costStrongGhoul = 3;
    public GameObject regularGhoul;
    public GameObject fastGhoul;
    public GameObject strongGhoul;
    public GameObject knight;

    // Private variables
    private const int START_MONEY = 10;
    private int currentMoney;
    private int count;
    private List<Vector3> enemySpawns;
    private Vector3 playerSpawn;
    private GameObject[] enemyList;
   
    public void Init(Room currentRoom, int floor)
    {
        currentMoney = START_MONEY + floor*2;
        Spawning = false;

        enemyList = new GameObject[] {regularGhoul, fastGhoul, strongGhoul};

        enemySpawns = currentRoom.enemySpawns;
        playerSpawn = currentRoom.playerSpawn;
        
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
}
