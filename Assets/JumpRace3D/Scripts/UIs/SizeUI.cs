using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeUI : BasicUISpeedEffect
{
    [Header("Size UI Properties")]
    [SerializeField]
    private RectTransform _rectOriginal; // Original rect transform
                                         // of the UI element

    [SerializeField]
    [Tooltip("The minimum size of the text in percentage.")]
    [Range(0, 1)]
    private float _sizePercentage; // The minimum size of the UI
                                   // element in percent
                                   
    private Vector2 _maxSize = Vector2.zero; // Maximum size of
                                             // the UI element

    private Vector2 _minSize = Vector2.zero; // Minimum size of
                                             // the UI element

    // Storing calculated size of the UI element
    private Vector2 _currentSize = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        // Setting the maximum size of the UI Element
        _maxSize = _rectOriginal.sizeDelta;

        // Calculating the minimum size of the UI Element
        _minSize = _maxSize * _sizePercentage;
    }

    // Update is called once per frame
    void Update()
    {
        if (step != 1) // Condition to change size
        {
            UpdateBasicUISpeedEffect(); // Calling BasicUISpeedEffect Update

            // Changing the size of the UI
            _rectOriginal.sizeDelta = Vector2.Lerp(_minSize, _maxSize, step);
        }
    }
}
