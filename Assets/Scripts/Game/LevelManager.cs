using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the enemies in the scene.
    /// </summary>
    [SerializeField] private List<GameObject> _pigs = null;

    /// <summary>
    /// Reference to the Open/Close UI Class
    /// </summary>
    [SerializeField] private OpenOrCloseComponent _endScreen = null;

    /// <summary>
    /// Turning the list of enemies to a HashSet so no enemy can be removed more than once.
    /// </summary>
    private HashSet<EnemiesBase> _enemies = new HashSet<EnemiesBase>();

    /// <summary>
    /// Let us to dispose unused listeners.
    /// </summary>
    protected CompositeDisposable _disposables = new CompositeDisposable();

    /// <summary>
    /// Creating the references
    /// </summary>
    void Start()
    {
        foreach(GameObject enemy in _pigs)
        {
            _enemies.Add(enemy.GetComponent<EnemiesBase>());
        }

        ListenerSetter();
    }

    /// <summary>
    /// Setting the listeners to the defeat of the enemies
    /// </summary>
    private void ListenerSetter()
    {
        foreach(var enemy in _enemies)
        {
            enemy.IsAlive
            .Subscribe(isAlive =>
            {
                if(!isAlive)
                {
                    _enemies.Remove(enemy);
                    //Debug.Log($"{_enemies.Count}");

                    if(_enemies.Count <= 0)
                    {
                        EndLevel();
                    }
                }
            }).AddTo(_disposables);
        }
    }

    /// <summary>
    /// Ending Level logic
    /// </summary>
    private void EndLevel()
    {
        //Debug.Log($"Yay!");
        _endScreen.OpenCloseComponent();
    }

    
}
