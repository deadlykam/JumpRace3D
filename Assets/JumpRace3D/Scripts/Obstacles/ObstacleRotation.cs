using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotation : MonoBehaviour
{
    [Header("Obstacle Rotation Properties")]
    [Tooltip("This is both the rotation speed and direction.")]
    public Vector3 RotationDirection; // The rotation speed and
                                      // direction for the
                                      // obstacle

    // Update is called once per frame
    void Update()
    {
        // Continuously rotating the obstacle
        transform.Rotate(RotationDirection * Time.deltaTime);
    }
}
