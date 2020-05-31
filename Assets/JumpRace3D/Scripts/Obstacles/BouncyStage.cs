using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyStage : MonoBehaviour
{
    [Header("Bouncy Stage Properties")]
    
    public GameObject Booster;
    public Transform Text3DHolder;
    public bool HasText3D { get { return Text3DHolder.childCount > 1; } }
    
    private BouncyStage _linkedStage; // The reference of the linked stage
    public BouncyStage LinkedStage
    {
        get { return _linkedStage; }
        set { _linkedStage = value; }
    }
    
    // Storing the stage number of the stage
    public int StageNumber { get; set; }

    public Vector3 LinkedStagePosition // The position of the linked stage
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
}
