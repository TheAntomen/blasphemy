using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public bool Spawning { get; private set; }

    public GameObject regularGhoul;
    public GameObject fastGhoul;
    public GameObject strongGhoul;
    public GameController controller;

    public int costRegularGhoul = 1;
    public int costFastGhoul = 2;
    public int costStrongGhoul = 3;

    

    private const int START_MONEY = 10;
    private int currentMoney;

    private int spawnIndex;
    private int enemyIndex;
    private int count;

    
    //private int enemyCounter;
    
    

    private Transform[] spawnpoints;
    private GameObject[] enemyList;
    



    // Start is called before the first frame update
    public void Init(int wave)
    {
        count = transform.childCount;
        currentMoney = START_MONEY + wave*2;
        //enemyCounter = 0;
        Spawning = false;

        enemyList = new GameObject[] { regularGhoul, fastGhoul, strongGhoul };

        spawnpoints = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            spawnpoints[i] = transform.GetChild(i);
        }

        InvokeRepeating("SpawnEnemies", 0.0f, 1.0f);
    }

    void SpawnEnemies()
    {
        spawnIndex = Random.Range(0, count);
        enemyIndex = Random.Range(0, enemyList.Length);

        Spawning = true;

        if (currentMoney <= 0)
        {
            Spawning = false;
            CancelInvoke();
            //controller.EnemyCounter = enemyCounter;

        }
        else
        {
            if (enemyIndex == 2 && currentMoney >= 3)
            {
                currentMoney -= costStrongGhoul;
                Instantiate(strongGhoul, spawnpoints[spawnIndex].position, strongGhoul.transform.rotation);
            }
            else
            {
                enemyIndex = Random.Range(0, enemyList.Length - 1);
            }

            if (enemyIndex == 1 && currentMoney >= 2)
            {
                currentMoney -= costFastGhoul;
                Instantiate(fastGhoul, spawnpoints[spawnIndex].position, fastGhoul.transform.rotation);
            }
            else
            {
                currentMoney -= costRegularGhoul;
                Instantiate(regularGhoul, spawnpoints[spawnIndex].position, regularGhoul.transform.rotation);
            }
            //enemyCounter += 1;
        }
    }
}
