using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6f;

    private float _speedMultiplier = 2f;
    [SerializeField] private float _fireRate = 0.15f;
    private float _canFire = -1f;
    
    [SerializeField] private int _health = 3;
    [SerializeField] private Color32 _damageColor;

    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _triplelaserPrefab;
    [SerializeField] private GameObject _shieldVisualizer;
    
    private bool _isTripleShotActive = false;
    private bool _isSpeeBoostActive = false;
    private bool _isShieldsActive = false;
    private SpawnManager _spawnManager;

    [SerializeField] private int _score; 
    private UI_Manager _uiManager;
    
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        transform.position = Vector3.zero;
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

    private void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        if (_isTripleShotActive)
        {
            Instantiate(_triplelaserPrefab, transform.position, quaternion.identity);
        }
        else
        {
            Vector3 spawnPos = transform.position + new Vector3(0f, 1.05f, 0f);
            Instantiate(_laserPrefab, spawnPos, Quaternion.identity);    
        }
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

    public void TakeDamage(int damage)
    {
        if (_isShieldsActive)
        {
            _isShieldsActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        
        _health -= damage;
        _uiManager.UpdateLives(_health);
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
