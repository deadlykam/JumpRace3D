using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpRaceInput : MonoBehaviour
{
    private float _xAxis; // The x-axis value from the screen

    /// <summary>
    /// Gets the x axis value from device screen, of type float
    /// </summary>
    public float X_Axis { get { return _xAxis; } }

    ///===
    private Vector2 _posStart;
    private Vector2 _posEnd;

    [SerializeField]
    private float _touchMin;

    /// <summary>
    /// Flag checking if the drag is allowed, of type bool
    /// </summary>
    private bool _isDrag
    { get { return Mathf.Abs(_posStart.x - _posEnd.x) >= _touchMin; } }

    public static JumpRaceInput Instance;

    private void Awake()
    {
        if (Instance == null) // NOT initialized
        {
            Instance = this; // Initializing the instance
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateXAxisTouch(); // Updating the x-axis value
        //UpdateXValueMouse(); 
    }

    /// <summary>
    /// Updates the x-axis value through touching.
    /// </summary>
    private void UpdateXAxisTouch()
    {
        // Loop to find all the touches
        foreach(Touch touch in Input.touches)
        {
            // Checking if the touch began
            if(touch.phase == TouchPhase.Began ||
               touch.phase == TouchPhase.Stationary)
            {
                _posStart = touch.position; // Setting start position
                _posEnd = touch.position; // Setting end position

                RestXAxis(); // Resetting the x-axis value
            }

            // Cheking if touch has moved
            if(touch.phase == TouchPhase.Moved)
            {
                _posEnd = touch.position; // Setting moved end position

                if (_isDrag) // Checking if allowed to be dragged
                {
                    // Updating the x-axis
                    //_xAxis = _posStart.x - _posEnd.x;
                    _xAxis = touch.deltaPosition.x;
                }
            }
        }

        // Condition to reset the x-axis value
        if (Input.touches.Length == 0) RestXAxis();
    }

    /// <summary>
    /// This method resets the x axis value.
    /// </summary>
    private void RestXAxis()
    {
        _xAxis = 0;

        /* Hint: Keeping this code commented out to show that
         *       a smooth transition to reset x-axis can be done
         *       here if needed but the _step and _fps variables
         *       must be created and the start value for _step
         *       should be 1. _step = 0 should also be done in
         *       TouchPhase.Moved condition
         *
         */
        /*// Storing the fps value for accurate calculation
        _fps = Time.deltaTime;

        // Fixing and calculating the ste value
        _step = (_step + (_fps * _resetSpeed)) >= 1 ? 
                1 : 
                (_step + (_fps * _resetSpeed));

        _xAxis = Mathf.Lerp(_xAxis, 0, _step);*/
    }
}
