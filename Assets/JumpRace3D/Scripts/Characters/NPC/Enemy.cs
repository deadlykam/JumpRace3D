using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BasicAnimation
{
    [Header("Enemy Properties")]
    [SerializeField]
    private float _offsetXAxis; // Random x axis value for end stage

    [SerializeField]
    private float _offsetZAxis; // Random z axis value for end stage

    private Vector3 _nextStagePosition; // Storing the next stage
                                        // position

    // Update is called once per frame
    void Update()
    {
        UpdateBasicAnimation(); // Calling the animation update
        HorizontalMovement();   // Moving the enemy
    }

    /// <summary>
    /// Method for moving the enemy horizontally.
    /// </summary>
    protected override void HorizontalMovement()
    {
        // Condition to check if enemy movement is enabled
        if (isEnableMovement)
        {
            // Fixing the stage position
            _nextStagePosition.Set(
                    _nextStagePosition.x,
                    transform.position.y,
                    _nextStagePosition.z);

            // Condition for making the enemy move
            if (transform.position != _nextStagePosition)
            {
                // Moving the enemy towards the stage position
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    _nextStagePosition,
                    SpeedHorizontal 
                    * GameData.Instance.SimulationSpeed 
                    * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// This method checks for collisions.
    /// </summary>
    /// <param name="other">The collided object, of type Collider</param>
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        // Condition to check if bouncy stage collided
        if (other.CompareTag("BouncyStage"))
        {
            // Storing the next stage position
            _nextStagePosition = other.GetComponent<BouncyStage>()
                .LinkedStagePosition;

            Jump(HeightNormal); // Jumping normal height

            JumpAnimation(); // Playing jump animation

            // Condition to check if stage is breakable and NOT
            // calling to break the stage
            if (other.GetComponent<BouncyStageBreakable>() != null)
                other.GetComponent<BouncyStage>().StageAction(false);
            else // Condition for NON breakable stages
                other.GetComponent<BouncyStage>().StageAction();


            // Looking at the next stage
            StartAutoRotation(other.GetComponent<BouncyStage>()
                .LinkedStagePosition);
            
            // Updating stage number
            SetStageNumber(other
                .GetComponent<BouncyStage>().StageNumber);

            // Condition to check if it is the last stage and giving
            // a random position within limit
            if (isLastStage)
                _nextStagePosition.Set(
                    _nextStagePosition.x + Random.Range(-_offsetXAxis, 
                                                        _offsetXAxis),
                    _nextStagePosition.y,
                    _nextStagePosition.z + Random.Range(-_offsetZAxis, 
                                                         _offsetZAxis));
        }
        // Condition for dying and turning on ragdoll
        else if (other.CompareTag("Player"))
        {
            // Condition for able to die
            if (!isRaceFinished)
            {
                SetRagdoll(true); // Starting ragdoll
                DieCharacter(); // Killing the character

                // Play hurt sfx
                AudioManager.Instance.PlayHurt();

                // Setting the stage number to max stage number
                SetStageNumber(-1);
            }
        }
    }

    /// <summary>
    /// Setting the start and stage position for the enemy.
    /// </summary>
    /// <param name="position">The start and stage position,#
    ///                        of type Vector3</param>
    public override void SetStartPosition(Vector3 position)
    {
        base.SetStartPosition(position); // Setting start position
        _nextStagePosition = position; // Setting stage position
    }
}
