﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridGenerator))]

/// <summary>
/// Class <c>StageGenerator</c> handles all the functionality for creating a stage, 
/// is responsible for placing all the objects needed for a stage.
/// </summary>
public class StageGenerator : MonoBehaviour
{
    public enum ProcessStatus { None, Generating, Reseting, Starting};

    [Header("Stage Properties")]
    public Transform BouncyStagesAvailable; // Transform containing all
                                            // the bouncy stages

    public Transform StageObjectsUsed;      // Transform containing all
                                            // the used objects

    private List<Transform> _obstaclesUsed  // Storing used obstacles
        = new List<Transform>();

    private GridGenerator _gridGenerator;   // Containing all the grid
                                            // points in the game world

    public ProcessStatus Status; // The status of the processing

    [Tooltip("The amount of distance between each stages.")]
    public float OffsetStage; // Distance between each stages
    private float _offsetStageCurrent = 0; // Current Distance offset

    [Tooltip("The stage height offset when going up.")]
    public float OffsetHeight; // Height offset of stages
    private float _offsetHeightCurrent = 0; // Current Height offset

    [Tooltip("The horizontal offset for each stages in the same level.")]
    public float OffsetSide; // Horizontal offset of stages
    private float _offsetSideCurrent; // Current side offset

    private BouncyStage _currentBouncyStage; // For storing the current 
                                             // or last BouncyStage,
                                             // needed later for giving
                                             // stage number.

    private Vector3 _stagePosition; // For storing the calculated stage
                                    // object position. Needed mainly to
                                    // avoid GC

    [Header("Level Properties")]
    [Tooltip("The maximum number of levels that can be reached, " +
             "Level != 0")]
    [Min(1)]
    public int LevelMax; // The maximum amount of level that can
                         // be generated
    private int _levelNumberCurrent = 1; // Current game level
                                         // the player has
                                         // reached
    [Tooltip("The number of levels during a gameplay, " +
             "Level != 0 && Level <= LevelMax")]
    [Min(1)]
    [SerializeField]
    private int _level = 1; // The number of levels during a gameplay
    private int _levelCurrent = 0; // Current levels generated
    private Vector3 _linePoint; // For storing the current line point.
                                // Needed mainly to avoid GC
    
    /// <summary>
    /// Flag to check if there are any used stages except the
    /// end stage, of type bool
    /// </summary>
    private bool _hasUsedStages
    { get { return StageObjectsUsed.childCount > 1; } }

    /// <summary>
    /// Flag to check if there are any used obstacles,
    /// of type bool
    /// </summary>
    private bool _hasUsedObstacles
    { get { return _obstaclesUsed.Count > 0; } }

    [Tooltip("The number of stages in a level")]
    [Min(1)]
    public int StageNumber; // Number of stages that will be generated
                            // in a level

    private int _stageGeneratedCounter = 0; // The counter used for 
                                            // tracking the number of 
                                            // stages created

    private int _stageIndex; // The index of the stage object 
                             // to generate

    private int _stageNumberCounter = 1; // Needed to set the number of
                                         // the stage

    Vector3 _stagePosCurrent = Vector3.zero; // For storing current
                                             // stage position which
                                             // will be needed for
                                             // calculation. Needed
                                             // to avoid GC

    Vector3 _stagePosPrevious = Vector3.zero; // For storing current
                                              // stage position which
                                              // will be needed for
                                              // calculation. Needed
                                              // to avoid GC

    [Header("Generation Correction Properties")]
    public int CorrectionCounter; // The counter to make sure
                                  // the stage generation remains
                                  // within the game world size

    private int _correctionCounter = -1; // The current correction
                                         // counter

    private bool _isCorrectionProcess
    { get { return _correctionCounter < CorrectionCounter && _correctionCounter > -1; } }

    [Header("Obstacle Properties")]
    public Transform ObstacleContainer; // Container containing obstacles

    private int _sizeObstacle // The size of the obstacle container
    { get { return ObstacleContainer.childCount; } }

    private int _pointerObstacle = 0; // The index pointer for the
                                      // obstacle container

    // Flag to check if obstacles are available
    private bool _isObstacleAvailable
    { get { return _pointerObstacle < _sizeObstacle; } }

    [Tooltip("The offset after which an obstacle will be generate. " +
        "0 < OffsetObstacle")]
    [Min(1)]
    public int OffsetObstacle; // The times after which an obstacle
                               // will be generated

    private int _offsetObstacle = 1; // The current offset value

    [Header("Long Jump Stage Properties")]
    public Transform LongBouncyStagesAvailable;

    [Tooltip("The probability for generating a long bouncy stage. " +
             "The higher the value the more chance to generate. " +
             "0 = No generation, 1 = All generation")]
    [Range(0f, 1f)]
    public float LongBouncyStageProbability;

    // Flag for checking if Long Bouncy stage
    // generation is still processing
    private bool _isProcessingLongBouncy
    {
        get {
            return LongBouncyStagesAvailable.childCount > 0
                   && _gridGenerator.HasPoints;
        }
    }

    [Header("Stage Link Properties")]
    [Tooltip("The LineRenderer for linking the bouncy stages")]
    public LineRenderer StageLinks;
    private int _linePointerIndex;

    // Stores all the stage object requests
    private List<StageObjectRequest> _stageObjectRequests 
        = new List<StageObjectRequest>();

    private StageObjectRequest _stageObjectRequestCurrent; // Current stage
                                                           // object request

    /// <summary>
    /// For checking if any requests are available, of type bool
    /// </summary>
    private bool _isRequest { get { return _stageObjectRequests.Count != 0; } }

    private bool _isProcessing = false; // Flag to check if a stage object
                                        // generation is being processed

    private bool _isPlaceCharacters = false; // Flag to check 

    public static StageGenerator Instance;

    public Enemy TestEnemy; //<-- DELETE THIS! ENEMIES WILL BE GENERATED FROM
                            //    ENEMY GENERATOR

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
        _gridGenerator = GetComponent<GridGenerator>(); // Setting the grid
                                                        // generator

        CalculateNumberOfLines(); // Calculating the number of points needed
                                  // for line renderer
    }

    void Update()
    {
        // Condition for generating stages
        if (Status == ProcessStatus.Generating)
            GenerationProcess(); // Generating the level
        // Condition for reseting level
        else if (Status == ProcessStatus.Reseting)
            ResetProcess(); // Resetting the level
    }

    /// <summary>
    /// This method is responsible for creating the level.
    /// </summary>
    private void GenerationProcess()
    {
        // Condition to check if level generation is NOT done
        if (_levelCurrent < _level)
        {
            // Checking if stage limit NOT reached
            if (_stageGeneratedCounter < StageNumber)
            {
                if (!_isProcessing) // Checking if no bouncy stage being processed
                {
                    if (!_isRequest) // Checking if no requests available
                    {
                        // Setting a request for bouncy stage objects
                        SetRequest(BouncyStagesAvailable.childCount, 0);
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
        // Condition for processing the long bouncy stage generation
        else if (_isProcessingLongBouncy && LongBouncyStageProbability != 0f)
        {
            if (!_isProcessing) // Checking if no long bouncy stage being processed
            {
                if (!_isRequest) // Checking if no requests available
                {
                    // Setting a request for long bouncy stage object
                    SetRequest(LongBouncyStagesAvailable.childCount, 1);
                }
                else // Requet is available
                {
                    _isProcessing = true; // Processing started
                    ProcessRequests(); // Processing the request
                }
            }
        }
        else // Condition for placing characters and stage generation done
        {
            if (!_isPlaceCharacters) // Characters not placed
            {
                // Starting the 3D text from the last added stage
                Stage3DTextManager.Instance
                    .Generate3DTexts(_currentBouncyStage);

                // Setting up the enemies
                EnemyGenerator.Instance
                    .SetupGeneration(_level, 
                                     _currentBouncyStage.LinkedStage);

                // Setting the player position
                Player.Instance.SetStartPosition(_stagePosition);

                _isPlaceCharacters = true; // Characters placed
            }
            else // Condititon to start the characters
            {
                // Condition for starting the player and the enemies
                if (EnemyGenerator.Instance.Status == ProcessStatus.None)
                {
                    Player.Instance.StartCharacter(); /* <-- This will NOT be called 
                                                         from here. This will be
                                                         called from tapping the
                                                         screen for the first
                                                         time in a gameplay.
                                                         REMOVE LATER!*/

                    EnemyGenerator.Instance.StartEnemy(); /* <-- This will NOT be called
                                                           * from here. This will be 
                                                           * called from tapping the 
                                                           * screen for the first time
                                                           * in a gameplay.
                                                           * REMOVE LATER!
                                                           */
                                                           
                    Status = ProcessStatus.None; // No further stage process needs
                                                 // to be done
                }
            }
        }
    }

    /// <summary>
    /// This method resets the stage.
    /// </summary>
    private void ResetProcess()
    {
        // Condition to check if any stage objects being used
        if (_hasUsedStages)
        {
            // Condition to check if no processing is happening
            // and creating a stage object remove request
            if (!_isProcessing)
            {
                // Condition to request to remove a stage object
                // from index = 1. NOT index = 0 because index = 0
                // is the end platform
                if (!_isRequest) SetRequest(1);
                else // Request is available
                {
                    _isProcessing = true; // Processing started
                    ProcessRequests(); // Processing the request
                }
            }
        }
        // Condition to check if any obstacles being used
        else if (_hasUsedObstacles)
        {
            // Condition to chekf if no processing is happening
            // and creating an obstacle remove request
            if (!_isProcessing)
            {
                // Condition to request to remove an obstacle
                // from index = 0.
                if (!_isRequest) SetRequest(0);
                else // Request is available
                {
                    _isProcessing = true; // Processing started
                    ProcessRequests(); // Processing the request
                }
            }
        }
        // Condition to start generating a new stage
        else
        {
            _isPlaceCharacters = false; // Characters needs to be placed
                                        // again in the new stage
                                        
            IncreaseLevel(); // Increasing the level
            ResetGenerationVariables(); // Resetting all the generating
                                        // variables

            Status = ProcessStatus.Generating; // Starting new stage
                                               // generation process
        }
    }

    /// <summary>
    /// This method adds a request for creating/resetting a stage object.
    /// </summary>
    /// <param name="index">The index of the stage object to create 
    ///                     or to remove, of type int</param>
    private void SetRequest(int index)
    {
        _stageObjectRequests.Add(new StageObjectRequest(index));
    }

    /// <summary>
    /// This method adds a request for creating/resetting a stage object.
    /// </summary>
    /// <param name="size">The number of stage objects available, 
    ///                    of type int</param>
    /// <param name="objectType">The type of object to generate,
    ///                          of type int
    ///                          <para>0 = Bouncy Stage</para>
    ///                          <para>1 = Long Bouncy Stage</para>
    ///                          </param>
    private void SetRequest(int size, int objectType)
    {
        // Adding request
        _stageObjectRequests.Add(new StageObjectRequest(
                                     Random.Range(0, size),
                                     objectType));
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

        // Condition for processing stage generation
        if (Status == ProcessStatus.Generating)
        {
            /*// Getting the current stage object request
            _stageObjectRequestCurrent = _stageObjectRequests[0];
            _stageObjectRequests.RemoveAt(0); // Removing the stage
                                              // object request*/

            // Adding the stage object
            AddStageObject(_stageObjectRequestCurrent.Index,
                           _stageObjectRequestCurrent.ObjectType);

            //_isProcessing = false; // Processing finished
        }
        // Condition for processing stage reset
        else if(Status == ProcessStatus.Reseting)
        {
            // Removing the stage object
            RemoveStageObject(_stageObjectRequestCurrent.Index);
        }

        _isProcessing = false; // Processing finished
    }

    /// <summary>
    /// This method removes a stage object from the used lists.
    /// </summary>
    /// <param name="index">The index of the stage object to be
    ///                     removed,
    ///                     <para>0 = UsedObstacleObject</para>
    ///                     <para>1 = UsedStageObject</para>
    ///                     ,of type int
    ///                     </param>
    private void RemoveStageObject(int index)
    {
        if(index == 0) // Condition to remove obstacle
        {
            
            // Hiding the used obstacle
            _obstaclesUsed[index].gameObject.SetActive(false);
            _obstaclesUsed.RemoveAt(index); // Removing the obstacle
                                            // from used list
        }
        else if(index == 1) // Condition to remove stage object
        {
            // Hiding the stage first
            StageObjectsUsed.GetChild(index).gameObject.SetActive(false);

            // Condition to check if stage is long bouncy stage and
            // setting it to the LongBouncyStagesAvailable list
            if(StageObjectsUsed.GetChild(index).GetChild(0)
                .GetComponent<BouncyStageLong>() != null)
                StageObjectsUsed.GetChild(index).SetParent(LongBouncyStagesAvailable);
            // Condition for stage not being long bouncy stage and
            // setting it to the BouncyStagesAvailable list
            else StageObjectsUsed.GetChild(index).SetParent(BouncyStagesAvailable);
        }
    }

    /// <summary>
    /// This method adds the stage object to the game world.
    /// </summary>
    /// <param name="index">The index of the object requested,
    ///                     of type int</param>
    /// <param name="objectType">The type of object to generate,
    ///                          of type int
    ///                          <para>0 = Bouncy Stage</para>
    ///                          <para>1 = Long Bouncy Stage</para>
    ///                          </param>
    private void AddStageObject(int index, int objectType)
    {
        // Condition to add a bouncy stage and an obstacle
        if (objectType == 0) AddBouncyStage(index);
        // Condition to add a long bouncy stage
        else if (objectType == 1) AddLongBouncyStage(index);
    }

    /// <summary>
    /// This method adds a long bouncy stage object to the game world.
    /// </summary>
    /// <param name="index">The index of the long bouncy stage, 
    ///                     of type int</param>
    private void AddLongBouncyStage(int index)
    {
        // Setting the position of the long bouncy stage
        LongBouncyStagesAvailable.GetChild(index)
            .position = _gridGenerator.GetGridPoint();
        
        // Probability condition for generating a long bouncy stage
        if (Random.Range(0f, 1f) <= LongBouncyStageProbability)
        {
            // Showing the long bouncy stage
            LongBouncyStagesAvailable.GetChild(index).gameObject.SetActive(true);

            // Changing the parent of the long bouncy stage
            LongBouncyStagesAvailable.GetChild(index).SetParent(StageObjectsUsed);
        }
    }

    /// <summary>
    /// This method adds a bouncy stage and obstacle objects to the game world.
    /// </summary>
    /// <param name="index">The index of the bouncy stage, of type int</param>
    private void AddBouncyStage(int index)
    {
        _offsetStageCurrent += OffsetStage; // Getting the new stage
                                            // distance value

        _stagePosition = Vector3.zero; // Resetting the value
                                       // to get accurate
                                       // calculation

        // Condition to change the x-axis position randomly
        if(_stageGeneratedCounter != 0) _offsetSideCurrent += 
                                        Random.Range(
                                            -OffsetSide, 
                                            OffsetSide);

        // Setting up the new stage object position
        _stagePosition.Set(_offsetSideCurrent,
                           _offsetHeightCurrent,
                           _offsetStageCurrent);

        _currentBouncyStage = null; // Removing the previous stage

        // Storing the newly added bouncy stage
        _currentBouncyStage = BouncyStagesAvailable.GetChild(index)
                              .GetChild(0).GetComponent<BouncyStage>();

        // Setting the stage object position
        _currentBouncyStage.transform.parent.position = _stagePosition;

        // Showing the stage object
        _currentBouncyStage.transform.parent.gameObject.SetActive(true);

        // Condition to check if the stage being added is BouncyStageBreakable
        if (_currentBouncyStage.GetComponent<BouncyStageBreakable>() != null)
        {
            // Enabling the script and showing the model
            _currentBouncyStage.gameObject.SetActive(true);

            // Resetting bouncy breakable stage
            _currentBouncyStage.GetComponent<BouncyStageBreakable>().ResetStage();
        }

        // Removing the stage object from the available list
        _currentBouncyStage.transform.parent.SetParent(StageObjectsUsed);

        // Linking the position of the current stage with the previous stage
        _currentBouncyStage.GetComponent<BouncyStage>().LinkedStage =
            StageObjectsUsed.GetChild(StageObjectsUsed.childCount - 2)
            .GetChild(0)
            .GetComponent<BouncyStage>();

        // Setting the the stage number of the stage
        _currentBouncyStage.GetComponent<BouncyStage>().StageNumber 
            = _stageNumberCounter++;

        // Calculating the new position of the current stage, needed for
        // calculating the direction
        _stagePosCurrent.Set(_currentBouncyStage.transform.parent.position.x,
                             0,
                             _currentBouncyStage.transform.parent.position.z);

        // Calculating the new position of the previous stage, needed for
        // calculating the direction
        _stagePosPrevious.Set(StageObjectsUsed.GetChild(StageObjectsUsed.childCount - 2)
                            .transform.position.x,
                            0,
                            StageObjectsUsed.GetChild(StageObjectsUsed.childCount - 2)
                            .transform.position.z);

        // Rotating the current stage to face the previous stage
        _currentBouncyStage.transform.parent.rotation = Quaternion.LookRotation(
                                _stagePosPrevious - _stagePosCurrent);

        // Adding the self and average points
        AddLinkPoint(_currentBouncyStage);

        _stageGeneratedCounter++; // stage object added
    }

    /// <summary>
    /// This method starts a new level generation.
    /// </summary>
    private void GenerateNewLevel()
    {
        _stageGeneratedCounter = 0; // Resetting stage counter for next level

        _levelCurrent++; // Starting new level

        _offsetHeightCurrent += OffsetHeight; // Getting new level height

        // 50% probability to go +ve or -ve in the x-axis
        _offsetSideCurrent += (Random.Range(0, 10) < 5) ? 
                              OffsetSide * -1 : OffsetSide;
        
        // Condition for starting the correction process
        if(_offsetStageCurrent >= _gridGenerator.ActualWorldSize
            || _offsetStageCurrent <= -_gridGenerator.ActualWorldSize)
        {
            OffsetStage = -OffsetStage; // Correction value
            _correctionCounter = 0; // Resetting the correction
                                    // counter
        }

        // Condition for NOT doing the correction process and doing
        // the normal stage generation process
        if (!_isCorrectionProcess)
        {
            // 50% probability to change the direction of the distance offset
            OffsetStage = (Random.Range(0, 10) < 5) ? OffsetStage * -1 : OffsetStage;
        }
        else _correctionCounter++; // Doing the correction process and incrementing
                                   // the counter for the correction process
    }
    
    /// <summary>
    /// This method calculates how many points needed in a LineRenderer.
    /// </summary>
    private void CalculateNumberOfLines()
    {
        StageLinks.positionCount = (2 * (_level * StageNumber)) - 1;
        _linePointerIndex = 0; // Resetting the index
    }

    /// <summary>
    /// This method adds a point to the line renderer.
    /// </summary>
    /// <param name="point">The point to add, of type Vector3</param>
    private void AddLinkPoint(Vector3 point)
    {
        // Adding the line point
        StageLinks.SetPosition(_linePointerIndex, point);

        _linePointerIndex++; // Incrementing for adding
                             // the next point
    }

    /// <summary>
    /// This method adds 1 or 2 points to the line renderer which are
    /// self point and the average point.
    /// </summary>
    /// <param name="stage">The stage from which to add a point, 
    ///                     of type BouncyStage</param>
    private void AddLinkPoint(BouncyStage stage)
    {
        // Condition for adding the average point
        if (StageObjectsUsed.childCount > 2)
        {
            _linePoint = Vector3.zero; // Resetting the line point
            
            // Condition for adding obstacles
            if (_offsetObstacle == OffsetObstacle 
                && _isObstacleAvailable)
            {
                // Calculating the average point
                _linePoint.Set((stage.transform.position.x
                              + stage.LinkedStagePosition.x) / 2,

                              (stage.transform.position.y
                              + stage.LinkedStagePosition.y) / 2,

                              (stage.transform.position.z
                              + stage.LinkedStagePosition.z) / 2);

                // Setting the position
                ObstacleContainer.GetChild(_pointerObstacle)
                    .position = _linePoint;

                // Setting the rotation of the obstacle, the value
                // is taken from the last stage placed because that
                // and the obstacle should have the same rotation
                ObstacleContainer.GetChild(_pointerObstacle)
                    .rotation = StageObjectsUsed
                                .GetChild(StageObjectsUsed.childCount - 1)
                                .transform.rotation;

                // Showing the obstacle
                ObstacleContainer.GetChild(_pointerObstacle)
                    .gameObject.SetActive(true);

                // Storing the obstacle used
                _obstaclesUsed.Add(
                    ObstacleContainer.GetChild(_pointerObstacle));

                _pointerObstacle++; // Pointing to the next obstacle
            }
            else // NOT adding obstacles
            {
                // Calculating the average point
                _linePoint.Set(stage.LinkedStagePosition.x,

                              (stage.transform.position.y
                              + stage.LinkedStagePosition.y) / 2,

                              (stage.transform.position.z
                              + stage.LinkedStagePosition.z) / 2);
            }

            AddLinkPoint(_linePoint); // Adding the average point
        }

        AddLinkPoint(stage.transform.position); // Adding the self point.

        // Incrementing the offset obstacle counter
        _offsetObstacle = _offsetObstacle + 1 > OffsetObstacle ? 
                          1 : 
                          _offsetObstacle + 1;
    }

    /// <summary>
    /// This method increases the level.
    /// </summary>
    private void IncreaseLevel()
    {
        // Calculating the next level
        _level = (_level + 1) > LevelMax ? LevelMax : _level + 1;

        _levelNumberCurrent++;
        Debug.Log("Starting Level: " + _levelNumberCurrent.ToString());
    }

    /// <summary>
    /// This method resets all the variables needed by stage generation
    /// process.
    /// </summary>
    private void ResetGenerationVariables()
    {
        _offsetStageCurrent = 0; // Resetting current distance offset
        _offsetHeightCurrent = 0; // Resetting current Height offset
        _offsetSideCurrent = 0; // Resetting current side offset

        _levelCurrent = 0; // Resetting level counter

        _stageGeneratedCounter = 0; // Resetting number of stages
                                    // Created
        _stageNumberCounter = 1; // Resetting the stage number counter

        _correctionCounter = -1; // Resetting correction counter

        _pointerObstacle = 0; // Resettting the pointer for obstacle
                              // container
        _offsetObstacle = 1;  // Resetting obstacle generator counter

        CalculateNumberOfLines(); // Resetting the LineRenderer

        _gridGenerator.ResetGrid(); // Resetting the GridGenerator
    }

    /// <summary>
    /// This method resets the stage.
    /// </summary>
    public void ResetStage() { Status = ProcessStatus.Reseting; }

    /// <summary>
    /// This struct creates a stage object request.
    /// </summary>
    struct StageObjectRequest
    {
        private int _index; // The index of the stage object

        /// <summary>
        /// The index of the object requested for generating, 
        /// of type int
        /// </summary>
        public int Index { get { return _index; } }

        private int _objectType; // The type of object to generate

        /// <summary>
        /// The type of object to generate, of type int
        /// <para>0 = Bouncy Stage</para>
        /// <para>1 = Long Bouncy Stage</para>
        /// </summary>
        public int ObjectType { get { return _objectType; } }

        /// <summary>
        /// Constructor for creating a stage object request.
        /// </summary>
        /// <param name="index">The index of the object requested,
        ///                     of type int</param>
        public StageObjectRequest(int index) : this(index, -1)
        {
        }

        /// <summary>
        /// Constructor for creating a stage object request.
        /// </summary>
        /// <param name="index">The index of the object requested,
        ///                     of type int</param>
        /// <param name="objectType">The type of object to generate,
        ///                          of type int
        ///                          <para>0 = Bouncy Stage</para>
        ///                          <para>1 = Long Bouncy Stage</para>
        ///                          </param>
        public StageObjectRequest(int index, int objectType)
        {
            _index = index;
            _objectType = objectType;
        }
    }
}
