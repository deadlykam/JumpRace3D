using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BoxCollider))]
public class BouncyStage : BasicStage
{
    [Header("Bouncy Stage Properties")]
    [SerializeField]
    private Animator _stageAnimator; // For controlling animation

    public GameObject Booster;       // For showing/hiding booster
    public Transform Text3DHolder;   // Container to store 3D text

    /// <summary>
    /// Checks if the Transform Text3DHolder has a 3D text,
    /// of type bool
    /// </summary>
    public bool HasText3D { get { return Text3DHolder.childCount > 1; } }
    
    private BouncyStage _linkedStage; // The reference of the linked stage

    // bounce trigger name
    private string _triggerBounce = "TriggerBounce";

    /// <summary>
    /// The linked stage getter/setter, of type BouncyStage
    /// </summary>
    public BouncyStage LinkedStage
    {
        get { return _linkedStage; }
        set { _linkedStage = value; }
    }

    /// <summary>
    /// This flag checks if the stage has animation,
    /// of type bool
    /// </summary>
    private bool _hasAnimation
    { get { return _stageAnimator != null; } }

    void Update()
    {
        UpdateBouncyStage(); // Calling update
    }

    /// <summary>
    /// This method calls the update for BouncyStage class.
    /// </summary>
    protected void UpdateBouncyStage()
    {
        // Checking if the stage has animation and changing the
        // animation speed with the game simulation speed
        if (_hasAnimation)
            _stageAnimator.speed = GameData.Instance.SimulationSpeed;
    }

    /// <summary>
    /// The stage number getter/setter, of type int
    /// </summary>
    public int StageNumber { get; set; }

    /// <summary>
    /// This returns the position of the stage, of type Vector3
    /// </summary>
    public Vector3 StagePosition
    {
        get { return transform.position; }
    }

    /// <summary>
    /// This returns the position of the linked stage, of type Vector3
    /// </summary>
    public Vector3 LinkedStagePosition
    {
        get { return _linkedStage.transform.position; }
    }

    /// <summary>
    /// This method hides/shows the booster.
    /// </summary>
    /// <param name="active">This flag hides or shows the booster,
    ///                      true = show, false = hide,
    ///                      of type bool</param>
    public void SetBooster(bool active) { Booster.SetActive(active); }

    /// <summary>
    /// This method activates the stage action and plays the stage
    /// animation if available.
    /// </summary>
    public override void StageAction()
    {
        base.StageAction();

        // Checking if animation is available
        if (_hasAnimation)
            _stageAnimator.SetTrigger(_triggerBounce); // Playing
                                                       // bounce
                                                       // animation
    }
}
