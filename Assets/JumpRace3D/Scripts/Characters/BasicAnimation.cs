using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>BasicAnimation</c> handles all the animation functionality common 
/// between all characters.
/// </summary>
public class BasicAnimation : BasicCharacter
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// This method is the update method of BasicAnimation and Must be 
    /// called in child class.
    /// </summary>
    protected void UpdateBasicAnimation()
    {
        UpdateBasicCharacter(); // Calling Basic Character update
    }
}
