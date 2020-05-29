using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyStage : MonoBehaviour
{
    [Header("Bouncy Stage Properties")]

    [Tooltip("This is the position of linked stage which is needed" +
             " for character direction calculation")]
    public Vector3 LinkedStage; // The position of the linked stage
}
