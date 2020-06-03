using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
/// <summary>
/// Class <c>BasicStage</c> handles all the common functinality between all
/// stages.
/// </summary>
public class BasicStage : MonoBehaviour
{
    //TODO: Add fields for particle effects

    /// <summary>
    /// This method activates the stage action.
    /// </summary>
    public virtual void StageAction()
    {
        //TODO: Activate the particle effect here
        Debug.Log("Particle Effects*");
    }

    /// <summary>
    /// This method activates the stage action.
    /// </summary>
    /// <param name="isActivated">Flag to activate or deactivate an
    ///                           action, of type bool</param>
    public virtual void StageAction(bool isActivated)
    {
        //TODO: Activate the particle effect here
        Debug.Log("Particle Effects*");
    }
}
