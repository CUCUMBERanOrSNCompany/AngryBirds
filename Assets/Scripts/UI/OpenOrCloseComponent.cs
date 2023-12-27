
using UnityEngine;

/// <summary>
/// Responsible for opening and closing UI components
/// </summary>
public class OpenOrCloseComponent : MonoBehaviour
{
    /// <summary>
    /// Indicates if we want the button to open or to close the component.
    /// </summary>
    [SerializeField] private bool _toOpen = false;

    /// <summary>
    /// Indicates if the button is acting as a switch that
    /// open/close the component in turns.
    /// </summary>
    [SerializeField] private bool _isSwitchBtn = false;

    /// <summary>
    /// Reference to the UI component
    /// </summary>
    [SerializeField] private GameObject _uiComponent = null;

    /// <summary>
    /// Responsible for the logic of the class.
    /// </summary>
    public void OpenCloseComponent()
    {
        if (_uiComponent)
        {
            _uiComponent.SetActive(_toOpen);
        }

        if(_isSwitchBtn)
        {
            _toOpen = !_toOpen;
        }
    }


}
