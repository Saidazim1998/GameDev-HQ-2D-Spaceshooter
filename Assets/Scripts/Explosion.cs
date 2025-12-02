using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private AudioSource _explosionAudioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _explosionAudioSource.Play();
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
