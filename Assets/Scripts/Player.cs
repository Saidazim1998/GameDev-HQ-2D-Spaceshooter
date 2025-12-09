using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6f;

    private float _speedMultiplier = 2f;
    [SerializeField] private float _fireRate = 0.15f;
    private float _canFire = -1f;
    
    [SerializeField] private float _health = 3;
    [SerializeField] private Color32 _damageColor;

    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _triplelaserPrefab;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _rightEngineHurt;
    [SerializeField] private GameObject _leftEngineHurt;
    
    private bool _isTripleShotActive = false;
    private bool _isSpeeBoostActive = false;
    private bool _isShieldsActive = false;
    private bool _isSpeedUpActive = false;
    private SpawnManager _spawnManager;

    [SerializeField] private int _score; 
    private UI_Manager _uiManager;

    [SerializeField] private AudioClip _laserSoundClip;
    private AudioSource _audioSource;
    
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("There is no gameobejct with name Spawn_Manager");
            return;
        } 

        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        if (_uiManager == null)
        {
            Debug.LogError("There is no gameobject with name Canvas.");
            return ;
        }

        transform.position = Vector3.zero;
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource==null)
        {
            Debug.Log("Audio source is NULL!");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
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

        if(Input.GetKeyDown(KeyCode.LeftShift) && !_isSpeedUpActive)
        {
            _speed *=_speedMultiplier;
            _isSpeedUpActive = true;
            _uiManager.EmptyingSlider();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) && _isSpeedUpActive)
        {
            _speed /=_speedMultiplier;
            _isSpeedUpActive = false;
            _uiManager.FillingSlider();
        }
    }

    private void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        if (_isTripleShotActive)
        {
            Instantiate(_triplelaserPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Vector3 spawnPos = transform.position + new Vector3(0f, 1.05f, 0f);
            Instantiate(_laserPrefab, spawnPos, Quaternion.identity);    
        }
        
        _audioSource.Play();
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float vecticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, vecticalInput, 0);
        
        transform.Translate(direction*_speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.85f, 0), 0);

        if (transform.position.x>= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if(transform.position.x<=-11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    public void TakeDamage(float damage)
    {
        if (_isShieldsActive)
        {
            _isShieldsActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        
        _health -= damage;
        int livesIndex = (int)_health;
        _uiManager.UpdateLives(livesIndex);

        if (_health == 2)
        {
            _rightEngineHurt.SetActive(true);
        }
        else if(_health == 1)
        {
            _leftEngineHurt.SetActive(true);
        }

        Debug.Log("Player's health = "+_health);

        StartCoroutine(ChangeColor());
        
        if (_health<=0)
        {
            Debug.Log("Game Over!");
            _spawnManager.OnPlayerDeath();
            Destroy(gameObject);
        }
    }

    IEnumerator ChangeColor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = _damageColor;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
        
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }


    public void SpeedUpActive()
    {
        _isSpeeBoostActive = true;
        _speed *=_speedMultiplier;
        StartCoroutine(SpeedUpPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    IEnumerator SpeedUpPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isSpeeBoostActive = false;
        _speed /= _speedMultiplier;
        
    }

    public void ShieldActive()
    {
        _isShieldsActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
