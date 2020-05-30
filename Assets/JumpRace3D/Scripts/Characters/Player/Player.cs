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
    /// This method checks for collisions.
    /// </summary>
    /// <param name="other">The collided object, of type Collider</param>
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
