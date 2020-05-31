using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [depricated] Class <c>BasicStage</c> handles all the common functinality between all
/// stages.
/// </summary>
public class BasicStage : MonoBehaviour
{
    private BasicStage _linkedStage; // The reference of the linked stage
    public BasicStage LinkedStage
    {
        get { return _linkedStage; }
        set { _linkedStage = value; }
    }

    public Vector3 LinkedStagePosition // The position of the linked stage
    {
        get { return _linkedStage.transform.position; }
    }
}
