using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUI : MonoBehaviour
{
    [Header("Basic UI Properties")]
    [SerializeField]
    private Canvas[] _canvases; // Storing all the
                                // canvases

    /// <summary>
    /// The flag that shows if the UI is shown or not,
    /// <para>true = UI shown</para>
    /// <para>false = UI hidden</para>
    /// of type bool
    /// </summary>
    public bool IsUIShown
    { get { return (_canvases.Length != 0) 
                    && _canvases[0].enabled; } }

    /// <summary>
    /// This method activates/deactivates canvases.
    /// </summary>
    /// <param name="active">The flag used to activate or
    ///                      deactivate canvases, of type
    ///                      bool</param>
    protected void SetCanvases(bool active)
    {
        // Loop for activating/deactivating canvases
        for (int i = 0; i < _canvases.Length; i++)
            _canvases[i].enabled = active;
    }

    /// <summary>
    /// This method shows the UI.
    /// </summary>
    public virtual void ShowUI() { SetCanvases(true); }

    /// <summary>
    /// This method hides the UI.
    /// </summary>
    public virtual void HideUI() { SetCanvases(false); }
}
