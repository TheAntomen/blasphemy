using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField]
    public int enemyCount;
    [SerializeField]
    public float rareSpawnChance;
    [SerializeField]
    public Enemy[] enemyTypes;
    [SerializeField]
    public Enemy boss;

}
