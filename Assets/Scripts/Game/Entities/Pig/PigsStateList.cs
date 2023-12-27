using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holding a map between the health state and the corresponding sprite
/// </summary>
public class PigsStateList : MonoBehaviour
{
    /// <summary>
    /// List of sprites
    /// </summary>
    [SerializeField] private List<Sprite> _sprites = null;

    /// <summary>
    /// Dictionary that maps health state to a sprite
    /// </summary>
    public Dictionary<HealthEnum, Sprite> PigsStates { get; private set; }
        = new Dictionary<HealthEnum, Sprite>();

    /// <summary>
    /// Singleton
    /// </summary>
    public static PigsStateList Instance { get; private set; }

    /// <summary>
    /// Instentiating the dictionary.
    /// </summary>
    private void Start()
    {
        Instance = this;

        if (_sprites.Count != 2)
        {
            throw new Exception($"_sprites expect EXACTLY two sprites. " +
                $"You provided: {_sprites.Count}");
        }

        PigsStates[HealthEnum.Healthy] = _sprites[0];

        PigsStates[HealthEnum.Injured] = _sprites[1];
    }
}
