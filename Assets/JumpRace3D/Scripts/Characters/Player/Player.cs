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

    [SerializeField]
    private float _timerEndScreen; // The time needed to show the
                                   // end screen

    private float _timerEndScreenCurrent; // The current time for
                                          // showing the end screen

    private bool _isEndScreenProcess = false; // Flag to start the
                                              // end screen process
              
    private Vector3 _rotatePlayer; // Needed to store
                                   // the rotation
                                   // value and to avoid
                                   // GC

    /// <summary>
    /// Flag to check if to show the end screen, of type bool
    /// </summary>
    private bool _isShowEndScreen
    { get { return _timerEndScreenCurrent >= _timerEndScreen; } }

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

        // Condition to show the end screen
        if (_isEndScreenProcess) ShowEndScreen();
    }

    private void ShowEndScreen()
    {
        if (_isShowEndScreen)
        {
            // Calling to update the race position
            RaceTracker.Instance.UpdateRacePosition(true);
            _isEndScreenProcess = false; // Stopping the end screen process
            _timerEndScreenCurrent = 0;  // Resetting the timer
        }
        // Increasing the timer
        else _timerEndScreenCurrent += Time.deltaTime;
    }

    /// <summary>
    /// This method rotates the player
    /// </summary>
    private void RotatePlayer()
    {
#if UNITY_EDITOR
        // Rotating the player
        transform.Rotate(new Vector3(0, 
                                     Input.GetAxis("Mouse X"),
                                     0) * Time.deltaTime * RotationSpeed * 10);

#endif
        
#if UNITY_ANDROID && !UNITY_EDITOR
        // Setting the rotation value of the player
        _rotatePlayer.Set(0,
                          JumpRaceInput.Instance.X_Axis,
                          0);

        // Rotating the player
        transform.Rotate(_rotatePlayer * Time.deltaTime * RotationSpeed);
#endif
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
        if(StageNumber != -1 &&
           (currentStage != StageNumber) &&
           (currentStage != (StageNumber + 1)) &&
           (currentStage != (StageNumber - 1)))
        {
            // Starting long jump popup effect
            MainCanvasUI.Instance.StartPopup("Long Jump!", 
                _colourLongJump1, _colourLongJump2);
        }
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
        if (StageNumber != -1 &&
           (currentStage != StageNumber) &&
           (currentStage != (StageNumber + 1)) &&
           (currentStage != (StageNumber - 1)))
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
        }
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

        // Plays the confetti effect
        ParticleGenerator.Instance.PlayConfetti();

        _timerEndScreenCurrent = 0;  // Resetting the timer
        _isEndScreenProcess = true;  // Starting the end screen process

        MainCanvasUI.Instance.SetBar(1); // Level finished updating
                                         // bar to full
                                         
        _floorDetector.SetActive(false); // Hiding floor line

        // Playing confetti sfx
        AudioManager.Instance.PlayConfetti();

        // Hiding the booster effect
        ParticleGenerator.Instance.SetBooster(false);
    }

    /// <summary>
    /// This method kills the player when the height threshold
    /// is crossed.
    /// </summary>
    protected override void CheckHeight()
    {
        if (isHeightStop) // Player crossed the threshold
        {
            ForceReset(); // Stopping Movement

            /* Hint: If the floor detection gives problem or
             *       doesn't look good for the water splash
             *       then commenting out the code below will
             *       give more accurate value and also show
             *       water splash effect. The reason for using
             *       floot detection for now is that it looks
             *       and feels better.
             */
            /*// Showing the water splash effect
            ParticleGenerator.Instance
                .PlaceWaterSplash(transform.position);*/
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

            // Updating the Player bar UI
            StageGenerator.Instance
                .SetPlayerUIBar(other.GetComponent<BouncyStage>()
                                .StageNumber);

            // Checking to see if to show Long Jump
            CheckLongJump(other.GetComponent<BouncyStage>()
                                .StageNumber,
                                other.CompareTag("Good"));

            // Updating stage number
            SetStageNumber(other
                .GetComponent<BouncyStage>().StageNumber);

            // Calling to update the race position
            RaceTracker.Instance.UpdateRacePosition();

            // Hiding the booster effect
            ParticleGenerator.Instance.SetBooster(false);

            // Playing stage bounce sfx
            AudioManager.Instance.PlayStageBounce();
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
            SetStageNumber(other.transform.parent
                    .GetComponent<BouncyStage>()
                    .StageNumber);

            // Calling to update the race position
            RaceTracker.Instance.UpdateRacePosition();

            // Starting the booster pickup effect
            ParticleGenerator.Instance
                .PlaceBoosterPickup(transform.position);

            // Showing the booster effect
            ParticleGenerator.Instance.SetBooster(true);

            // Playing Booster Pickup sfx
            AudioManager.Instance.PlayBoosterPickup();
        }
        // Condition for long jump
        else if (other.CompareTag("LongBouncyStage"))
        {
            Jump(HeightLong); // Jumping long height

            JumpAnimation(); // Playing jump animation

            ApplyExtraJumpSpeed(); // Applying extra jump speed

            // Activating disappearing process
            other.GetComponent<BouncyStageLong>().StageAction();

            // Hiding the booster effect
            ParticleGenerator.Instance.SetBooster(false);

            // Playing stage bounce sfx
            AudioManager.Instance.PlayStageBounce();

            // This may or may not be included later but will need
            // to be thought about
            //_previousStage = 0; // Making previous stage to 0 so that
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

            // Playing hurt sfx
            AudioManager.Instance.PlayHurt();
        }
        // Condition for touch the floor and activating
        // water splash effect
        else if (other.CompareTag("Floor"))
        {
            // Showing the water splash effect
            ParticleGenerator.Instance
                .PlaceWaterSplash(transform.position);

            // Playing water splash sfx
            AudioManager.Instance.PlayerWaterSplash();
        }
    }

    /// <summary>
    /// This method checks for collision exits.
    /// </summary>
    /// <param name="other">The exiting colliding object, 
    ///                     of type Collider</param>
    protected void OnTriggerExit(Collider other)
    {
        // Condition for exiting the player detector
        if (other.CompareTag("PlayerDetector"))
        {
            // Hiding the booster
            other.transform.parent.GetChild(0)
                .GetComponent<BouncyStage>().SetBooster(false);
        }
    }

    /// <summary>
    /// This method places the booster on to the player's
    /// character feet.
    /// </summary>
    public void SetBoosters()
    {
        ParticleGenerator.Instance.PlaceBooster(ModelInfo);
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

        // Stopping to increase the level and allowing
        // to regenerate the current level
        StageGenerator.Instance.NotIncreaseLevel();

        SetStageNumber(-1); // Resetting the stage number

        // Calling to update the race position
        //RaceTracker.Instance.UpdateRacePosition(true);

        _timerEndScreenCurrent = 0;  // Resetting the timer
        _isEndScreenProcess = true;  // Starting the end screen process

        // Removing booster so that ragdoll has no errors
        ParticleGenerator.Instance.RemoveBooster();
        SetRagdoll(true); // Starting ragdoll
        _floorDetector.SetActive(false); // Hiding floor line
    }
}
