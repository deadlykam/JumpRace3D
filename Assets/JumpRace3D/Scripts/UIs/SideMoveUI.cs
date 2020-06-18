using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMoveUI : BasicUISpeedEffect
{
    [Header("Side Move UI Properties")]
    [SerializeField]
    private Transform _leftTarget; // Left target

    /// <summary>
    /// Returns the left target, of type Transform
    /// </summary>
    protected Transform leftTarget { get { return _leftTarget; } }

    [SerializeField]
    private Transform _rightTarget; // Right target

    /// <summary>
    /// Returns the right target, of type Transform
    /// </summary>
    protected Transform rightTarget { get { return _rightTarget; } }

    // Update is called once per frame
    void Update()
    {

        UpdateBasicUISpeedEffect(); // Updating BasicUISpeedEffect

        // Moving the UI
        transform.position = Vector3.Lerp(_leftTarget.position, 
                                          _rightTarget.position, 
                                          step);
    }
}
