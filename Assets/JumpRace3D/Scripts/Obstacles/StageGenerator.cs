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

    public int StageNumber; // The starting stage number;

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
        // Checking if stage limit not reached
        if (_stageGeneratedCounter < StageNumber)
        {
            if (!_isProcessing) // Checking if no stage object being processed
            {
                if (!_isRequest) // Checking if no requests available
                    SetRequest();// Getting a request
                else // Request is available
                {
                    _isProcessing = true; // Processing started
                    ProcessRequests(); // Processing the request
                }
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
        BouncyStagesAvailable.GetChild(index).gameObject.SetActive(true);
        BouncyStagesAvailable.GetChild(index).SetParent(StageObjectsUsed);
        //TODO: move the stage object
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
