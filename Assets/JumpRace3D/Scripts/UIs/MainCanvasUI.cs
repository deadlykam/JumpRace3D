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
    private MoveUI _loadingUI;

    /// <summary>
    /// Flag checking if loading screen being shown,
    /// of type bool
    /// </summary>
    public bool IsLoadingShown
    { get { return _loadingUI.IsUIShown; } }

    [SerializeField]
    private BasicUI _startUI;

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
}
