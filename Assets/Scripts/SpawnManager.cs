using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class for handling the logic for spawning enemies
/// </summary>
public class SpawnManager : MonoBehaviour
{
    // Properties
    public bool Spawning { get; private set; }

    // Public variables
    public GameObject knight;


    // Private variables
    private FloorManager floor;
    private List<Vector3> enemySpawns;
    private Vector3 playerSpawn;
    private Vector3 bossSpawn;
   
    public void Init(Room currentRoom, FloorManager _floor)
    {
        floor = _floor;

        Spawning = false;

        enemySpawns = currentRoom.enemySpawns;
        playerSpawn = currentRoom.playerSpawn;
        bossSpawn = currentRoom.bossSpawn;
    }

    /// <summary>
    /// Method for spawning Enemies. Randomly decides which enemy to spawn and may, if availabe, spawn a rare variant of the enemy type.
    /// </summary>
    public List<Enemy> SpawnEnemies()
    {
        List<Enemy> enemies = new List<Enemy>();

        Spawning = true;

        foreach(Vector3 spawn in enemySpawns)
        {
            int enemyIndex = Random.Range(0, floor.enemyTypes.Length);
            Enemy enemyType = floor.enemyTypes[enemyIndex];

            bool rarespawn = Random.value <= floor.rareSpawnChance;

            if (rarespawn && enemyType.rareVariants != null)
            {
                int rareTypIndex = Random.Range(0, enemyType.rareVariants.Length);
                Enemy rareEnemy = enemyType.rareVariants[rareTypIndex];
                enemies.Add(Instantiate(rareEnemy, spawn, Quaternion.identity));
            }
            else
            {
                enemies.Add(Instantiate(enemyType, spawn, Quaternion.identity));
            }
        }
        return enemies;
    }

    public GameObject SpawnPlayer()
    {
        return Instantiate(knight, playerSpawn, Quaternion.identity);
    }

    public Enemy SpawnBoss()
    {
        return Instantiate(floor.boss, bossSpawn, Quaternion.identity);
    }
}
