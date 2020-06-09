using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyStageMove : BouncyStage
{
    [Header("Movement Properties")]
    public float Speed; // The movement speed of the stage

    [Tooltip("The distance the stage will move to. Distance >= 1")]
    [Min(1)]
    public float Distance; // The movement limit of the stage

    // Start is called before the first frame update
    void Start()
    {
        /* FEATURE: Can give random value for local.x at start
         *          so that all the move stages does not look
         *          like they all started at the same time with
         *          same position. May even look unique.
         */
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBouncyStage(); // Calling base update

        Speed = 
            // Checking if +ve distance crossed and fixing speed
            transform.localPosition.x > Distance ?
            Speed < 0 ? Speed : -Speed :

            // Checking if -ve distance crossed and fixing speed
            transform.localPosition.x < -Distance ?
            Speed > 0 ? Speed : -Speed :

            Speed; // Speed remain same
        
        // Moving the stage
        transform.Translate(Vector3.right * Speed 
                            * GameData.Instance.SimulationSpeed 
                            * Time.deltaTime);
    }
}
