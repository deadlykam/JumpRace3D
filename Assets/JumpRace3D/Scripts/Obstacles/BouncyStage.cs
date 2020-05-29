using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyStage : MonoBehaviour
{
    [Header("Bouncy Stage Properties")]

    [Tooltip("[depricated]This is the position of linked stage which is needed" +
             " for character direction calculation")]
    public Vector3 LinkedStage; // The position of the linked stage

    public GameObject Booster;

    /// <summary>
    /// This method hides/shows the booster.
    /// </summary>
    /// <param name="active">This flag hides or shows the booster,
    ///                      true = show, false = hide,
    ///                      of type bool</param>
    public void SetBooster(bool active) { Booster.SetActive(active); }
}
