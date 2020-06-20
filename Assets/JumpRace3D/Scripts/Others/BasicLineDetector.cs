using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLineDetector : MonoBehaviour
{
    [Header("Basic Line Detector Properties")]
    [SerializeField]
    private string[] _colliderTags; // Tag names of all the
                                    // colliders the ray will
                                    // hit

    private Ray _ray;        // For creating a ray
    private RaycastHit _hit; // For storing hit objects

    private Vector3 _hitPoint; // The point where the ray hit

    /// <summary>
    /// Returns the point of the ray hit, of type Vector3
    /// </summary>
    protected Vector3 hitPoint { get { return _hitPoint; } }

    private bool _isHitCollider; // Flag to check if a collider
                                 // has been hit by the ray

    /// <summary>
    /// Flag that checks if a collider has been hit or not,
    /// of type bool
    /// </summary>
    protected bool isHitCollider { get { return _isHitCollider; } }

    private int _index = 0; // Index to go through all the
                            // colliders

    // Update is called once per frame
    void Update()
    {
        UpdateBasicLineDetector(); // Calling the update method     
    }

    /// <summary>
    /// This method updates the BasicLineDetector
    /// </summary>
    protected void UpdateBasicLineDetector()
    {
        _isHitCollider = false; // Resetting the collision flag

        // Casint a ray downward
        _ray = new Ray(transform.position, Vector3.down);

        // Checking if the ray hit anything
        if (Physics.Raycast(_ray, out _hit))
        {
            // Checknig if the RaycastHit has any hit stored
            if (_hit.collider != null)
            {
                // Loop for finding a collision with the colliders
                for (_index = 0; _index < _colliderTags.Length; _index++)
                {
                    // Condition for hitting a collider
                    if (_hit.collider.CompareTag(_colliderTags[_index]))
                    {
                        _hitPoint = _hit.point; // Storing hit point
                        _isHitCollider = true;  // Ray collided with a
                                                // collider
                    }
                }
            }
        }
    }

    /// <summary>
    /// This method checks if the given collider was hit by the ray.
    /// </summary>
    /// <param name="colliderTag">The collider to check if was hit by the ray,
    ///                        of type string</param>
    /// <returns>Flag to check if the ray hit given collider,
    ///          <para>true = ray hit the collider</para>
    ///          <para>false = ray did NOT hit the collider</para>
    ///          of type bool</returns>
    protected bool RayHitCollision(string colliderTag)
    {
        //return (_hit.collider != null) && (_hit.collider.CompareTag(colliderTag));

        // Condition to check if the ray hit against the given collider
        if((_hit.collider != null) && 
           (_hit.collider.CompareTag(colliderTag)))
        {
            _hitPoint = _hit.point; // Storing hit point
            return true; // Ray hit with the given collider
        }

        return false; // Ray did NOT hit with the given collider
    }
}
