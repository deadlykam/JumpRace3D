using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StageGenerator;

public class EnemyGenerator : MonoBehaviour
{
    public List<Enemy> EnemiesAvailable; // List of available
                                         // enemies

    // List of enemies being used
    private List<Enemy> _enemiesUsed = new List<Enemy>();

    private ProcessStatus _status // Status of the enemy
        = ProcessStatus.None;     // generator

    /// <summary>
    /// Shows the current process status of the EnemyGenerator,
    /// of type ProcessStatus
    /// </summary>
    public ProcessStatus Status { get{ return _status; } }

    public int EnemyMax; // Maximum enemies allowed

    private int _numberOfEnemies; // Number of enemies
                                  // to generate

    private int _processCounter;// Counter that checks
                                // how many enemies
                                // have been generated,
                                // started or resetted

    private int _sizeEnemies // Number of enemies available
    { get { return EnemiesAvailable.Count; } }

    /// <summary>
    /// [depricated]Flag that checks if enemy generation process has
    /// started, of type bool
    /// </summary>
    private bool _isProcess
    { get { return _processCounter < _numberOfEnemies; } }

    private BouncyStage _currentStage; // The stage to generate on

    public static EnemyGenerator Instance;

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        // Condition for generating enemies
        if (_status == ProcessStatus.Generating)
        {
            // Adding the enemies in the game world
            AddEnemy(Random.Range(0, _sizeEnemies));
        }
        // Condition for starting the enemies
        else if(_status == ProcessStatus.Starting)
        {
            // Starting enemy
            _enemiesUsed[_processCounter++].StartCharacter();

            // Condition to stop the starting process
            if (_processCounter >= _enemiesUsed.Count)
                _status = ProcessStatus.None; // Process Done
        }
        // Condition for resetting the enemies
        else if(_status == ProcessStatus.Resetting)
        {
            // Adding the enemy back to availability
            EnemiesAvailable.Add(_enemiesUsed[0]);

            // Hiding the enemy
            _enemiesUsed[0].gameObject.SetActive(false);

            // Removing the enemy from used list
            _enemiesUsed.RemoveAt(0);

            // Condition to check if Reset process done
            if (_enemiesUsed.Count == 0)
                _status = ProcessStatus.None; // Process Done
        }
    }
    
    /// <summary>
    /// This method adds an enemy to the game world.
    /// </summary>
    /// <param name="index">The index of the enemy to add,
    ///                     of type int</param>
    private void AddEnemy(int index)
    {
        // Showing the enemy
        EnemiesAvailable[index]
            .transform.gameObject.SetActive(true);

        // Getting enemy model
        EnemiesAvailable[index].GetCharacterModel();

        // Setting the enemy's position
        EnemiesAvailable[index]
            .SetStartPosition(_currentStage.StagePosition);

        // Adding the enemy in the used list
        _enemiesUsed.Add(EnemiesAvailable[index]);

        // Removing the enemy from the available list
        EnemiesAvailable.RemoveAt(index);

        // Getting the next linked stage
        _currentStage = _currentStage.LinkedStage;

        // Incrementing the generated counter
        _processCounter++;

        // Condition for finishing adding all the enemies
        if (_processCounter >= _numberOfEnemies)
            _status = ProcessStatus.None; // Process Done
    }

    /// <summary>
    /// This method sets up the initial setup for the enemy generation.
    /// </summary>
    /// <param name="numberOfEnemies">The number of enemies to generate,
    ///                               of type int</param>
    /// <param name="stage">The stage from which to start to put enemies
    ///                     on, of type BouncyStage</param>
    public void SetupGeneration(int numberOfEnemies, BouncyStage stage)
    {
        // Correcting the number of enemies generation
        // if any errors found
        _numberOfEnemies = numberOfEnemies >= EnemyMax ? 
                            EnemyMax : 
                            numberOfEnemies;

        _currentStage = stage; // Setting the current stage

        _processCounter = 0; // Resetting the process counter

        _status = ProcessStatus.Generating; // Starting to add
                                           // enemies
    }

    /// <summary>
    /// This method starts up all the enemies added to the game world.
    /// </summary>
    public void StartEnemy()
    {
        _processCounter = 0; // Resetting the process counter

        _status = ProcessStatus.Starting; // Starting to start
                                          // enemies
    }

    /// <summary>
    /// This method starts the reset process of EnemyGenerator.
    /// </summary>
    public void ResetEnemy()
    {
        _processCounter = 0; // Resetting the process counter

        _status = ProcessStatus.Resetting; // Start the reset
                                          // process
    }
}
