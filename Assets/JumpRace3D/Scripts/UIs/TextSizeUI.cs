using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSizeUI : BasicUISpeedEffect
{
    [Header("Text Size UI Properties")]
    [SerializeField]
    private TextMeshProUGUI _text; // Text UI to change size of

    /// <summary>
    /// Returns the Text, of type TextMeshProUGUI
    /// </summary>
    protected TextMeshProUGUI text { get { return _text; } }

    [SerializeField]
    [Tooltip("The minimum size of the text in percentage.")]
    [Range(0, 1)]
    private float _sizePercentage; // The minimum size of the text
                                   // in percent

    private float _sizeMax; // Maximum text size

    /// <summary>
    /// The maximum size of the text font, of type float
    /// </summary>
    protected float sizeMax { get { return _sizeMax; } }

    private float _sizeMin; // Minimum text size

    /// <summary>
    /// The minimum size of the text font, of type float
    /// </summary>
    protected float sizeMin { get { return _sizeMin; } }

    // Start is called before the first frame update
    void Start()
    {
        _sizeMax = _text.fontSize; // Storing the max font size
        _sizeMin = _sizeMax * _sizePercentage; // Storing the min
                                               // font size
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBasicUISpeedEffect(); // Updating BasicUISpeedEffect

        // Changing the size of the font
        _text.fontSizeMax = Mathf.Lerp(_sizeMin, _sizeMax, step);
    }
}
