using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasUI : MonoBehaviour
{
    [Header("Main Canvas UI Properties")]
    [SerializeField]
    private RectTransform _mainCanvasRect;

    /// <summary>
    /// Returns the RectTransform of the main canvas,
    /// of type RectTransform
    /// </summary>
    public RectTransform MainCanvasRect
    { get { return _mainCanvasRect; } }

    [SerializeField]
    private PlayerUI _playerUI; // The player UI screen

    [SerializeField]
    private EndScreenUI _endScreenUI; // The end screen UI

    [SerializeField]
    private PopupUI _popupUI; // The popup message

    [SerializeField]
    private MoveUI _loadingUI; // The loading screen

    [SerializeField]
    private LoadUI _loadingBar; // The loading bar

    /// <summary>
    /// Flag checking if loading screen being shown,
    /// of type bool
    /// </summary>
    public bool IsLoadingShown
    { get { return _loadingUI.IsUIShown; } }

    [SerializeField]
    private BasicUI _startUI; // The start screen

    public static MainCanvasUI Instance;

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

    /// <summary>
    /// This method shows or hides the loading UI.
    /// </summary>
    /// <param name="active">Flag to show/hide loading UI,
    ///                      <para>true = show loading UI</para>
    ///                      <para>false = hide loading UI</para>
    ///                      of type bool
    ///                      </param>
    public void SetLoadingUI(bool active)
    {
        if (active) _loadingUI.ShowUI(); // Showing loading UI
        else _loadingUI.HideUI();        // Hiding loading UI
    }

    /// <summary>
    /// This method shows or hides the start UI.
    /// </summary>
    /// <param name="active">Flag to show/hide start UI,
    ///                      <para>true = show start UI</para>
    ///                      <para>false = hide start UI</para>
    ///                      of type bool
    ///                      </param>
    public void SetStartUI(bool active)
    {
        if (active) _startUI.ShowUI(); // Showing start UI
        else _startUI.HideUI();        // Hiding start UI
    }

    /// <summary>
    /// This method shows or hides the end screen UI.
    /// </summary>
    /// <param name="active">Flag to show/hide end screen UI,
    ///                      <para>true = show end UI</para>
    ///                      <para>false = hide end UI</para>
    ///                      of type bool</param>
    public void SetEndScreenUI(bool active)
    {
        if (active) _endScreenUI.ShowUI(); // Showing end screen
        else _endScreenUI.HideUI();        // Hiding end screen
    }

    /// <summary>
    /// This method sets the end screen position UI.
    /// </summary>
    public void SetEndScreenPosition() { _endScreenUI.SetUI(); }

    /// <summary>
    /// This method shows the player UI.
    /// </summary>
    /// <param name="active">Flag to show/hide player UI,
    ///                      <para>true = show player UI</para>
    ///                      <para>false = hide player UI</para>
    ///                      of type bool</param>
    public void SetPlayerUI(bool active)
    {
        if (active) _playerUI.ShowUI(); // Showing player UI
        else _playerUI.HideUI();        // Hiding player UI
    }

    /// <summary>
    /// This method starts the game.
    /// </summary>
    public void StartGame()
    {
        SetStartUI(false);

        SetPlayerUI(true); // Showing the player UI

        EnemyGenerator.Instance.StartEnemy(); // Starting the enemies
        Player.Instance.StartCharacter(); // Starting the player

        AudioManager.Instance.PlayWind(); // Starting wind sfx
    }

    /// <summary>
    /// This method starts the generation of next stage.
    /// </summary>
    public void StartNextLevel()
    {
        _endScreenUI.DisableTrigger(); // Disabling the trigger event UI

        // Showing the loading screen
        SetLoadingUI(true);

        //_playerUI.HideUI(); // Hiding the player UI

        // Setting progress bar back to 0%
        SetLoadingBar(0);

        StageGenerator.Instance.ResetStage(); // Resetting the stage
                                              // and starting a new
                                              // stage
    }

    /// <summary>
    /// This method sets the stage numbers of the Player UI.
    /// </summary>
    /// <param name="stageNumber">The current stage number,
    ///                           of type int</param>
    public void SetStageNumber(int stageNumber)
    {
        _playerUI.SetStageNumbers(stageNumber);
    }

    /// <summary>
    /// This method sets the bar fill amount.
    /// </summary>
    /// <param name="percentage">The bar fill amount in percentage,
    ///                          of type float</param>
    public void SetBar(float percentage)
    {
        _playerUI.SetBar(percentage);
    }

    /// <summary>
    /// This method shows the popup.
    /// </summary>
    /// <param name="text">The text of the popup, of type string</param>
    /// <param name="colour1">The colour of the first text,
    ///                       of type Color</param>
    /// <param name="colour2">The colour of the second text,
    ///                       of type Colour</param>                      
    public void StartPopup(string text, Color colour1, Color colour2)
    {
        // Setting popup text and colours
        _popupUI.SetText(text, colour1, colour2);
        _popupUI.ShowPopup(); // Showing the popup
    }

    /// <summary>
    /// This method checks if the loading screen has slid in.
    /// </summary>
    /// <returns>Flag that checks if the loading screen has slid in,
    ///          <para>true = has slid in</para>
    ///          <para>false = has NOT slid in</para>
    ///          of type bool</returns>
    public bool IsLoadingScreenSlidIn() { return _loadingUI.IsSlideIn; }

    /// <summary>
    /// Setting the loading bar progress.
    /// </summary>
    /// <param name="amount">The progress value to set between 0 - 1,
    ///                      of type float</param>
    public void SetLoadingBar(float amount) => _loadingBar.SetPercentage(amount);

    /// <summary>
    /// This method checks if the current progress has not been done.
    /// </summary>
    /// <param name="amount">The progress amount to check if NOT done,
    ///                      of type float</param>
    /// <returns>True means progress NOT done, fales otherwise, 
    ///          of type bool</returns>
    public bool CheckLoadingBar(float amount)
    {
        return _loadingBar.CheckProgress(amount);
    }

    /// <summary>
    /// This method sets the race position text.
    /// </summary>
    /// <param name="racePosition">The race position of the character,
    ///                            of type int</param>
    /// <param name="isPlayer">Flag to check if the character is the 
    ///                        player, of type bool</param>
    /// <param name="name">The name of the character, of type string</param>
    public void SetInGameRacePosition(int racePosition, bool isPlayer, string name)
    {
        _playerUI.SetInGameRacePosition(racePosition, isPlayer, name);
    }
}
