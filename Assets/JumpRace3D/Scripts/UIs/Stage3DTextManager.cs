using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3DTextManager : MonoBehaviour
{
    [Header("Stage 3D Text Manager Properties")]
    public Transform Text3DHolder; // The container that holds
                                   // all the 3D texts at the
                                   // start of the game

    /*private Transform[] Text3DCointainer; // Containing all the 
                                          // 3D Texts container

    private TextMeshPro[] Text3Ds; // Containing all the 3D Texts*/

    private Text3D[] _Text3Ds; // Containing all the 3D Texts

    private int _text3DPointer = 0; // Pointer to help place the
                                    // 3D texts

    // The size of the 3D text container
    private int _size { get { return _Text3Ds.Length; } }

    // The flag to check if the process has started
    private bool _isProcess { get { return _text3DPointer < _size; } }

    private BouncyStage _stageCurrent; // Current stage to place
                                       // a 3D text to

    public static Stage3DTextManager Instance;

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
        _Text3Ds = new Text3D[Text3DHolder.childCount]; // Resizing the
                                                        // 3D text holder
                                                        
        _text3DPointer = _size; // Stopping the process

        // Loop for storing all the 3D texts
        // for future use
        for (int i = 0; i < _size; i++)
            _Text3Ds[i] = Text3DHolder.GetChild(i)
                            .GetComponent<Text3D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isProcess) // Condition for 3D text generation processing
        {
            // Setting the parent of the 3D text
            _Text3Ds[_text3DPointer].transform.SetParent(_stageCurrent.Text3DHolder);

            // Re-positioning the 3D text to 0
            _Text3Ds[_text3DPointer].transform.localPosition = Vector3.zero;

            // Re-rotating the 3D text to 0 degrees
            _Text3Ds[_text3DPointer].transform.localRotation = Quaternion.identity;

            // Setting up the stage number of the stage
            _Text3Ds[_text3DPointer].SetText("" + _stageCurrent.StageNumber);

            // Getting the next stage
            _stageCurrent = _stageCurrent.LinkedStage;

            // Condition to check if the next platform is the
            // end platform then stoping the 3D text generation
            if(_stageCurrent.LinkedStage == null)
            {
                _text3DPointer = _size; // Stopping future process
                return; // Exiting process
            }

            _text3DPointer++; // Going to next process
        }
    }

    /// <summary>
    /// This method sets up the conditions needed to start
    /// placing 3D texts to the stages.
    /// </summary>
    /// <param name="stage">The stage from which 3D texts should 
    ///                     start to be placed, of type 
    ///                     BouncyStage</param>
    public void Generate3DTexts(BouncyStage stage)
    {
        _stageCurrent = stage; // Setting the current stage
        _text3DPointer = 0;    // Starting the 3D placement
                               // process
    }
}
