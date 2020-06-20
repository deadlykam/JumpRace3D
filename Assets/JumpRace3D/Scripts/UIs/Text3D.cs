using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Text3D : MonoBehaviour
{
    public TextMeshProUGUI TextNumber;

    /// <summary>
    /// This method sets the text of the TextNumber.
    /// </summary>
    /// <param name="text">The text to set, of type string</param>
    public void SetText(string text) { TextNumber.SetText(text); }
}
