using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>Player</c> handles all the player related functions.
/// </summary>
public class Player : BasicAnimation
{
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
    /// This method makes the player go forward when triggered by
    /// the user.
    /// </summary>
    protected override void HorizontalMovement()
    {
        // Condition for moving forward
        if (Input.GetMouseButton(0)) base.HorizontalMovement();
    }
}
