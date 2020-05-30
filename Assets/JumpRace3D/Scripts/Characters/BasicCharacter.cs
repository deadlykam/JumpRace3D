﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>BasicCharacter</c> handles all the functionality common between all characters.
/// </summary>
public class BasicCharacter : MonoBehaviour
{
    [Header("Basic Character Properties")]

    [Tooltip("This value behaves for both jump and gravity")]
    public float SpeedHorizontal; // The forward speed
    public float SpeedVertical;   // Jump and gravity value

    public float HeightNormal;    // Normal height of a jump
    public float HeightPerfect;   // Perfect height of a jump
    private float _heightCurrent; // The current height from the bounced stage

    [Tooltip("The jump acceleration transition. 0 = instant transition, 1 = transition")]
    [Range(0, 1)]
    public float JumpSmooth;

    [Tooltip("The gravity acceleration transition. 0 = instant transition, 1 = transition")]
    [Range(0, 1)]
    public float GravitySmooth;

    private float _acceleration = 1; // The acceleration of gravity
                                     // and jump

    private float _verticalVelocity; // Vertical velocity needed for
                                     // acceleration calculation

    private int _targetDir = -1; // The direction at which the 
                                 // character will move vertically
                                 //
                                 // Values:
                                 //  1 = Jumping up
                                 // -1 = Falling down
                                 //  0 = Stop vertival movement

    private bool _isVerticalMovement = true; // This flag controls the
                                             // vertical movement of the
                                             // character

    private Vector3 _characterPosition = Vector3.zero; // Needed to avoid 
                                                       // unnecessary GC

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBasicCharacter(); // Calling BasicCharacter Update
    }

    /// <summary>
    /// This method makes the basic character to fall down or jump up.
    /// </summary>
    private void VerticalMovement()
    {
        if (_isVerticalMovement) // Checking if vertical movement is allowed
        {
            // Moving the character vertically
            transform.Translate(Vector3.up * SpeedVertical * _acceleration * Time.deltaTime);

            // Condition to check if the character should start
            // falling down
            if (transform.position.y >= _heightCurrent) _targetDir = -1;

            // Smoothing the acceleration of the character
            _acceleration = Mathf.SmoothDamp(_acceleration,
                                             _targetDir,
                                             ref _verticalVelocity,
                                             // Checking which smooth
                                             // acceleration to use
                                             _targetDir == 1 ?
                                             JumpSmooth :
                                             GravitySmooth);
        }
    }

    /// <summary>
    /// This method rotates the character.
    /// </summary>
    /// <param name="target">The target position needed for
    ///                      calculating character direction, 
    ///                      of type Vector3</param>
    private void RotateCharacter(Vector3 target)
    {
        // Fixing character position for calculating
        // accurate rotation
        _characterPosition.Set(transform.position.x,
                               0,
                               transform.position.z);

        // Looking at the target instantly
        transform.rotation = Quaternion.LookRotation(target - _characterPosition);
    }

    /// <summary>
    /// This method handles the BasicCharacter update and must be called by
    /// the child class
    /// </summary>
    protected void UpdateBasicCharacter()
    {
        VerticalMovement();
    }

    /// <summary>
    /// This method makes the character go forward.
    /// </summary>
    protected virtual void HorizontalMovement()
    {
        transform.Translate(Vector3.forward * SpeedHorizontal * Time.deltaTime);
    }

    /// <summary>
    /// This method checks for collisions.
    /// </summary>
    /// <param name="other">The collided object, of type Collider</param>
    protected virtual void OnTriggerEnter(Collider other)
    {
        // Condition to check if bouncy stage collided
        // and making character jump
        if (other.CompareTag("BouncyStage"))
        {
            _targetDir = 1; // Making the player jump

            // Getting the normal height
            _heightCurrent = transform.position.y + HeightNormal;

            // Hiding the booster
            other.GetComponent<BouncyStage>().SetBooster(false);
        }
        // Condition to check if booster collided
        // and making character jump
        else if (other.CompareTag("Booster"))
        {
            _targetDir = 1; // Making the player jump

            // Getting the normal height
            _heightCurrent = transform.position.y + HeightPerfect;

            other.gameObject.SetActive(false); // Hiding the booster
        }
        // Condition to check if to show booster
        else if (other.CompareTag("PlayerDetector"))
        {
            // Showing the booster
            other.transform.parent.GetComponent<BouncyStage>()
                .SetBooster(true);
        }
        // Condition to check if end stage reached
        else if (other.CompareTag("EndStage"))
        {
            _isVerticalMovement = false; // Stopping vertical movement

            //TODO: The game has ended. Give end scene here
        }
    }
}
