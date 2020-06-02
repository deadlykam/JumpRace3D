using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>StageGenerator</c> handles all the functionality needed to create 
/// an imaginary grid world.
/// </summary>
public class GridGenerator : MonoBehaviour
{
    [Header("Grid Generator Properties")]
    [SerializeField]
    private float WorldSize; // The size of the game world

    [Tooltip("The amount to reduce the world size for the stage " +
             "generator. WorldSizeReduce < WorldSize")]
    public float WorldSizeReduce; // The amount to reduce the world
                                  // size for the StageGenerator

    /// <summary>
    /// The actual world size for generating bouncy stages.
    /// </summary>
    public float ActualWorldSize
    { get { return WorldSize - WorldSizeReduce; } }

    public float CentrePointOffset; // Offsets of the
                                    // centre point

    [Tooltip("This is used for shifting the centre point " +
        "around randomly, 0 = no shifting, 1 = chance of " +
        "shifting 1 offset value.")]
    [Range(0f, 1f)]
    public float CentreNoise; // For giving random centre
                              // point for the grid centre
                              // point

    // Flag to check if done using all the grid points
    public bool IsDone { get { return _currentZ > WorldSize; } }
    
    // Flag checking if any grid points available
    public bool HasPoints { get { return _currentZ <= WorldSize; } }

    private float _currentX; // The current x-axis value
    private float _currentZ; // The current z-axis value
    private Vector3 _gridPoint; // For storing and sending
                                // a new grid point, needed
                                // mainly for GC

    // Getting the centre point value
    private float _centrePointValue
    { get { return -WorldSize + CentrePointOffset; } }

    // Start is called before the first frame update
    void Start()
    {
        ResetGrid(); // Setting up the grid for 
                     // the first time
    }

    /// <summary>
    /// This method sets up a new centre point.
    /// </summary>
    private void UpdateCentrePoint()
    {
        // Getting the new x-axis point.
        // Equation:
        //  Pnx = P(n - 1)x + (4 x Offset), if Pnx > Size then reset.
        _currentX = (_currentX + (4 * CentrePointOffset)) > WorldSize ?
                        _centrePointValue :
                        _currentX + (4 * CentrePointOffset);

        // Condition to check if to get new z-axis point
        if(_currentX == _centrePointValue)
        {
            /* Getting the new z-axis point.
               Equation:
                Pnz = P(n - 1)z + (4 x Offset).

               Not resetting Pnz after (Pnz > Size)
               because using the value (Pnz > Size)
               to determine if the grid is finished
               or not
             */
            _currentZ = _currentZ + (4 * CentrePointOffset);
        }
    }

    /// <summary>
    /// This method resets the grid point to the first grid point.
    /// </summary>
    public void ResetGrid()
    {
        // Setting the first x-axis centre point
        _currentX = _centrePointValue;

        // Setting the first z-axis centre point
        _currentZ = _centrePointValue;
    }

    /// <summary>
    /// This method gets a new grid centre point.
    /// </summary>
    /// <returns>The new grid centre point, of 
    ///          type Vector3</returns>
    public Vector3 GetGridPoint()
    {
        _gridPoint = Vector3.zero; // resting the point's values

        // Setting the new grid point
        _gridPoint.Set(_currentX + (CentreNoise != 0 ?
                                    CentrePointOffset * Random.Range(
                                        -CentreNoise,
                                         CentreNoise)
                                         : 0),
                       0,
                       _currentZ + (CentreNoise != 0 ?
                                    CentrePointOffset * Random.Range(
                                        -CentreNoise,
                                         CentreNoise)
                                         : 0));

        UpdateCentrePoint(); // Setting new centre point

        return _gridPoint; // Sending the new grid point
    }
}
