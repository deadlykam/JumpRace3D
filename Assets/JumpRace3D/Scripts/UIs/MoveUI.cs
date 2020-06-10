using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUI : MonoBehaviour
{
    [Header("Move UI Properties")]
    private RectTransform _rectTransform; // Needed to move around the UI

    [SerializeField]
    private bool _isShow = false; // Flag for showing or hiding the UI

    /// <summary>
    /// Flag to check if the UI is showing or not, of type bool
    /// </summary>
    public bool IsShow { get { return _isShow; } }

    [SerializeField]
    private float _speed; // The speed of the UI movement

    private Vector2 _currentPosition; // Storing the current position
                                      // of the UI at the start

    // Start is called before the first frame update
    void Start()
    {
        // Storing the RectTransform
        _rectTransform = GetComponent<RectTransform>();

        // Storing the current position
        _currentPosition = _rectTransform.anchoredPosition;

        // Condition to check if to show the UI at the start
        if (IsShow) _rectTransform.anchoredPosition = Vector2.zero;

        /**
            Hint: If it does not work then create a Vector2 and store the
                  width and height separately and then set the .sizeDelta
         */
        // Setting the size of the RexTransform
        _rectTransform.sizeDelta = GameData.Instance.MainCanvas.sizeDelta;

    }

    // Update is called once per frame
    void Update()
    {
        // Condition to move in to show the UI
        if(IsShow 
            && _rectTransform.anchoredPosition != Vector2.zero)
        {
            _rectTransform.anchoredPosition = Vector2.MoveTowards(
                    _rectTransform.anchoredPosition,
                    Vector2.zero,
                    _speed * Time.deltaTime
                );
        }
        // Condition to move out to hide the UI
        else if(!IsShow 
            && _rectTransform.anchoredPosition != _currentPosition)
        {
            _rectTransform.anchoredPosition = Vector2.MoveTowards(
                    _rectTransform.anchoredPosition,
                    _currentPosition,
                    _speed * Time.deltaTime
                );
        }
    }
}
