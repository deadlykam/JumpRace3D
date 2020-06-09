using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [Header("Character Info Properties")]
    [SerializeField]
    private Transform _ragdollModel; // The ragdoll associated
                                     // with this model

    [SerializeField]
    private Transform _headBone; // The location of the
                                 // head bone

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
