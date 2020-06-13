using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [Header("Character Info Properties")]
    [SerializeField]
    private Transform _model; // The character model

    /// <summary>
    /// Returns the model object, of type Transform
    /// </summary>
    public Transform Model { get { return _model; } }

    [SerializeField]
    private Transform _ragdollModel; // The ragdoll associated
                                     // with this model

    /// <summary>
    /// Returns the ragdoll model object, of type Transform
    /// </summary>
    public Transform RagdollModel { get { return _ragdollModel; } }

    [SerializeField]
    private Transform _headBone; // The location of the
                                 // head bone

    [SerializeField]
    private Animator _characterAnimator; // The animator of the
                                         // character

    /// <summary>
    /// Returns the animator of the character, of type Animator
    /// </summary>
    public Animator CharacterAnimator
    { get { return _characterAnimator; } }

    /// <summary>
    /// This method sets the crown on the character model.
    /// </summary>
    /// <param name="crown">The crown to be set, of type Transform</param>
    public void SetCrown(Transform crown)
    {
        SetCrown(crown, Vector3.zero); // Setting the crown
    }

    /// <summary>
    /// This method sets the crown on the character model.
    /// </summary>
    /// <param name="crown">The crown to be set, of type Transform</param>
    /// <param name="offset">The offset of the crown from the head,
    ///                      of type Vector3</param>
    public void SetCrown(Transform crown, Vector3 offset)
    {
        // Showing the crown if NOT shown
        if (!crown.gameObject.activeSelf)
            crown.gameObject.SetActive(true);

        crown.SetParent(_headBone);   // Putting the crown on
                                      // the leader

        crown.localPosition = offset; // Setting the crown
                                      // offset

        // Resetting the crown rotation
        crown.localRotation = Quaternion.identity;
    }
}
