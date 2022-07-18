using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    protected int health;
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected float range;
    [SerializeField]
    public float speed;

    protected bool damageTaken;

    public Vector2 direction;

    private Color damageColor = new Color(1.000f, 0f, 0f, 1.000f);
    private int damageFlashCount = 6;

    protected abstract void Update();

    protected virtual void Attack()
    {
        Debug.Log("Attack from abstract class");
    }

    protected IEnumerator DamageFlash()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color defaultColor = renderer.color;
        
        for (int state = 0; state < damageFlashCount; state++)
        {
            if ((state % 2) == 0)
            {
                renderer.color = damageColor;
            }else
            {
                renderer.color = defaultColor;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }
}
