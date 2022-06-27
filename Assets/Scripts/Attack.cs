using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for handling damage and collision of attacks
/// </summary>
public class Attack : MonoBehaviour
{
    // Public variables
    public const int DAMAGE = 10;

    // Private variables
    private ArrayList hitEnemies;
    
    // Start is called before the first frame update
    void Awake()
    {
        hitEnemies = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Ghoul enemy = other.gameObject.GetComponent<Ghoul>();

        if (enemy != null && !hitEnemies.Contains(enemy))
        {
            enemy.ChangeHealth(-DAMAGE);
            hitEnemies.Add(enemy);
        }
    }

}
