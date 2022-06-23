using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{

    public const int TOTAL_WAVES = 8;

    public int currentWave;
    public int difficulty;

    public SpawnManager spawnManager;
    public Canvas ui;


    private const float pauseDuration = 3.0f;

    private float pauseCounter;

    private GameObject[] enemies;
    private GameObject knight;
    private TextMeshProUGUI wavetext;
    

    // Start is called before the first frame update
    void Start()
    {
        difficulty = StateNameController.difficulty;
        currentWave = 1;
        pauseCounter = 0;

        wavetext = ui.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

        knight = GameObject.FindGameObjectWithTag("Player");

        if (knight == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
        }

        if (!spawnManager.Spawning)
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            
            if (enemies.Length == 0)
            {
                pauseCounter += Time.deltaTime;
                wavetext.SetText(currentWave + " / " + "8");
                if (currentWave > TOTAL_WAVES)
                {
                    StateNameController.completedNormal = true;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
                }
                else if (pauseCounter >= pauseDuration)
                {
                    spawnManager.Init(currentWave);
                    currentWave++;
                    pauseCounter = 0;
                }
            }
        }

    }

}



