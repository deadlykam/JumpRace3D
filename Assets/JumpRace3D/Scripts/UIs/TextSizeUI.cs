using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSizeUI : BasicUISpeedEffect
{
    [Header("Text Size UI Properties")]
    [SerializeField]
    private TextMeshProUGUI Text; // Text UI to change size of

    [SerializeField]
    [Tooltip("The minimum size of the text in percentage.")]
    [Range(0, 1)]
    private float _sizePercentage; // The minimum size of the text
                                   // in percent

    private float _sizeMax; // Maximum text size

    private float _sizeMin; // Minimum text size

    // Start is called before the first frame update
    void Start()
    {
        _sizeMax = Text.fontSize; // Storing the max font size
        _sizeMin = _sizeMax * _sizePercentage; // Storing the min
                                               // font size
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBasicUISpeedEffect(); // Updating BasicUISpeedEffect

        // Changing the size of the font
        Text.fontSizeMax = Mathf.Lerp(_sizeMin, _sizeMax, step);
    }
}
