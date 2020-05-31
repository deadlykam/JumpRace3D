using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>Player</c> handles all the player related functions.
/// </summary>
public class Player : BasicAnimation
{
    [Header("Player Properties")]
    public float RotationSpeed;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBasicAnimation(); // Calling the animation update
        HorizontalMovement();   // Making player go forward
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
    /// This method makes the player go forward when triggered by
    /// the user.
    /// </summary>
    protected override void HorizontalMovement()
    {
        // Condition for moving forward and
        // rotation when given
        if (Input.GetMouseButton(0))
        {
            base.HorizontalMovement(); // Going forward
            RotatePlayer(); // Rotating the player
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
    }

    /// <summary>
    /// This method checks for collisions.
    /// </summary>
    /// <param name="other">The collided object, of type Collider</param>
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        // Condition to check if bouncy stage collided
        // and showing 3D texts
        if (other.CompareTag("BouncyStage"))
        {
            Jump(HeightNormal); // Jumping normal height

            // Hiding the booster
            other.GetComponent<BouncyStage>().SetBooster(false);

            // Generating 3D text from the currently hit stage
            Stage3DTextManager.Instance.Generate3DTexts(
                other.transform.GetComponent<BouncyStage>());
        }
        else if (other.CompareTag("Booster"))
        {
            Jump(HeightPerfect); // Jumping perfect height

            other.gameObject.SetActive(false); // Hiding the booster

            // Generating 3D text from the currently hit stage
            Stage3DTextManager.Instance.Generate3DTexts(
                other.transform.parent.GetComponent<BouncyStage>());
        }
        // Condition to check if to show booster
        else if (other.CompareTag("PlayerDetector"))
        {
            // Showing the booster
            other.transform.parent.GetComponent<BouncyStage>()
                .SetBooster(true);
        }
    }
}
