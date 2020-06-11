using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>Player</c> handles all the player related functions.
/// </summary>
public class Player : BasicAnimation
{
    [Header("Player Properties")]

    [SerializeField]
    private GameObject _floorDetector; // The line generator
    public float RotationSpeed; // Rotating camera speed
    public float HeightLong; // Long jump height

    [Header("Popup Text Colour Properties")]
    [SerializeField]
    private Color _colourPerfect1; // Perfect colour front

    [SerializeField]
    private Color _colourPerfect2; // Perfect colour back

    [SerializeField]
    private Color _colourLongJump1; // Long jump colour front

    [SerializeField]
    private Color _colourLongJump2; // Long jump colour back

    [SerializeField]
    private Color _colourGood1; // Good colour front

    [SerializeField]
    private Color _colourGood2; // Long jump colour back

    private int _previousStage = -1; // Keep track of the stage number,
                                    // needed for finding Long Jumps

    public static Player Instance;

    
    void Awake()
    {
        if (Instance == null) // NOT initialized
        {
            Instance = this; // Initializing the instance

            DontDestroyOnLoad(gameObject); // Making it available
                                           // throughout the game
        }
        else Destroy(gameObject); // Already initialized and
                                  // destroying duplicate
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBasicAnimation(); // Calling the animation update
        HorizontalMovement();   // Making player go forward

        if (Input.GetKeyDown(KeyCode.R))
        {
            SetRagdoll(true); // Starting ragdoll
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SetRagdoll(false); // Starting ragdoll
        }
    }

    /// <summary>
    /// This method rotates the player
    /// </summary>
    private void RotatePlayer()
    {
        // Rotating the player
        transform.Rotate(new Vector3(0, 
                                     Input.GetAxis("Mouse X"),
                                     0) * Time.deltaTime * RotationSpeed);
    }

    /// <summary>
    /// This method checks if the Long Jump popup conditions
    /// have been fulfilled.
    /// </summary>
    /// <param name="currentStage">The current stage number the player,
    ///                            has landed on, of type int</param>
    private void CheckLongJump(int currentStage)
    {
        // Condition for showing Long Jump popup
        if(_previousStage != -1 &&
           (currentStage != _previousStage) &&
           (currentStage != (_previousStage + 1)) &&
           (currentStage != (_previousStage - 1)))
        {
            // Starting long jump popup effect
            MainCanvasUI.Instance.StartPopup("Long Jump!", 
                _colourLongJump1, _colourLongJump2);
        }

        _previousStage = currentStage; // Updating the previous stage
    }

    /// <summary>
    /// This method checks if the Long Jump popup conditions
    /// have been fulfilled.
    /// </summary>
    /// <param name="currentStage">The current stage number the player,
    ///                            has landed on, of type int</param>
    /// <param name="isGood">Flag to check if to show 'Good' popup,
    ///                      of type bool</param>                           
    private void CheckLongJump(int currentStage, bool isGood)
    {
        // Condition for showing Long Jump popup
        if (_previousStage != -1 &&
           (currentStage != _previousStage) &&
           (currentStage != (_previousStage + 1)) &&
           (currentStage != (_previousStage - 1)))
        {
            // Starting long jump popup effect
            MainCanvasUI.Instance.StartPopup("Long Jump!",
                _colourLongJump1, _colourLongJump2);
        }
        // Condition to show 'Good' popup
        else if (isGood) {
            // Starting 'good' popup effect
            MainCanvasUI.Instance.StartPopup("Good",
                _colourGood1, _colourGood2);
            Debug.Log("Good");
        }

        _previousStage = currentStage; // Updating the previous stage
    }

    /// <summary>
    /// This method makes the player go forward when triggered by
    /// the user.
    /// </summary>
    protected override void HorizontalMovement()
    {
        // Condition for moving forward and
        // rotation when given
        if (Input.GetMouseButton(0) && 
            isEnableMovement)
        {
            base.HorizontalMovement(); // Going forward

            // Condition to check if NOT going forward
            // then changing flag to forward
            if (!isHorizontalMovement)
                isHorizontalMovement = true;

            RotatePlayer(); // Rotating the player
            StopAutoRotation(); // Stopping auto rotation effect
                                // if applied
        }
        else // Condition for NOT going forward
        {
            // Condition to check if going forward
            // then changing flag to NOT forward
            if (isHorizontalMovement)
                isHorizontalMovement = false;
        }
    }

    /// <summary>
    /// This method finishes the race for the player
    /// </summary>
    protected override void RaceFinished()
    {
        base.RaceFinished();

        Debug.Log("Raced Finished!");

        //TODO: The game has ended. Give end scene here

        MainCanvasUI.Instance.SetBar(1); // Level finished updating
                                         // bar to full

        // Showing the loading screen
        MainCanvasUI.Instance.SetLoadingUI(true); // <-- Remove from
                                                  //     here please

        StageGenerator.Instance.ResetStage(); // Resetting the stage
                                              // and starting a new
                                              // stage

        EnemyGenerator.Instance.ResetEnemy(); // Reset the enemies
                                              // in the game world

        _floorDetector.SetActive(false); // Hiding floor line

        _previousStage = -1; // Resetting for next stage
    }

    /// <summary>
    /// This method kills the player when the height threshold
    /// is crossed.
    /// </summary>
    protected override void CheckHeight()
    {
        // base.CheckHeight();

        if (isHeightStop) // Player crossed the threshold
        {
            ForceReset(); // Stopping Movement
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
        if (other.CompareTag("BouncyStage") ||
            other.CompareTag("Good"))
        {
            Jump(HeightNormal); // Jumping normal height

            JumpAnimation(); // Playing jump animation

            // Hiding the booster
            other.GetComponent<BouncyStage>().SetBooster(false);

            // Looking at the next stage
            StartAutoRotation(other.GetComponent<BouncyStage>()
                .LinkedStagePosition);

            // Activating the stage action
            other.GetComponent<BouncyStage>().StageAction();

            // Requesting leader position
            RaceTracker.Instance.AddRequest(
                other.GetComponent<BouncyStage>().StageNumber,
                ModelInfo);

            // Updating the Player bar UI
            StageGenerator.Instance
                .SetPlayerUIBar(other.GetComponent<BouncyStage>()
                                .StageNumber);

            // Checking to see if to show Long Jump
            CheckLongJump(other.GetComponent<BouncyStage>()
                                .StageNumber,
                                other.CompareTag("Good"));
        }
        else if (other.CompareTag("Booster"))
        {
            Jump(HeightPerfect); // Jumping perfect height

            JumpAnimation(); // Playing jump animation

            other.gameObject.SetActive(false); // Hiding the booster

            // Looking at the next stage
            StartAutoRotation(other.transform.parent
                .GetComponent<BouncyStage>().LinkedStagePosition);

            // Activating the stage action
            other.transform.parent
                .GetComponent<BouncyStage>().StageAction();

            // Requesting leader position
            RaceTracker.Instance.AddRequest(
                other.transform.parent
                .GetComponent<BouncyStage>().StageNumber,
                ModelInfo);

            // Updating the Player bar UI
            StageGenerator.Instance
                .SetPlayerUIBar(other.transform.parent
                    .GetComponent<BouncyStage>()
                    .StageNumber);

            // Activating the simulation speed effect
            GameData.Instance.StartSimulationSpeedEffect();

            // Showing the popup
            MainCanvasUI.Instance.StartPopup("Perfect", _colourPerfect1, 
                                             _colourPerfect2);

            // Updating the previous stage number
            _previousStage = other.transform.parent
                    .GetComponent<BouncyStage>()
                    .StageNumber;
        }
        // Condition for long jump
        else if (other.CompareTag("LongBouncyStage"))
        {
            Jump(HeightLong); // Jumping long height

            JumpAnimation(); // Playing jump animation

            ApplyExtraJumpSpeed(); // Applying extra jump speed

            // Activating disappearing process
            other.GetComponent<BouncyStageLong>().StageAction();

            _previousStage = 0; // Making previous stage to 0 so that
                                // landing on any stages will show
                                // long jump message except for
                                // landing on booster
        }
        // Condition to check if to show booster
        else if (other.CompareTag("PlayerDetector"))
        {
            // Showing the booster
            other.transform.parent.GetChild(0)
                .GetComponent<BouncyStage>().SetBooster(true);
        }
        // Condition to check if hitting bottom of a stage
        else if (other.CompareTag("StageBottom"))
        {
            InstantFall(); // Instantly falling
        }
        // Condition for dying and turning on ragdoll
        else if (other.CompareTag("Obstacle"))
        {
            ForceReset(); // Stopping Movement
        }
    }

    /// <summary>
    /// This method starts the player and shows the floor line.
    /// </summary>
    public override void StartCharacter()
    {
        base.StartCharacter();
        _floorDetector.SetActive(true); // Showing floor line
    }

    /// <summary>
    /// Stopping the player movements, starting the ragdoll 
    /// and hiding the floor detector
    /// </summary>
    public override void ForceReset()
    {
        base.ForceReset();

        SetRagdoll(true); // Starting ragdoll
        _floorDetector.SetActive(false); // Hiding floor line
    }
}
