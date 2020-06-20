using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUI : BasicUIEffect
{
    [Header("Toggle UI Properties")]
    [SerializeField]
    private GameObject _ui; // The UI element to toggle

    [SerializeField]
    private float _timerToggle; // The maximum time for
                                // toggling UI

    private float _timer; // Current time

    // Update is called once per frame
    void Update()
    {
        UpdateBasicUIEffect(); // Calling the update of
                               // BasicUIEffect

        // Calculating the timer for toggling
        _timer = _timer - fps <= 0 ? _timerToggle : 
                                      _timer - fps;

        // Condition for toggling the UI element
        if (_timer == _timerToggle) _ui.SetActive(!_ui.activeSelf);
    }
}
