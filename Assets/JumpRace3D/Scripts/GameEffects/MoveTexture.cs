using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTexture : MonoBehaviour
{
    [SerializeField]
    private float _speed; // Speed of the movement

    [SerializeField]
    private float _maxLimit; // Maximum limit till reset

    private Material _material; // Material of the object

    private Vector2 _offset = Vector2.zero; // The offset to move

    private float _step; // Steps taken to move the texture

    private float _fps; // Storing the fps for calculation

    // Start is called before the first frame update
    void Start()
    {
        // Setting the material
        _material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        _fps = _speed * GameData.Instance.SimulationSpeed 
                      * Time.deltaTime; // Calculating the fps

        // Getting the step value
        _step = (_step + _fps) >= _maxLimit ? 0 : (_step + _fps);

        _offset.Set(_step, 0); // Setting the move value

        // Moving the texture
        _material.SetTextureOffset("_MainTex", _offset);
    }
}
