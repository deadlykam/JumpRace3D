using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetector : MonoBehaviour
{
    [Header("Floor Detector Properties")]
    public LineRenderer FloorLine;

    private Ray ray;        // For creating a ray
    private RaycastHit hit; // For storing hit objects

    // Update is called once per frame
    void Update()
    {
        // Casting a ray downward
        ray = new Ray(transform.position, Vector3.down);

        // Checking if the ray hit anything
        if (Physics.Raycast(ray, out hit))
        {
            // Checking if the RaycastHit has any hit stored
            if (hit.collider != null)
            {
                // Condition for hitting BouncyStage
                if (hit.collider.CompareTag("BouncyStage")
                    || hit.collider.CompareTag("Booster")
                    || hit.collider.CompareTag("EndStage")
                    || hit.collider.CompareTag("LongBouncyStage")
                    || hit.collider.CompareTag("Good"))
                {
                    // Setting the FloorLine
                    SetFloorLine(hit.point, Color.green);
                }
                // Condition for hitting the floor
                else if (hit.collider.CompareTag("Floor"))
                {
                    // Setting the FloorLine
                    SetFloorLine(hit.point, Color.red);
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
    }
}
