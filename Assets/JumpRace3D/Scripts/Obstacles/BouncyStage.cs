using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyStage : MonoBehaviour
{
    [Header("Bouncy Stage Properties")]
    
    public GameObject Booster;

    private Vector3 _linkedStage; // The position of the linked stage
    public Vector3 LinkedStage
    {
        get { return _linkedStage; }
        set { _linkedStage = value; }
    }

    /// <summary>
    /// This method hides/shows the booster.
    /// </summary>
    /// <param name="active">This flag hides or shows the booster,
    ///                      true = show, false = hide,
    ///                      of type bool</param>
    public void SetBooster(bool active) { Booster.SetActive(active); }
}
