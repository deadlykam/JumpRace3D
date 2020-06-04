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
    public float SpeedGravity;    // The gravity speed
    public float SpeedJump;       // Normal jump speed
    public float SpeedFastJump;   // Extra jump speed for long jumps
    private float _extraVerticalSpeed; // Any extra vertical speed given

    /// <summary>
    /// The actual vertical speed with or without any extra speed, 
    /// of type float
    /// </summary>
    private float _actualVerticalSpeed
    { get { return SpeedJump + _extraVerticalSpeed; } }

    [Tooltip("Starting offset of the character")]
    public Vector3 StartOffset;   // This is the starting offset
                                  // of the character

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

    private bool _isEnableMovement = false; // This flag controls the
                                            // vertical movement of the
                                            // character

    /// <summary>
    /// This flag checks if movement is enabled, of type bool
    /// </summary>
    protected bool isEnableMovement { get { return _isEnableMovement; } }

    /// <summary>
    /// This flag checks if the character is moving forward or not,
    /// <para>true = forward</para>
    /// <para>false = NOT forward</para>
    /// of type bool
    /// </summary>
    protected bool isHorizontalMovement { get; set; }

    private Vector3 _characterPosition = Vector3.zero; // Needed to avoid 
                                                       // unnecessary GC

    public float AutoRotationSpeed; // The speed for auto rotation

    private Quaternion _targetRotation; // Storing the target rotation for
                                        // auto rotation

    private bool _isAutoRotateCharacter = false; // Flag for starting auto 
                                                 // rotate the character

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
        if (_isEnableMovement) // Checking if vertical movement is allowed
        {
            // Moving the character vertically
            transform.Translate(Vector3.up
                                  
                                  // Checking which speed to apply
                                * (_targetDir == 1 ?
                                   _actualVerticalSpeed :
                                    _acceleration < 0 ?
                                     SpeedGravity :
                                     _actualVerticalSpeed)

                                * _acceleration
                                * GameData.Instance.SimulationSpeed
                                * Time.deltaTime);

            // Condition to check if the character should start
            // falling down
            if (transform.position.y >= _heightCurrent)
            {
                _targetDir = -1; // Changing to falling direction
                _extraVerticalSpeed = 0; // Removing any extra speed
            }

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
    /// This method auto rotates character to the target
    /// rotation.
    /// </summary>
    private void AutoRotateCharacter()
    {
        // Condition to start auto rotation of the character
        if (_isAutoRotateCharacter &&
            !isHorizontalMovement)
        {
            // Condition for rotating the character
            if (transform.rotation != _targetRotation)
                transform.rotation = Quaternion.RotateTowards(
                                        transform.rotation,
                                        _targetRotation,
                                        AutoRotationSpeed *
                                        Time.deltaTime);
            // Condition for stopping auto rotation
            else _isAutoRotateCharacter = false;
        }
    }

    /// <summary>
    /// This method rotates the character.
    /// </summary>
    /// <param name="target">The target position needed for
    ///                      calculating character direction, 
    ///                      of type Vector3</param>
    protected void StartAutoRotation(Vector3 target)
    {
        // Fixing the target position for calculating
        // accurate rotation
        target.Set(target.x, 0, target.z);

        // Fixing character position for calculating
        // accurate rotation
        _characterPosition.Set(transform.position.x,
                               0,
                               transform.position.z);

        // Storing the target rotation
        _targetRotation = Quaternion.LookRotation(target - 
                                                  _characterPosition);

        // Condition to check if rotation is needed
        if (transform.rotation != _targetRotation)
            _isAutoRotateCharacter = true; // Start rotating the character
    }

    /// <summary>
    /// This method stops the auto rotation effect.
    /// </summary>
    protected void StopAutoRotation()
    {
        // Condition to check if auto rotation
        // is being in effect
        if(_isAutoRotateCharacter)
            _isAutoRotateCharacter = false; // Stopping
                                            // auto
                                            // rotation
    }

    /// <summary>
    /// This method handles the BasicCharacter update and must be called by
    /// the child class
    /// </summary>
    protected void UpdateBasicCharacter()
    {
        VerticalMovement(); // Updating jumping/gravity movement
        AutoRotateCharacter(); // Updating auto rotation
    }

    /// <summary>
    /// This method makes the character go forward.
    /// </summary>
    protected virtual void HorizontalMovement()
    {
        if (_isEnableMovement) // Checking if horizontal movement allowed
        {
            transform.Translate(Vector3.forward * SpeedHorizontal
                                * GameData.Instance.SimulationSpeed
                                * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// This method makes the character jump.
    /// </summary>
    /// <param name="height">The maximum height of the jump,
    ///                      of type float</param>
    protected void Jump(float height)
    {
        _targetDir = 1; // Making the player jump

        // Getting the height
        _heightCurrent = transform.position.y + height;
    }

    /// <summary>
    /// This method makes the character fall instantly
    /// </summary>
    protected void InstantFall()
    {
        _targetDir = -1; // Changing to falling direction
        _extraVerticalSpeed = 0; // Removing any extra speed
        _acceleration = 0; // Making acceleration to 0 because
                           // no transition to acceleration
                           // from jumping but transition
                           // to acceleration from 0
    }

    /// <summary>
    /// This method applies extra jump speed to the vertical speed.
    /// </summary>
    protected void ApplyExtraJumpSpeed()
    {
        _extraVerticalSpeed = SpeedFastJump;
    }

    /// <summary>
    /// This method finishes the race for the character.
    /// </summary>
    protected virtual void RaceFinished()
    {
        _isEnableMovement = false; // Stopping movement 
    }

    /// <summary>
    /// This method forcefully resets the character from any 
    /// current status.
    /// </summary>
    protected virtual void ForceReset()
    {
        _isEnableMovement = false; // Stopping movement
        _isAutoRotateCharacter = false; // Stopping rotation
    }

    /// <summary>
    /// This method checks for collisions.
    /// </summary>
    /// <param name="other">The collided object, of type Collider</param>
    protected virtual void OnTriggerEnter(Collider other)
    {
        // Condition to check if end stage reached
        if (other.CompareTag("EndStage"))
        {
            RaceFinished(); // Race completed
        }
    }

    /// <summary>
    /// This method sets the position of the character.
    /// </summary>
    /// <param name="x">The x-axis position, of type float</param>
    /// <param name="y">The y-axis position, of type float</param>
    /// <param name="z">The z-axis position, of type float</param>
    public virtual void SetStartPosition(float x, float y, float z)
    {
        transform.position = Vector3.zero; // Resetting the position
        transform.position.Set(x, y, z);   // Applying the new position
    }

    /// <summary>
    /// This method sets the position of the character.
    /// </summary>
    /// <param name="x">The start position, of type Vector3</param>
    public virtual void SetStartPosition(Vector3 position)
    {
        // Setting the starting position of the character
        // with StartOffset
        transform.position = position + StartOffset;
    }

    /// <summary>
    /// This method starts the character's vertical movement.
    /// </summary>
    public virtual void StartCharacter() { _isEnableMovement = true; }
}
