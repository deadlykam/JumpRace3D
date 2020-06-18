using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeShadow : BasicLineDetector
{
    [Header("Fake Shadow Properties")]
    [SerializeField]
    private Transform _shadow; // The fake shadow model

    [SerializeField]
    private Vector3 _offset; // The position offset
                             // of the shadow

    [SerializeField]
    private Vector3 _offsetEnd; // The position offset 
                                // for end stage

    private bool _isEnd; // Flag that checks if it is
                         // the end stage

    // Update is called once per frame
    void Update()
    {
        UpdateBasicLineDetector(); // Updating BasicLineDetector

        // Condition for updating the fake shadow position
        if (isHitCollider)
        {
            // Condition to set the end position offset
            if (_isEnd) _shadow.position = (hitPoint + _offsetEnd);
            // Condition for normal offset
            else _shadow.position = (hitPoint + _offset);
        }
    }

    /// <summary>
    /// This method sets the flag for the end position offset.
    /// </summary>
    /// <param name="isEnd">Flag to set the end position offset,
    ///                     of type bool</param>
    public void SetEnd(bool isEnd) => _isEnd = isEnd;

    /// <summary>
    /// This method shows/hides the shadow.
    /// </summary>
    /// <param name="active">Flag to show/hide shadow,
    ///                      of type bool</param>
    public void SetFakeShadow(bool active)
    {
        // Showing/Hiding shadow
        gameObject.SetActive(active);

        // Condition to reset the end offset
        if (active) _isEnd = false;
    }
}
