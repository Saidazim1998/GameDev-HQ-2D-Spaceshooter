using System;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;

    [Tooltip("ID 0-> Triple shot \nID 1-> Speed \nID 2-> Shield")]
    [SerializeField] private int _powerUpID;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down*_speed*Time.deltaTime);
        if (transform.position.y <=-5.8f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            switch (_powerUpID)
            {
                case 0: 
                    player.TripleShotActive();
                    break;
                case 1:
                    player.SpeedUpActive();
                    break;
                case 2:
                    player.ShieldActive();
                    break;
                default:
                    Debug.Log("Default state is active");
                    break;
            }
            Destroy(gameObject);
        }
    }
}
