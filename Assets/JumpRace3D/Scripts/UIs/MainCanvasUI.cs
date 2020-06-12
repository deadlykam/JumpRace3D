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
    private PopupUI _popupUI; // The popup message

    [SerializeField]
    private MoveUI _loadingUI; // The loading screen

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
    /// This method starts the game.
    /// </summary>
    public void StartGame()
    {
        //TODO: Start the game from here
        SetStartUI(false);

        EnemyGenerator.Instance.StartEnemy(); // Starting the enemies
        Player.Instance.StartCharacter(); // Starting the player
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
    /// This method shows the popup
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
