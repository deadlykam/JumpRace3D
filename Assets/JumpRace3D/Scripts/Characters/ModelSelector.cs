using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StageGenerator;

public class ModelSelector : MonoBehaviour
{
    private List<CharacterInfo> _modelsAvailable // List of available models
        = new List<CharacterInfo>();

    private List<CharacterInfo> _modelUsed // List of models being used
        = new List<CharacterInfo>();
    
    // Storing all the model requests.
    private List<ModelRequest> _requestModel = new List<ModelRequest>();

    private ModelRequest _currentRequest; // Storing the current request

    // The current status of ModelSelector
    private ProcessStatus _status = ProcessStatus.None;

    private int _modelIndex; // Storing the index of an available model

    /// <summary>
    /// For checking if any requests are available, of type bool
    /// </summary>
    private bool _isRequest
    { get { return _requestModel.Count != 0; } }

    /// <summary>
    /// Flag that checks if there are NO used models, of type bool
    /// </summary>
    private bool _isUsedEmpty
    { get { return _modelUsed.Count == 0; } }

    public static ModelSelector Instance;

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

    private void Start()
    {
        // Loop for adding all the CharacterInfo
        foreach (Transform child in transform)
            _modelsAvailable.Add(child.GetComponent<CharacterInfo>());
    }

    // Update is called once per frame
    void Update()
    {
        // Condition for generating models
        if (_status == ProcessStatus.Generating)
        {
            if (_isRequest) ProcessRequest(); // Condition for 
                                              // processing a request
        }
        // Condition for resetting models
        else if (_status == ProcessStatus.Resetting)
            ResetModels(); // Resetting model
    }

    /// <summary>
    /// This method processes a request.
    /// </summary>
    private void ProcessRequest()
    {
        _currentRequest = _requestModel[0]; // Storing the current request
        _requestModel.RemoveAt(0);          // Removing the current request

        // Getting a random model to be set to a character
        _modelIndex = Random.Range(0, _modelsAvailable.Count);

        // Setting the character model
        _currentRequest.CharacterAnimation
            .SetupCharacterModel(_modelsAvailable[_modelIndex]);

        // Moving the model to the used list
        _modelUsed.Add(_modelsAvailable[_modelIndex]);

        // Removing the model from the available list
        _modelsAvailable.RemoveAt(_modelIndex);
    }

    /// <summary>
    /// This method resets all the models.
    /// </summary>
    private void ResetModels()
    {
        // Setting the parent back to character model container
        _modelUsed[0].transform.SetParent(transform);

        // Hiding the main model
        _modelUsed[0].gameObject.SetActive(false);

        // Retting the model by showing it
        _modelUsed[0].Model.gameObject.SetActive(true);

        // Resetting the ragdoll model by hiding it
        _modelUsed[0].RagdollModel.gameObject.SetActive(false);

        _modelsAvailable.Add(_modelUsed[0]); // Making model available again
        _modelUsed.RemoveAt(0); // Removing the model from used list

        // Checking if all used models are resetted
        // and stopping the reset process
        if (_isUsedEmpty) _status = ProcessStatus.None;
    }

    /// <summary>
    /// This method adds a new request for character model.
    /// </summary>
    /// <param name="basicAnimation">The character requesting a model, 
    ///                              of type BasicCharacter</param>
    public void AddRequest(BasicAnimation basicAnimation)
    {
        Debug.Log("Requested model: " + basicAnimation.name);
        _requestModel.Add(new ModelRequest(basicAnimation));
    }

    /// <summary>
    /// This method starts the model generating process.
    /// </summary>
    public void StartGenerating() { _status = ProcessStatus.Generating; }

    /// <summary>
    /// This method starts the model reset process.
    /// </summary>
    public void ResetModelSelector()
    {
        _status = ProcessStatus.Resetting; // Starting reset process
        _requestModel.Clear(); // Removing previous model requests.
    }

    /// <summary>
    /// This struct creates a model request.
    /// </summary>
    struct ModelRequest
    {
        // The animation handler of a character
        private BasicAnimation _basicAnimation;

        /// <summary>
        /// Returns the animation handler of a character,
        /// of type BasicAnimation
        /// </summary>
        public BasicAnimation CharacterAnimation
        { get { return _basicAnimation; } }

        /// <summary>
        /// Constructor for creating a model request.
        /// </summary>
        /// <param name="basicAnimation">The animation handler script 
        ///                              of a character, of type
        ///                              BasicCharacter</param>
        public ModelRequest(BasicAnimation basicAnimation)
        {
            _basicAnimation = basicAnimation;
        }
    }
}
