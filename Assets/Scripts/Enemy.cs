using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;

    [SerializeField] private int _damage = 1;
    [SerializeField] private GameObject _laserPrefab;

    private Player _player;

    private Animator _animator;

    private AudioSource _audioSource;
    private float _fireRate = 3f;
    private float _canFire = -1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player!=null)
        {
            _animator = GetComponent<Animator>();
        }
        transform.position = new Vector3(Random.Range(-9, 9), 7.28f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time>_canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject laser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = laser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down*Time.deltaTime*_speed);

        if (transform.position.y<=-5.27f)
        {
            float randomX = Random.Range(-9, 9);
            transform.position = new Vector3(randomX, 7.28f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player!=null)
            {
                player.TakeDamage(_damage);
            }
            
            _animator.SetTrigger("OnEnemyDestroyed");
            _speed = 0;
            
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());

            Destroy(gameObject,2.8f);
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            if (_player!=null)
            {
                _player.AddScore(10);
            }
            
            _animator.SetTrigger("OnEnemyDestroyed");
            _speed = 0;
            
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            
            Destroy(gameObject,2.8f);
        }
    }
}
