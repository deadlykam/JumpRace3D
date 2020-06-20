using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreenUI : BasicUI
{
    [Header("End Screen UI Properties")]
    [SerializeField]
    private TextMeshProUGUI[] _posTexts; // Position texts

    [SerializeField]
    private GameObject[] _posBacks; // Red backgrounds for
                                    // highlighting the player

    [SerializeField]
    private Canvas _triggerCanvas; // Canvas that triggers an
                                   // event


    /// <summary>
    /// This method resets the End Screen UI
    /// </summary>
    private void ResetUI()
    {
        // Loop for resetting all the texts and player
        // highlighters
        for (int i = 0; i < _posTexts.Length; i++)
        {
            // Resetting the text
            _posTexts[i].text = "";

            // Condition for resetting the player highlighter
            if (i != 0) _posBacks[i - 1].SetActive(false);
        }

        _triggerCanvas.enabled = true; // Enabling the trigger
                                       // canvas again
    }

    /// <summary>
    /// This method shows all the racers at the end screen.
    /// </summary>
    public void SetUI()
    {
        // Loop for showing all the racers in the end screen
        for(int i = 0; i < RaceTracker.Instance.Racers.Length; i++)
        {
            // Checking if the racer is the player
            if(RaceTracker.Instance.Racers[i]
                .CharacterName == GameData.Instance.PlayerName)
            {
                // Checking if it is not the first position
                // and showing the player highlighter
                if (i != 0) _posBacks[i - 1].SetActive(true);
            }
            // Condition for not being the player and hiding
            // the player highlighter
            else if (i != 0) _posBacks[i - 1].SetActive(false);

            // Setting the racers name
            _posTexts[i].text = RaceTracker.Instance
                                .Racers[i].CharacterName;
        }
    }
    
    /// <summary>
    /// This method disables the trigger event UI.
    /// </summary>
    public void DisableTrigger() { _triggerCanvas.enabled = false; }

    /// <summary>
    /// This method hides the End Screen UI.
    /// </summary>
    public override void HideUI()
    {
        base.HideUI();
        ResetUI(); // Resetting the UI
    }
}
