using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    public const int DAMAGE = 10;

    Rigidbody2D rb2d;
    Animator animator;
    ArrayList hitEnemies;
    

    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        hitEnemies = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("attacking");
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
