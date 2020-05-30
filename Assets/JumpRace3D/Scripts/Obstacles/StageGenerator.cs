using System.Collections;
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
    private float _offsetHeightCurrent = 0; // Current Height offset

    [Tooltip("The horizontal offset for each stages in the same level.")]
    public float OffsetSide; // Horizontal offset of stages
    private float _offsetSideCurrent; // Current side offset

    private Vector3 _stagePosition; // For storing the new stage object
                                    // position. Needed mainly to avoid
                                    // GC

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

                _isPlaceCharacters = true; // Characters placed
            }
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
            StageObjectsUsed.GetChild(StageObjectsUsed.childCount - 2).position;

        // Adding the self and average points
        AddLinkPoint(StageObjectsUsed.GetChild(StageObjectsUsed.childCount - 1)
            .GetComponent<BouncyStage>());
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

        // 50% probability to change the direction of the distance offset
        OffsetStage = (Random.Range(0, 10) < 5) ? OffsetStage * -1 : OffsetStage;
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

            // Calculating the average point
            _linePoint.Set(stage.LinkedStage.x,
                          (stage.transform.position.y + stage.LinkedStage.y) / 2,
                          (stage.transform.position.z + stage.LinkedStage.z) / 2);

            AddLinkPoint(_linePoint); // Adding the average point
        }

        AddLinkPoint(stage.transform.position); // Adding the self point.
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
