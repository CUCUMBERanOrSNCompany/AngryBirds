using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

/// <summary>
/// Base class for enemies
/// </summary>
[RequireComponent(typeof(SFXManager))]
public abstract class EnemiesBase : MonoBehaviour, IDisposable
{
    /// <summary>
    /// Implementing IDisposable
    /// </summary>
    public abstract void Dispose();

    /// <summary>
    /// Reactive property that reports the current state of the pig
    /// </summary>
    public ReactiveProperty<bool> IsAlive { get; private set; }
        = new ReactiveProperty<bool>(true);

    /// <summary>
    /// Reference to the SFX Manager component
    /// </summary>
    private SFXManager _sfxManager = null;

    /// <summary>
    /// Time since last snore
    /// </summary>
    private float _time = 0;

    /// <summary>
    /// Adding references
    /// </summary>
    private void Start()
    {
        _sfxManager = GetComponent<SFXManager>();
    }

    /// <summary>
    /// Running the snoring logic
    /// </summary>
    private void Update()
    {
        if (!IsAlive.Value) return;

        _time += Time.deltaTime;

        if(_time > 5)
        {
            _time = 0;
            Snore();
        }
    }

    /// <summary>
    /// Playing Snoring sound
    /// </summary>
    private void Snore()
    {
        _sfxManager.PlaySFXNoOverride(SoundsEnum.Snore);
    }

}
