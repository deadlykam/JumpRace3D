using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>BasicCharacter</c> handles all the functionality common between all characters.
/// </summary>
public class BasicCharacter : MonoBehaviour
{
    [Header("Basic Character Properties")]

    [Tooltip("This value behaves for both jump and gravity")]
    public float speed; // Jump and gravity value

    public float HeightNormal;  // Normal height of a jump
    public float HeightPerfect; // Perfect height of a jump

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
    /// This method handles the BasicCharacter update and must be called by
    /// the child class
    /// </summary>
    protected void UpdateBasicCharacter()
    {
        VerticalMovement();
    }

    /// <summary>
    /// This method makes the basic character to fall down or jump up.
    /// </summary>
    private void VerticalMovement()
    {
        // Moving the character vertically
        transform.Translate(Vector3.up * speed * _acceleration * Time.deltaTime);

        // Condition to check if the character should start
        // falling down
        if (transform.position.y >= HeightNormal) _targetDir = -1;

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

    /// <summary>
    /// This method checks for collisions
    /// </summary>
    /// <param name="other">The collided object, of type Collider</param>
    private void OnTriggerEnter(Collider other)
    {
        // Condition to check if bouncy stage collided
        // and making character jump
        if (other.CompareTag("BouncyStage")) _targetDir = 1;
    }
}
