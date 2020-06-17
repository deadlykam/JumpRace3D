using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetector : MonoBehaviour
{
    [Header("Floor Detector Properties")]
    public LineRenderer FloorLine;

    [SerializeField]
    private Transform _lineBottom; // Object to show at the bottom
                                  // of the line

    private Ray _ray;        // For creating a ray
    private RaycastHit _hit; // For storing hit objects

    // Update is called once per frame
    void Update()
    {
        // Casting a ray downward
        _ray = new Ray(transform.position, Vector3.down);

        // Checking if the ray hit anything
        if (Physics.Raycast(_ray, out _hit))
        {
            // Checking if the RaycastHit has any hit stored
            if (_hit.collider != null)
            {
                // Condition for hitting BouncyStage
                if (_hit.collider.CompareTag("BouncyStage")
                    || _hit.collider.CompareTag("Booster")
                    || _hit.collider.CompareTag("EndStage")
                    || _hit.collider.CompareTag("LongBouncyStage")
                    || _hit.collider.CompareTag("Good"))
                {
                    // Setting the FloorLine
                    SetFloorLine(_hit.point, Color.green);
                }
                // Condition for hitting the floor
                else if (_hit.collider.CompareTag("Floor"))
                {
                    // Setting the FloorLine
                    SetFloorLine(_hit.point, Color.red);
                }
            }
        }
    }

    /// <summary>
    /// This method sets up the FloorLine LineRenderer.
    /// </summary>
    /// <param name="hitPoint">The point at which the FloorLine should touch,
    ///                        of type Vector3</param>
    /// <param name="colour">The colour of the LineRenderer, 
    ///                      of type Color</param>
    private void SetFloorLine(Vector3 hitPoint, Color colour)
    {
        // Setting the colour of the line
        FloorLine.startColor = colour;
        FloorLine.endColor = colour;

        // Setting the starting point of the line
        FloorLine.SetPosition(0, transform.position);

        // Setting the ending point of the line
        FloorLine.SetPosition(1, hitPoint);

        // Setting the position of the line end object
        _lineBottom.position = hitPoint;
    }
}
