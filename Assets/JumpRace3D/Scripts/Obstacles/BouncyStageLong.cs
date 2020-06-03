using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyStageLong : BasicStage
{
    [Header("Disappear Properties")]
    [Min(1)]
    public float Timer; // Maximum time needed to make
                        // the stage disappear, range
                        // Timer >= 1

    private float _timerCurrent; // Current timer for
                                 // making the stage
                                 // disappear

    private bool _isDisappearProcess { get { return _timerCurrent < Timer; } }

    // Start is called before the first frame update
    void Start()
    {
        ResetTimer(); // Resetting the timer at start
    }

    // Update is called once per frame
    void Update()
    {
        // Condition for processing the disappearence of
        // long bouncy stage
        if (_isDisappearProcess)
        {
            // Counting up to hide the long bouncy stage
            _timerCurrent += Time.deltaTime;
            
            // Condition for hiding the long bouncy stage
            if (!_isDisappearProcess)
            {
                ResetTimer(); // Resetting the timer

                // Hint: Check if parent needs to be hiden or self
                gameObject.SetActive(false); // Disappearing
            }
        }
    }

    /// <summary>
    /// This method resets the timer.
    /// </summary>
    private void ResetTimer() { _timerCurrent = Timer; }

    /// <summary>
    /// This method starts the particle effect and the disappearing
    /// process.
    /// </summary>
    public override void StageAction()
    {
        base.StageAction(); // Calling particle effect
        _timerCurrent = 1;  // Starting the disappearing process
    }
}
