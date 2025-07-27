using System.Diagnostics;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private float _asteroidRotation = 20f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    private AudioManager _audioManager;

    // Update is called once per frame
    void Start()
    {

        _audioManager = FindObjectOfType<AudioManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }


    void Update()
    {
        transform.Rotate(0, 0, _asteroidRotation * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 2.5f);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, 0.2f);
            _audioManager.PlayDeathExplosion();
        }
    }
}
