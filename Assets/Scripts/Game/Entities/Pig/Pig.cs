using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

/// <summary>
/// Responsible for the pig logic
/// </summary>
public class Pig : MonoBehaviour, IDisposable
{
    /// <summary>
    /// Counting the number of times the pig collided
    /// </summary>
    private int _collisionCounter = 0;

    /// <summary>
    /// Reactive property that reports the current state of the pig
    /// </summary>
    public ReactiveProperty<bool> IsAlive { get; private set; }
        = new ReactiveProperty<bool>(true);

    /// <summary>
    /// Triggers whenever a collision detected
    /// </summary>
    /// <param name="collision">object we collided with</param>
    private void OnCollisionEnter2D(Collision2D collision)
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
    public void Dispose()
    {
        IsAlive.Value = false;

        gameObject.GetComponent<SpriteRenderer>().sprite
            = PigsStateList.Instance.PigsStates[HealthEnum.Injured];

        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
    }
    
}
