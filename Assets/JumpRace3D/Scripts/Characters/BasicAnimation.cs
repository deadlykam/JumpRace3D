using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>BasicAnimation</c> handles all the animation functionality common 
/// between all characters.
/// </summary>
public class BasicAnimation : BasicCharacter
{
    [Header("Basic Animation Properties")]
    [SerializeField]
    private Animator _characterAnimator; // The character animations

    [SerializeField]
    private Transform _model; // The animation model

    [SerializeField]
    private Transform _ragdollModel; // The ragdoll model

    // All children transform in the model
    private List<Transform> _modelChildren = new List<Transform>();

    // All children transform in the ragdoll model
    private List<Transform> _ragdollModelChildren = new List<Transform>();

    [SerializeField]
    private int _fallAnimations; // Number of fall animations

    [SerializeField]
    private int _jumpAnimations; // Number of jump animations
    
    private string _fallSelectParameter = "FallSelect";
    private string _jumpSelectParameter = "JumpSelect";
    private string _triggerJumpParameter = "TriggerJump";
    private string _triggerLandParameter = "TriggerLand";

    // Start is called before the first frame update
    void Start()
    {
        SetupRagDoll();
    }
    
    /// <summary>
    /// This method updates the animation speed with the simulation
    /// speed.
    /// </summary>
    private void UpdateAnimationSpeed()
    {
        // Condition to check if animation speed needs to be updated
        if (_characterAnimator.speed != GameData.Instance.SimulationSpeed)
            _characterAnimator.speed = GameData.Instance.SimulationSpeed;
    }

    /// <summary>
    /// This method is the update method of BasicAnimation and Must be 
    /// called in child class.
    /// </summary>
    protected void UpdateBasicAnimation()
    {
        UpdateBasicCharacter(); // Calling Basic Character update
        UpdateAnimationSpeed(); // Updating the animation speed
    }

    /// <summary>
    /// This method sets up the ragdoll children.
    /// </summary>
    protected void SetupRagDoll()
    {
        // Loop for adding all model children
        foreach(Transform child in _model.GetComponentsInChildren<Transform>())
        {
            _modelChildren.Add(child);
        }

        // Loop for adding all ragdoll children
        foreach (Transform child in _ragdollModel.GetComponentsInChildren<Transform>())
        {
            _ragdollModelChildren.Add(child);
        }
    }
    
    /// <summary>
    /// This method starts or stops the ragdoll of a character.
    /// </summary>
    /// <param name="active">The flag to activate/deactivate a ragdoll,
    ///                      <para>true = activate ragdoll</para>
    ///                      <para>false = deactivate ragdoll</para>
    ///                      of type bool</param>
    protected void SetRagdoll(bool active)
    {
        if (active) // Condition for activating ragdoll
        {
            // Loop for making the ragdoll match the animation from model
            for(int i = 0; i < _modelChildren.Count; i++)
            {
                // Setting ragdoll position to animation position
                _ragdollModelChildren[i].position 
                    = _modelChildren[i].position;

                // Setting ragdoll rotation to animation rotatin
                _ragdollModelChildren[i].rotation 
                    = _modelChildren[i].rotation;

                // Checking if the ragdoll child has rigidbody
                if(_ragdollModelChildren[i]
                    .GetComponent<Rigidbody>() != null)
                {
                    // Removing any velocities from the ragdoll child
                    _ragdollModelChildren[i].GetComponent<Rigidbody>()
                        .velocity = Vector3.zero;
                }
            }
        }

        _ragdollModel.gameObject.SetActive(active); // Activate/deactivate
                                                    // ragdoll

        _model.gameObject.SetActive(!active); // Activate/deactivate model

        mainCollider.enabled = !active; // Activate/deactivate
                                        // main collider
    }

    /// <summary>
    /// This method plays jump animation of the character and selects
    /// a random fall animation as well.
    /// </summary>
    protected void JumpAnimation()
    {
        // Selecting a random jump animation
        _characterAnimator.SetInteger(_jumpSelectParameter, 
                                      Random.Range(
                                          1, _jumpAnimations + 1));

        // Triggering the jump animation
        _characterAnimator.SetTrigger(_triggerJumpParameter);

        FallAnimation(); // Setting a random fall animation to be
                         // played after the jump animation
    }

    /// <summary>
    /// This method plays a selected jump animation and a random
    /// fall animation.
    /// </summary>
    /// <param name="animation">For playing a selected animation,
    ///                         of type int</param>
    protected void JumpAnimation(int animation)
    {
        // Selecting a random jump animation
        _characterAnimator.SetInteger(_jumpSelectParameter, animation);

        // Triggering the jump animation
        _characterAnimator.SetTrigger(_triggerJumpParameter);

        FallAnimation(); // Setting a random fall animation to be
                         // played after the jump animation
    }

    /// <summary>
    /// This method plays a random fall animation of the character.
    /// </summary>
    protected void FallAnimation()
    {
        // Selecting a random fall animation
        _characterAnimator.SetInteger(_fallSelectParameter, 
                                      Random.Range(
                                          1, _fallAnimations + 1));
    }

    /// <summary>
    /// This method plays the end animation for the character when 
    /// the race ends.
    /// </summary>
    protected override void RaceFinished()
    {
        base.RaceFinished();

        // Triggering landing animation
        _characterAnimator.SetTrigger(_triggerLandParameter);
    }

    /// <summary>
    /// This method starts the character and plays a fall animation.
    /// </summary>
    public override void StartCharacter()
    {
        base.StartCharacter();

        SetRagdoll(false); // Resetting the ragdoll <-- Should not be here

        FallAnimation(); // Playing a fall animation
    }

    /// <summary>
    /// This method resets animation to the start animation.
    /// </summary>
    public virtual void ResetAnimaion()
    {
        // Resetting animation back to hover animation
        _characterAnimator.SetInteger(_fallAnimations, 0);
    }
}
