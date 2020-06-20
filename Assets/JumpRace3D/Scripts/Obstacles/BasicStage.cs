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
    [Header("Particle Effect Properties")]
    [SerializeField]
    [Tooltip("The type of effect to play when action is called, " +
             "1 = Medium Shockwave, 2 = Small Shockwave, " +
             "3 = Large Shockwave")]
    private int _effectType; // The type of effect to play

    [SerializeField]
    private Vector3 _particleOffset; // The offset position to add
                                     // to the particle position

    /// <summary>
    /// This method activates the stage action.
    /// </summary>
    public virtual void StageAction()
    {
        // Starting a shockwave effect
        ParticleGenerator.Instance
            .AddShockwaveRequest(_effectType,
                                 transform.position + 
                                 _particleOffset);
    }

    /// <summary>
    /// This method activates the stage action.
    /// </summary>
    /// <param name="isActivated">Flag to activate or deactivate an
    ///                           action, of type bool</param>
    public virtual void StageAction(bool isActivated)
    {
        // Starting a shockwave effect
        ParticleGenerator.Instance
            .AddShockwaveRequest(_effectType, 
                                 transform.position + 
                                 _particleOffset);
    }
}
