using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>GameData</c> contains data that will be used by other classes.
/// </summary>
public class GameData : MonoBehaviour
{
    [Tooltip("The y-axis value limit for objects to fall to.")]
    public float FallHeightLimit; // The limit of fall distance

    // TODO: Give fields for simulation speed

    public static GameData Instance;

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
        
    }
}
