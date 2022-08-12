using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface to force implemenation of needed methods and properties to handle damage in the game.
/// </summary>
public interface IDamageable
{
    public int CurrentHealth { get; set; }

    public void ChangeHealth(int amount);

    public IEnumerator DamageFlash();
}
