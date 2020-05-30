﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    [Header("Stage Generator Properties")]
    public Transform BouncyStagesAvailable; // Transform containing all
                                            // the bouncy stages

    public Transform StageObjectsUsed;      // Transform containing all
                                            // the used objects

    [Tooltip("The amount of distance between each stages.")]
    public float OffsetStage; // Distance between each stages
    private float _offsetStageCurrent = 0; // Current Distance offset

    [Tooltip("The stage height offset when going up.")]
    public float OffsetHeight; // Height offset of stages

    [Tooltip("The horizontal offset for each stages in the same level.")]
    public float OffsetSide; // Horizontal offset of stages

    private Vector3 _stagePosition; // For storing the new stage object
                                    // position. Needed mainly to avoid
                                    // GC

    [Tooltip("The number of levels during a gameplay, Level != 0")]
    [Min(1)]
    public int Level; // The number of levels during a gameplay
    private int _levelCurrent = 0; // Current levels generated
    
    [Tooltip("The number of stages in a level")]
    public int StageNumber; // Number of stages that will be generated
                            // in a level

    private int _stageGeneratedCounter = 0; // The counter used for 
                                            // tracking the number of 
                                            // stages created

    private int _stageIndex; // The index of the stage object 
                             // to generate

    // Stores all the stage object requests
    private List<StageObjectRequest> _stageObjectRequests 
        = new List<StageObjectRequest>();

    private StageObjectRequest _stageObjectRequestCurrent; // Current stage
                                                           // object request

    // For checking if any requests are available
    private bool _isRequest { get { return _stageObjectRequests.Count != 0; } }

    private bool _isProcessing = false; // Flag to check if a stage object
                                        // generation is being processed

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        // Condition to check if level generation is NOT done
        if (_levelCurrent < Level)
        {
            // Checking if stage limit NOT reached
            if (_stageGeneratedCounter < StageNumber)
            {
                if (!_isProcessing) // Checking if no stage object being processed
                {
                    if (!_isRequest) // Checking if no requests available
                    {
                        SetRequest();// Getting a request
                    }
                    else // Request is available
                    {
                        _isProcessing = true; // Processing started
                        ProcessRequests(); // Processing the request
                    }
                }
            }
            else GenerateNewLevel(); // Starting a new level generation
        }

    }

    /// <summary>
    /// This method adds a request for creating a stage object.
    /// </summary>
    private void SetRequest()
    {
        // Adding request
        _stageObjectRequests.Add(new StageObjectRequest(
                                     Random.Range(0,
                                     BouncyStagesAvailable.childCount)));
    }

    /// <summary>
    /// This method processes the stage object request.
    /// </summary>
    private void ProcessRequests()
    {
        // Getting the current stage object request
        _stageObjectRequestCurrent = _stageObjectRequests[0];
        _stageObjectRequests.RemoveAt(0); // Removing the stage
                                          // object request

        // Adding the stage object
        AddStageObject(_stageObjectRequestCurrent.Index);

        _stageGeneratedCounter++; // stage object added
        _isProcessing = false; // Processing finished
    }

    /// <summary>
    /// This method adds the stage object to the game world.
    /// </summary>
    /// <param name="index">The index of the stage object, of type int</param>
    private void AddStageObject(int index)
    {
        _offsetStageCurrent += OffsetStage; // Getting the new stage
                                    // distance value

        _stagePosition = Vector3.zero; // Resetting the value
                                       // to get accurate
                                       // calculation

        // Setting up the new stage object position
        _stagePosition.Set(OffsetSide,
                           OffsetHeight,
                           _offsetStageCurrent);

        // Setting the stage object position
        BouncyStagesAvailable.GetChild(index).position = _stagePosition;

        // Showing the stage object
        BouncyStagesAvailable.GetChild(index).gameObject.SetActive(true);

        // Removing the stage object from the available list
        BouncyStagesAvailable.GetChild(index).SetParent(StageObjectsUsed);
    }

    /// <summary>
    /// This method starts a new level generation.
    /// </summary>
    private void GenerateNewLevel()
    {
        _stageGeneratedCounter = 0;
        _levelCurrent++;

        // TODO: implement the height offset here

        // 50% probability to change the direction of the distance offset
        //OffsetZ = (Random.Range(0, 10) < 5) ? OffsetZ * -1 : OffsetZ;
    }

    /// <summary>
    /// This struct creates a stage object request.
    /// </summary>
    struct StageObjectRequest
    {
        public int Index; // The index of the stage object

        /// <summary>
        /// Constructor for creating a stage object request.
        /// </summary>
        /// <param name="index">The index of the object requested,
        ///                     of type int</param>
        public StageObjectRequest(int index)
        {
            Index = index;
        }
    }
}
