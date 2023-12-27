using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

/// <summary>
/// Responsible for the pig logic
/// </summary>
public class Pig : EnemiesBase
{
    /// <summary>
    /// Counting the number of times the pig collided
    /// </summary>
    protected int _collisionCounter = 0;

    /// <summary>
    /// Triggers whenever a collision detected
    /// </summary>
    /// <param name="collision">object we collided with</param>
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(IsAlive.Value)
        {
            _collisionCounter += 1;

            // It is possible for the pig not to hit the ground,
            // and we still need to classify the hit
            if ((collision.gameObject.tag == "Ground")
                || (_collisionCounter > 2)) Dispose();
        }
    }

    /// <summary>
    /// Killing the object
    /// </summary>
    public override void Dispose()
    {
        IsAlive.Value = false;

        gameObject.GetComponent<SpriteRenderer>().sprite
            = PigsStateList.Instance.PigsStates[HealthEnum.Injured];

        gameObject.GetComponent<PolygonCollider2D>().enabled = false;

        gameObject.GetComponent<Collider2D>().enabled = false;
    }
    
}
