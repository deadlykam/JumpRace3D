using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform _leftFootBooster; // The left foot
                                        // booster effect

    [SerializeField]
    private Transform _rightFootBooster;// The right foot
                                        // booster effect

    [SerializeField]
    private BasicParticle _waterSplash; // The water splash
                                        // effect

    [SerializeField]
    private float _waterSplashHeight; // The height at which
                                      // the water splash
                                      // effect should happen

    [SerializeField]
    private Transform _confettiHolder; // A holder containing all the
                                       // confetti effects

    // List of all the confetti
    private List<BasicParticle> _confetti 
        = new List<BasicParticle>();

    [SerializeField]
    private Transform _shockwavesMediumHolder; // A holder containing
                                               // all the medium
                                               // shockwaves

    [SerializeField]
    private Transform _shockwavesSmallHolder; // A holder containing
                                              // all the small
                                              // shockwaves

    // List of medium shockwaves
    private List<BasicParticle> _shockwavesMedium 
        = new List<BasicParticle>();

    private int _shockwaveMediumPointer = 0; // Pointer needed for
                                             // activating the
                                             // medium shockwaves

    // List of small shockwaves
    private List<BasicParticle> _shockwavesSmall 
        = new List<BasicParticle>();

    private int _shockwaveSmallPointer = 0; // Pointer needed for
                                            // activating the
                                            // medium shockwaves

    [SerializeField]
    private BasicParticle _shockwaveLarge; // Long shockwave
                                           // effect for long
                                           // bouncy stages

    // Storing all the particle requests
    private List<ParticleRequest> _particleRequests 
        = new List<ParticleRequest>();

    private ParticleRequest _currentRequest; // Storing the current
                                             // particle request

    /// <summary>
    /// Flag to check if there are any requests for showing
    /// the particle effects, of type bool
    /// </summary>
    private bool _isRequest
    { get { return _particleRequests.Count != 0; } }

    private bool _isProcess; // Bool to check if processing
                             // is going on

    public static ParticleGenerator Instance;

    private void Awake()
    {
        if (Instance == null) // NOT initialized
        {
            Instance = this; // Initializing the instance
        }
    }

    void Start()
    {
        // Loop for adding all the medium shockwaves to the list
        for (int i = 0; i < _shockwavesMediumHolder.childCount; i++)
            _shockwavesMedium.Add(_shockwavesMediumHolder
                .GetChild(i).GetComponent<BasicParticle>());

        // Loop for adding all the small shockwaves to the list
        for (int i = 0; i < _shockwavesSmallHolder.childCount; i++)
            _shockwavesSmall.Add(_shockwavesSmallHolder
                .GetChild(i).GetComponent<BasicParticle>());

        // Loop for adding all the confetti to the list
        for (int i = 0; i < _confettiHolder.childCount; i++)
            _confetti.Add(_confettiHolder.GetChild(i)
                .GetComponent<BasicParticle>());
    }

    void Update()
    {
        if (_isRequest) // Checking if any requests available
        {
            if (!_isProcess) // Checking if no requests being processed
            {
                _isProcess = true; // Starting processing
                ProcessRequest();  // Processing a request
            }
        }
    }

    /// <summary>
    /// This method processes the particle request.
    /// </summary>
    private void ProcessRequest()
    {
        _currentRequest = _particleRequests[0]; // Getting a request
        _particleRequests.RemoveAt(0); // Removing the request from list

        // Condition to start the medium shockwave effect
        if (_currentRequest.EffectType == 1)
            PlaceShockwave(ref _shockwavesMedium,
                           ref _shockwaveMediumPointer,
                           _currentRequest.Position);
        // Condition to start the small shockwave effect
        else if (_currentRequest.EffectType == 2)
            PlaceShockwave(ref _shockwavesSmall,
                           ref _shockwaveSmallPointer,
                           _currentRequest.Position);
        // Condition to start the large shockwave
        else if (_currentRequest.EffectType == 3)
            PlaceShockwaveLarge(_currentRequest.Position);

        _isProcess = false; // Processing done
    }

    /// <summary>
    /// This method starts the shockwave effect.
    /// </summary>
    /// <param name="shockwaveList">Reference to the shockwave list for getting
    ///                             the shockwave effect, of type
    ///                             List<BasicParticle></param>
    /// <param name="pointer">Referance to the shockwave list pointer, of type
    ///                       int</param>
    /// <param name="position">The position to place the shockwave, of type
    ///                        Vector3</param>
    private void PlaceShockwave(ref List<BasicParticle> shockwaveList, 
                                ref int pointer, Vector3 position)
    {
        // Starting the shockwave effect
        shockwaveList[pointer].StartParticleEffect(position);

        // Incrementing the pointer to point towards the next
        // shockwave
        pointer = pointer + 1 >= shockwaveList.Count ? 0 : pointer + 1;
    }

    /// <summary>
    /// This method starts the long bouncy stage shockwave
    /// effect.
    /// </summary>
    /// <param name="position">The position to place the long shockwave
    ///                        effect, of type Vector3</param>
    private void PlaceShockwaveLarge(Vector3 position)
    {
        // Starting the long shockwave effect
        _shockwaveLarge.StartParticleEffect(position);
    }

    /// <summary>
    /// This method places the booster particles on to the
    /// character feet.
    /// </summary>
    /// <param name="character">The character on which the boosters will
    ///                         be placed, of type CharacterInfo</param>
    public void PlaceBooster(CharacterInfo character)
    {
        // Placing the booster
        character.SetFeetObject(_leftFootBooster, _rightFootBooster);
    }

    /// <summary>
    /// This method shows/hides the booster.
    /// </summary>
    /// <param name="active">The flag to show or hide the booster,
    ///                      <para>true = show booster</para>
    ///                      <para>false = hide booster</para>
    ///                      of type bool</param>
    public void SetBooster(bool active)
    {
        _leftFootBooster.gameObject.SetActive(active);
        _rightFootBooster.gameObject.SetActive(active);
    }

    /// <summary>
    /// This method starts the water splash effect.
    /// </summary>
    /// <param name="position">The position to generate the
    ///                        water splash effect, of type
    ///                        Vector3</param>
    public void PlaceWaterSplash(Vector3 position)
    {
        // Setting the correct height of the water splash
        position.Set(position.x, 
                     _waterSplashHeight, 
                     position.z);

        // Starting the water splash
        _waterSplash.StartParticleEffect(position);
    }

    /// <summary>
    /// This method starts the confetti effect.
    /// </summary>
    public void PlayConfetti()
    {
        // Loop for starting all the confetti effects
        for (int i = 0; i < _confetti.Count; i++)
            _confetti[i].StartParticleEffect();
    }

    /// <summary>
    /// This method starts a shockwave effect.
    /// </summary>
    /// <param name="effectType">The type of shockwave effect to show,
    ///                          <para>1 = Shockwave Medium</para>
    ///                          <para>2 = Shockwave Small</para>
    ///                          <para>3 = Shockwave Large</para>
    ///                          of type int</param>
    /// <param name="position">The position to place the shockwave effect,
    ///                        of type Vector3</param>
    public void AddShockwaveRequest(int effectType, Vector3 position)
    {
        // Adding a new request for showing shockwave particle
        _particleRequests.Add(new ParticleRequest(effectType, position));
    }

    /// <summary>
    /// This method resets the ParticleGenerator.
    /// </summary>
    public void ResetParticleGenerator()
    {
        SetBooster(false); // Hiding the booster
    }

    public struct ParticleRequest
    {
        private int _effectType; // The type of particle effect

        /// <summary>
        /// The type of particle effect to show, 
        /// <para>1 = Shockwave Medium</para>
        /// <para>2 = Shockwave Small</para>
        /// <para>3 = Shockwave Large</para>
        /// of type int
        /// </summary>
        public int EffectType { get { return _effectType; } }

        private Vector3 _position; // The position of the
                                   // particle effect

        /// <summary>
        /// The position to place the particle effect, of type
        /// Vector3
        /// </summary>
        public Vector3 Position { get { return _position; } }

        /// <summary>
        /// This constructor creates a particle effect request.
        /// </summary>
        /// <param name="effectType">The type of particle effect,
        ///                          <para>1 = Shockwave Medium</para>
        ///                          <para>2 = Shockwave Small</para>
        ///                          <para>3 = Shockwave Large</para>
        ///                          of type int</param>
        /// <param name="position">The position of the particle
        ///                        effect, of type Vector3</param>
        public ParticleRequest(int effectType, Vector3 position)
        {
            _effectType = effectType; // Setting the type
            _position = position;     // Setting the position
        }
    }
}
