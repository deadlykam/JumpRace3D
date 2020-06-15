using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicParticle : MonoBehaviour
{
    [SerializeField]
    private float _timer; // The time after which to disable
                          // the particle effect

    private float _timerCurrent; // Current count down timer

    // Update is called once per frame
    void Update()
    {
        // Condition for hiding the particle effect
        if (_timerCurrent < 0) gameObject.SetActive(false);
        // Condition to counting down
        else _timerCurrent -= Time.deltaTime;
    }

    /// <summary>
    /// This mehtod starts the count down timer for
    /// hiding the particle effect.
    /// </summary>
    public virtual void StartParticleEffect()
    {
        gameObject.SetActive(true); // Showing the particle
        _timerCurrent = _timer; // Rsetting timer
    }

    /// <summary>
    /// This mehtod starts the count down timer for
    /// hiding the particle effect.
    /// </summary>
    /// <param name="position">The position to place the particle
    ///                        effect, of type Vector3</param>
    public virtual void StartParticleEffect(Vector3 position)
    {
        transform.position = position; // Setting the position
        gameObject.SetActive(true); // Showing the particle
        _timerCurrent = _timer; // Rsetting timer
    }
}
