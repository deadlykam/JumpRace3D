using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupUI : TextSizeUI
{
    [Header("Popup UI Properties")]
    [SerializeField]
    private TextMeshProUGUI _text2; // Text UI to change size of

    [SerializeField]
    private float _timer; // The amount of time needed to show
                          // the text

    private float _timerCurrent;

    private bool _isEffect; // Flag to check if effect in process
    private bool _isBig; // Flag to check if the text went to biggest font

    // Update is called once per frame
    void Update()
    {
        UpdateTextEffect(); // Calling to do the popup effect
    }

    /// <summary>
    /// This method does the popup effect.
    /// </summary>
    private void UpdateTextEffect()
    {
        // Checking if popup effect is active
        if (_isEffect)
        {
            // Condition to call update
            if(_timerCurrent == 0)
                UpdateBasicUISpeedEffect(); // Updating BasicUISpeedEffect

            // Changing the size of the font
            text.fontSizeMax = Mathf.Lerp(sizeMin, sizeMax, step);

            // Changing the size of the font
            _text2.fontSizeMax = Mathf.Lerp(sizeMin, sizeMax, step);

            if (!_isBig) // Checking if font not big yet
            {
                // Condition to check if the font is big
                if (step == 1)
                {
                    _isBig = true; // Font is big
                    _timerCurrent = _timer; // Resetting timer
                }
            }
            else // Font is big
            {
                // Checking if font is back to original size
                if (step == 0 && _timerCurrent == 0)
                {
                    _isEffect = false; // Stopping popup effect

                    text.gameObject.SetActive(false);   // Hiding text
                    _text2.gameObject.SetActive(false); // Hiding text
                }
                // Condition to countdown the timer for showing
                // the max font
                else _timerCurrent = _timerCurrent - fps <= 0 ? 0 : 
                                     _timerCurrent - fps;
            }
        }
    }

    /// <summary>
    /// This method starts the popup effect.
    /// </summary>
    public void ShowPopup()
    {
        // Checking if the text is NOT in effect mode
        if (!_isEffect)
        {           
            text.gameObject.SetActive(true);   // Showing text
            _text2.gameObject.SetActive(true); // Showing text

            _timerCurrent = 0; // Resetting the timer

            _isEffect = true; // Starting text effect
            _isBig = false;   // text font NOT big
        }
    }

    /// <summary>
    /// This method sets the text and colour of the message.
    /// </summary>
    /// <param name="msg">The popup text, of type string</param>
    /// <param name="colour1">Colour for the first text, 
    ///                       of type Color</param>
    /// <param name="colour2">Colour for the second text,
    ///                       of type Color</param>                      
    public void SetText(string msg, Color colour1, Color colour2)
    {
        text.text = msg;  // Setting popup text
        _text2.text = msg;// Setting popup text

        text.color = colour1;   // Setting first text colour
        _text2.color = colour2; // Setting second text colour
    }
}
