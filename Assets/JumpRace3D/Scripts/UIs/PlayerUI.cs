using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Player UI Properties")]
    [SerializeField]
    private TextMeshProUGUI _stageNumberCurrent; // For showing the
                                                 // current stage
                                                 // number

    [SerializeField]
    private TextMeshProUGUI _stageNumberNext; // For showing the next
                                              // stage number

    [SerializeField]
    private Image _bar; // For implementing the bar
    
    /// <summary>
    /// Setting the stage numbers of the Player UI
    /// </summary>
    /// <param name="current">The current stage number, 
    ///                       of type int</param>
    public void SetStageNumbers(int current)
    {
        // Setting the current stage number
        _stageNumberCurrent.text = current.ToString();

        // Setting the next stage number
        _stageNumberNext.text = (current + 1).ToString();
    }

    /// <summary>
    /// This method sets bar fill amount.
    /// </summary>
    /// <param name="percentage">The bar fill amount in percentage,
    ///                          of type float</param>
    public void SetBar(float percentage)
    {
        // Fixing any error values
        percentage = percentage > 1 ? 1 : 
                     percentage < 0 ? 0 : 
                     percentage;

        _bar.fillAmount = percentage; // Setting the bar
    }
}
