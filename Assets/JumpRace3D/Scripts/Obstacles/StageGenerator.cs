using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridGenerator))]

/// <summary>
/// Class <c>StageGenerator</c> handles all the functionality for creating a stage, 
/// is responsible for placing all the objects needed for a stage.
/// </summary>
public class StageGenerator : MonoBehaviour
{
    [Header("Stage Properties")]
    public Transform BouncyStagesAvailable; // Transform containing all
                                            // the bouncy stages

    public Transform StageObjectsUsed;      // Transform containing all
                                            // the used objects

    private GridGenerator _gridGenerator;   // Containing all the grid
                                            // points in the game world

    [Tooltip("The amount of distance between each stages.")]
    public float OffsetStage; // Distance between each stages
    private float _offsetStageCurrent = 0; // Current Distance offset

    [Tooltip("The stage height offset when going up.")]
    public float OffsetHeight; // Height offset of stages
    private float _offsetHeightCurrent = 0; // Current Height offset

    [Tooltip("The horizontal offset for each stages in the same level.")]
    public float OffsetSide; // Horizontal offset of stages
    private float _offsetSideCurrent; // Current side offset

    private Vector3 _stagePosition; // For storing the calculated stage
                                    // object position. Needed mainly to
                                    // avoid GC

    [Tooltip("The number of levels during a gameplay, Level != 0")]
    [Min(1)]
    public int Level; // The number of levels during a gameplay
    private int _levelCurrent = 0; // Current levels generated
    private Vector3 _linePoint; // For storing the current line point.
                                // Needed mainly to avoid GC
    
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

    // For checking if any requests are available
    private bool _isRequest { get { return _stageObjectRequests.Count != 0; } }

    private bool _isProcessing = false; // Flag to check if a stage object
                                        // generation is being processed

    private bool _isPlaceCharacters = false; // Flag to check 
    
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
        // Condition to check if level generation is NOT done
        if (_levelCurrent < Level)
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
                // Setting the player position
                Player.Instance.SetStartPosition(_stagePosition);
                Player.Instance.StartCharacter(); /* <-- This will NOT be called 
                                                         from here. This will be
                                                         called from tapping the
                                                         screen for the first
                                                         time in a gameplay.
                                                         REMOVE LATER!*/

                //TODO: Set the enemy characters here

                /*// Starting the 3D text from the starting stage
                Stage3DTextManager.Instance
                    .Generate3DTexts(StageObjectsUsed.GetChild(
                        StageObjectsUsed.childCount - 1)
                        .GetComponent<BouncyStage>());*/

                _isPlaceCharacters = true; // Characters placed
            }
        }

    }

    /// <summary>
    /// [depricated] This method adds a request for creating a stage object.
    /// </summary>
    private void SetRequest()
    {
        // Adding request
        _stageObjectRequests.Add(new StageObjectRequest(
                                     Random.Range(0,
                                     BouncyStagesAvailable.childCount),
                                     0));
    }

    /// <summary>
    /// This method adds a request for creating a stage object.
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

        // Adding the stage object
        AddStageObject(_stageObjectRequestCurrent.Index,
                       _stageObjectRequestCurrent.ObjectType);
        //AddBouncyStage(_stageObjectRequestCurrent.Index);

        //_stageGeneratedCounter++; // stage object added
        _isProcessing = false; // Processing finished
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

        // Setting the stage object position
        BouncyStagesAvailable.GetChild(index).position = _stagePosition;

        // Showing the stage object
        BouncyStagesAvailable.GetChild(index).gameObject.SetActive(true);

        // Removing the stage object from the available list
        BouncyStagesAvailable.GetChild(index).SetParent(StageObjectsUsed);
        
        // Linking the position of the current stage with the previous stage
        StageObjectsUsed.GetChild(StageObjectsUsed.childCount - 1)
            .GetComponent<BouncyStage>().LinkedStage =
            StageObjectsUsed.GetChild(StageObjectsUsed.childCount - 2)
            .GetComponent<BouncyStage>();

        // Setting the the stage number of the stage
        StageObjectsUsed.GetChild(StageObjectsUsed.childCount - 1)
            .GetComponent<BouncyStage>().StageNumber = _stageNumberCounter++;

        // Calculating the new position of the current stage, needed for
        // calculating the direction
        _stagePosCurrent.Set(StageObjectsUsed.GetChild(StageObjectsUsed.childCount - 1)
                            .transform.position.x,
                            0,
                            StageObjectsUsed.GetChild(StageObjectsUsed.childCount - 1)
                            .transform.position.z);

        // Calculating the new position of the previous stage, needed for
        // calculating the direction
        _stagePosPrevious.Set(StageObjectsUsed.GetChild(StageObjectsUsed.childCount - 2)
                            .transform.position.x,
                            0,
                            StageObjectsUsed.GetChild(StageObjectsUsed.childCount - 2)
                            .transform.position.z);

        // Rotating the current stage to face the previous stage
        StageObjectsUsed.GetChild(StageObjectsUsed.childCount - 1)
                            .transform.rotation = Quaternion.LookRotation(
                                _stagePosPrevious - _stagePosCurrent);

        // Adding the self and average points
        AddLinkPoint(StageObjectsUsed.GetChild(StageObjectsUsed.childCount - 1)
            .GetComponent<BouncyStage>());

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
        StageLinks.positionCount = (2 * (Level * StageNumber)) - 1;
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
