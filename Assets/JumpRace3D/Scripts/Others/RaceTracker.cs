using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTracker : MonoBehaviour
{
    [Header("Race Tracker Properties")]
    [SerializeField]
    private Transform _leaderCrown;  // The leader crown object

    [SerializeField]
    private Vector3 _crownOffset; // The offset value of the crown
                                  // when placed inside a racer

    private int _leaderRacePosition = -1; // The current position 
                                          // of the leader in a 
                                          // race

    // Storing all the race position requests
    private List<RacePositionRequest> _racePositionRequests 
        = new List<RacePositionRequest>();

    private RacePositionRequest _currentRequest; // Current race
                                                 // position
                                                 // request

    /// <summary>
    /// For checking if any requests are available, of type bool
    /// </summary>
    private bool _isRequest
    { get { return _racePositionRequests.Count != 0; } }

    private bool _isProcessing = false; // Flag to check if a 
                                        // request is being
                                        // processed

    public static RaceTracker Instance; // Singleton


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
        if (_isRequest) // Condition to check if any request available
        {
            if (!_isProcessing) // Checking if any process going on
            {
                _isProcessing = true; // Starting process
                ProcessRequest();     // Processing reqest
            }
        }
    }

    /// <summary>
    /// This method processes the request and sets up the leader and
    /// it's crown.
    /// </summary>
    private void ProcessRequest()
    {
        // Storing the first request
        _currentRequest = _racePositionRequests[0];

        // Removing the first request
        _racePositionRequests.RemoveAt(0);

        // Condition to check if a new leader emerged from the
        // race
        if(_leaderRacePosition == -1 || 
           _currentRequest.RacePosition < _leaderRacePosition)
        {
            // Setting the new leader position
            _leaderRacePosition = _currentRequest.RacePosition;

            // Setting the crown on a new leader
            _currentRequest.RacerModel
                .SetCrown(_leaderCrown, _crownOffset);
        }

        _isProcessing = false; // Process done
    }

    /// <summary>
    /// This method adds a new request for race position.
    /// </summary>
    /// <param name="racePosition">The position of the racer, of type
    ///                            int</param>
    /// <param name="racerModel">The model of the racer, of type
    ///                          CharacterInfo</param>
    public void AddRequest(int racePosition, CharacterInfo racerModel)
    {
        // Creating a new request and adding it to the request list
        _racePositionRequests.Add(new RacePositionRequest(
                                    racePosition,
                                    racerModel));
    }

    /// <summary>
    /// This method resets the race tracker to default to be used
    /// for the next race.
    /// </summary>
    public void ResetRaceTracker()
    {
        // Hiding the crown
        _leaderCrown.gameObject.SetActive(false);

        // Resetting the leader position
        _leaderRacePosition = -1;

        // Remvoing all requests from the list
        _racePositionRequests.Clear();
    }

    /// <summary>
    /// This struct creates a race position request.
    /// </summary>
    struct RacePositionRequest
    {
        private int _racePosition; // The current position of the 
                                   // racer in a race

        /// <summary>
        /// The race position of the race, of type int
        /// </summary>
        public int RacePosition { get { return _racePosition; } }

        private CharacterInfo _racerModel; // The transform of the
                                           // racer model if crown
                                           // needs to be exchanged

        /// <summary>
        /// The transform of the racer model, of type CharacterInfo
        /// </summary>
        public CharacterInfo RacerModel { get { return _racerModel; } }

        /// <summary>
        /// Constructor for creating a race position request.
        /// </summary>
        /// <param name="racePosition">The position of the racer in a
        ///                            race, of type int</param>
        /// <param name="racerModel">The model of the racer, of type
        ///                          CharacterInfo</param>
        public RacePositionRequest(int racePosition, CharacterInfo racerModel)
        {
            _racePosition = racePosition;
            _racerModel = racerModel;
        }
    }
}
