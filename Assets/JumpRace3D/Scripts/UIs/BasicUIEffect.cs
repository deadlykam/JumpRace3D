using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUIEffect : MonoBehaviour
{
    private float _fps; // For storing the fps value

    /// <summary>
    /// Returns the fps value, of type float
    /// </summary>
    protected float fps { get { return _fps; } }

    /// <summary>
    /// The update method for BasicUIEffect.
    /// </summary>
    protected void UpdateBasicUIEffect() { _fps = Time.deltaTime; }
}
