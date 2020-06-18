using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadUI : SideMoveUI
{
    [Header("Load UI Properties")]
    [SerializeField]
    private TextMeshProUGUI _text;
    
    private float _currentValue; // The target value

    /// <summary>
    /// Flag to check if update is needed for the UI, of type bool
    /// </summary>
    private bool _isUpdateUI { get { return step != _currentValue; } }

    private int _percentageValue { get { return (int)(step * 100); } }

    // Update is called once per frame
    void Update()
    {
        UpdateBasicUIEffect(); // Calling update on BasicUIEffect

        if(_isUpdateUI) // Checking if update is needed
        {
            // Calculating the step
            step = (step + (speed * fps)) >= _currentValue ? 
                                                _currentValue : 
                                                (step + (speed * fps));

            // Moving the UI
            transform.position = Vector3.Lerp(leftTarget.position,
                                              rightTarget.position,
                                              step);

            // Updating the text
            _text.text = _percentageValue.ToString() + "%";
        }
    }

    /// <summary>
    /// This method sets the percentage value for moving the UI.
    /// </summary>
    /// <param name="amount">The percentage amount to set between 0 - 1,
    ///                      of type float</param>
    public void SetPercentage(float amount)
    {
        // Fixing amount value
        amount = amount > 1 ? 1 : amount < 0 ? 0 : amount;
        _currentValue = amount; // Setting current value
    }

    /// <summary>
    /// This method checks if the current progress has not been done.
    /// </summary>
    /// <param name="amount">The progres amount to check if NOT done,
    ///                      of type float</param>
    /// <returns>True means progress NOT done, false otherwise, 
    ///          of type bool</returns>
    public bool CheckProgress(float amount) { return _currentValue != amount; }
}
