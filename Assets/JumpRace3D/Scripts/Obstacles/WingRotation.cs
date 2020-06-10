using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingRotation : MonoBehaviour
{
    [Header("Wing Rotation Properties")]
    [SerializeField]
    private float _rotationLimit; // How much degrees the wing 
                                  // will rotate
    [SerializeField]
    private float _rotationSpeed; // The speed of the rotation
    
    private Quaternion _maxLimit; // Maximum rotation limit
    private Quaternion _minLimit; // Minimum rotation limit
    private float _rotationStep = 0; // The rotation step
    private float _fps; // For storing the Time.deltaTime

    /// <summary>
    /// Gets the y-axis rotation value of the transform,
    /// of type float
    /// </summary>
    private float _yAxis
    { get { return transform.localRotation.eulerAngles.y; } }

    // Start is called before the first frame update
    void Start()
    {
        // Calculating maximum rotation limit
        _maxLimit = Quaternion.Euler(0, _rotationLimit + _yAxis, 0);

        // Calculating minimum rotation limit
        _minLimit = Quaternion.Euler(0, -_rotationLimit + _yAxis, 0);

        // Fixing the starting rotation direction
        _rotationStep = _rotationSpeed > 0 ? 0 : 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Storing the fps for calculation
        _fps = Time.deltaTime * GameData.Instance.SimulationSpeed;

        // Fixing and adding the rotation steps
        _rotationStep = _rotationStep + (_fps * _rotationSpeed) > 1 ? 1 :
                        _rotationStep + (_fps * _rotationSpeed) < 0 ? 0 :
                        _rotationStep + (_fps * _rotationSpeed);

        // Fixing the direction of the rotation speed
        _rotationSpeed = _rotationStep == 1 ? -_rotationSpeed :
                         _rotationStep == 0 ? -_rotationSpeed :
                         _rotationSpeed;

        // Rotating the wing
        transform.localRotation = Quaternion.Lerp(_maxLimit, _minLimit, _rotationStep);
    }
}
