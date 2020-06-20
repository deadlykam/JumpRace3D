using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUISpeedEffect : BasicUIEffect
{
    [Header("Basic UI Speed Effect Properties")]
    [SerializeField]
    [Tooltip("The speed of the effect")]
    private float _speed; // The speed of the effect

    /// <summary>
    /// Get/sets the speed value, of type float
    /// </summary>
    protected float speed {
        get { return _speed; }
        set { _speed = value; }
    }

    private float _step; // Storing transition value

    /// <summary>
    /// Get/set the step value, of type float
    /// </summary>
    protected float step
    {
        get { return _step; }
        set { _step = value; }
    }

    /// <summary>
    /// This method updates the BasicUISpeedEffect
    /// </summary>
    protected void UpdateBasicUISpeedEffect()
    {
        UpdateBasicUIEffect(); // Updating the BasicUIEffect

        // Calculating the step value
        _step = (_step + (_speed * fps)) > 1 ? 1 :
                (_step + (_speed * fps)) < 0 ? 0 :
                (_step + (_speed * fps));

        // Fixing the speed direction
        _speed = _step == 1 ? -_speed :
                 _step == 0 ? -_speed :
                 _speed;
    }
}
