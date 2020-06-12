using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTracker : MonoBehaviour
{
    [Header("Race Tracker Properties")]
    [SerializeField]
    private Transform _leaderCrown;  // The leader crown object
                                          
    private bool _isProcessing = false; // Flag to check if a 
                                        // request is being
                                        // processed

    // List of all the racers in the game playing
    private List<BasicCharacter> _racers = new List<BasicCharacter>();

    // Converting the racers list to array
    private BasicCharacter[] _racersToArray;

    private int _indexCompare; // Index for comparing a character

    private int _indexSearch; // Index for searching a character
                              // to compare to

    private bool _isPlayerShown; // Bool to check if player position
                                 // has been shown or not

    // Storing the character being compared which will be
    // needed for swapping
    private BasicCharacter _currentCharacter;

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
        // Condition to prcess the raceer positions
        if (_isProcessing) ProcessPosition();
    }
    
    /// <summary>
    /// This method processes the racers positions.
    /// </summary>
    private void ProcessPosition()
    {
        _indexCompare = 1; // Starting compare index
        _currentCharacter = null; // Resetting current character
        _isPlayerShown = false; // Player position has not
                                // been shown

        // Loop for going through all the characters
        while(_indexCompare < _racersToArray.Length)
        {
            // Storing current character for comparison
            _currentCharacter = _racersToArray[_indexCompare];

            _indexSearch = _indexCompare - 1; // Getting starting
                                              // index for compare

            // Loop for comparing and checking if the character
            // is less than the current character
            while(_indexSearch >= 0 &&
                  _racersToArray[_indexSearch]
                  .CompareTo(_currentCharacter) == -1)
            {
                // Swaping the characters
                _racersToArray[_indexSearch + 1] 
                    = _racersToArray[_indexSearch];

                _indexSearch--; // Getting another character to
                                // compare to
            }

            // Putting current character at the right order
            _racersToArray[_indexSearch + 1] = _currentCharacter;

            _indexCompare++; // Getting another character to
                             // compare to
        }

        // Checking if models has been assigned to the character
        if (_racersToArray[0].ModelInfo != null)
        {
            // Setting the crown on a new leader
            _racersToArray[0].ModelInfo.SetCrown(_leaderCrown);
        }

        // Loop to show the first 3 character positions including the player
        for(int i = 0; i < _racersToArray.Length; i++)
        {
            // Condition to show first 2 positions
            if (i < 2)
            {
                // Checking if the player is in first 2 positions
                if(!_isPlayerShown &&
                    _racersToArray[i].CharacterName == 
                    GameData.Instance.PlayerName)
                {
                    MainCanvasUI.Instance
                        .SetInGameRacePosition(i, true, 
                        _racersToArray[i].CharacterName);

                    _isPlayerShown = true;
                }
                else // Showing other characters positions
                {
                    //TODO: Show character's name
                    MainCanvasUI.Instance
                        .SetInGameRacePosition(i, false,
                        _racersToArray[i].CharacterName);
                }
            }
            // Condition for showing the player's position if NOT shown
            else if (!_isPlayerShown)
            {
                // Finding the player's character
                if (_racersToArray[i].CharacterName ==
                    GameData.Instance.PlayerName)
                {
                    //TODO: Show player's name
                    MainCanvasUI.Instance
                        .SetInGameRacePosition(i, true,
                        _racersToArray[i].CharacterName);

                    break;
                }
            }
            // Condition for already showing the player's position
            // and now needs to show 1 more character position
            else
            {
                //TODO: Show character's name
                MainCanvasUI.Instance
                        .SetInGameRacePosition(i, false,
                        _racersToArray[i].CharacterName);

                break;
            }
            
        }

        _isProcessing = false; // Processing done
    }

    /// <summary>
    /// This method starts the race position process
    /// </summary>
    public void UpdateRacePosition() { _isProcessing = true; }

    /// <summary>
    /// This method adds a racer to the racer list.
    /// </summary>
    /// <param name="racer">The racer to add, 
    ///                     of type BasicCharacter</param>
    public void AddRacer(BasicCharacter racer) { _racers.Add(racer); }

    /// <summary>
    /// This method initializes the race tracker at the start of a race.
    /// </summary>
    public void StartRaceTracker()
    {
        // Converting all the racers list to array
        _racersToArray = _racers.ToArray();

        UpdateRacePosition(); // Showing the correct position at start
    }

    /// <summary>
    /// This method resets the race tracker to default to be used
    /// for the next race.
    /// </summary>
    public void ResetRaceTracker()
    {
        // Hiding the crown
        _leaderCrown.gameObject.SetActive(false);

        // Removing all the racers from the list
        _racers.Clear();
    }
}
