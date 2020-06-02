﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyStageBreakable : BouncyStage
{
    [Header("Breakable Properties")]
    public StageForce[] BreakableObjects; // Storing the
                                          // breakable
                                          // objects

    private int _pointer;  // Pointer needed to request
                           // breaking the breakable
                           // objects

    // Flag to check if stage is breaking
    private bool _isActivated
    { get { return _pointer < BreakableObjects.Length; } }
    
    private bool _isReset = false; // Flag for resetting
                                   // the breakable stage

    // Start is called before the first frame update
    void Start()
    {
        _pointer = BreakableObjects.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isActivated && !_isReset) // Condition for breaking the stage
        {
            // Enabling the individual forces
            BreakableObjects[_pointer++].ActivateForce();
        }
        else if(_isActivated && _isReset) // Condition for resetting the stage
        {
            // Resetting the individual forces
            BreakableObjects[_pointer++].ResetForce();
        }
    }

    /// <summary>
    /// This method shows the particle effects and 
    /// breaks the stage.
    /// </summary>
    /// <param name="isActivated">Flag to break or reset a stage,
    ///                           <para>false = break</para>
    ///                           <para>true = reset</para>
    ///                           of type bool
    ///                           </param>
    public override void StageAction(bool isActivated)
    {
        base.StageAction(); // Showing particle effects

        _pointer = 0; // Starting the stage action process

        _isReset = isActivated; // Breaking or resetting
                                // stage
    }
}