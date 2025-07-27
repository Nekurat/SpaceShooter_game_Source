using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const bool V = true;
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _LaserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private GameObject _TripleShotPrefab;
    [SerializeField]
    private GameObject _SpeedBoostPrefab;
    [SerializeField]
    private GameObject _ShieldPrefab;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _playerShield;
    [SerializeField]
    private int _score;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private GameObject _rightEngine;
    private bool _isInvulnerable = false;

    private UIManager _uiManager;
    [SerializeField]
    private AudioSource _laserAudio;
    private AudioManager _audioManager;
    // Start is called before the first frame update
    void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        transform.position = new Vector3(0, 0, 0);
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0f), 0f);

        if (transform.position.x >= 11)
        {
            transform.position = new Vector3(-11f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11)
        {
            transform.position = new Vector3(11f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            Vector3 LaserOffset = new Vector3(0, 1f, 0);
            _canFire = Time.time + _fireRate;

            if (_isTripleShotActive)
            {
                Instantiate(_TripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_LaserPrefab, transform.position + LaserOffset, Quaternion.identity);
            }

            _laserAudio.Play();
        }

    }

    public void Damage()
    {
        if (_isInvulnerable) return;

        Debug.Log("Damage called");

        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _playerShield.SetActive(false);
            StartCoroutine(InvulnerableRoutine());
            return;
        }

        _lives -= 1;
        _uiManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        _audioManager.PlayDeathExplosion();

        if (_lives < 1)
        {
            _uiManager.GameOver();
            Destroy(this.gameObject);
            _audioManager.PlayDeathExplosion();
        }

        StartCoroutine(InvulnerableRoutine());

    }

    private IEnumerator InvulnerableRoutine()
    {
        _isInvulnerable = true;
        yield return new WaitForSeconds(0.1f);
        _isInvulnerable = false;
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed = 8.5f;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(4f);
        _isSpeedBoostActive = false;
        _speed = 5f;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _playerShield.SetActive(true);
    }

    public void AddScore(int score)
    {
        _score += score;
        _uiManager.UpdateScore(_score);
    }

}


