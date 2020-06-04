using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BasicAnimation
{
    [Header("Enemy Properties")]
    public float DeathHeight; // The height after which the enemy
                              // will die

    private Vector3 _nextStagePosition; // Storing the next stage
                                        // position
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBasicAnimation(); // Calling the animation update
        HorizontalMovement();   // Moving the enemy
        DieCharacter();
    }

    /// <summary>
    /// This method kills the enemy.
    /// </summary>
    protected override void DieCharacter()
    {
        // Condition to kill the enemy
        if(transform.position.y <= DeathHeight)
            base.DieCharacter(); // Killing the enemy
    }

    /// <summary>
    /// Method for moving the enemy horizontally.
    /// </summary>
    protected override void HorizontalMovement()
    {
        //base.HorizontalMovement();

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

            // Looking at the next stage
            StartAutoRotation(other.GetComponent<BouncyStage>()
                .LinkedStagePosition);
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
