using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemyspeed = 2.5f;

    private Player _player;
    private Animator _deathAnim;
    private AudioManager _audioManager;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField] private Transform _gunLeft;
    [SerializeField] private Transform _gunRight;
    private Coroutine _enemyFireCoroutine;
    //hande to animator component

    void Start()
    {

        float randomX = Random.Range(-8f, 8f);
        transform.position = new Vector3(randomX, 7f, 0f);

        _enemyFireCoroutine = StartCoroutine(EnemyFire());

        _audioManager = FindObjectOfType<AudioManager>();

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player != null)
        {
            _deathAnim = GetComponent<Animator>();
        }
    }

    private IEnumerator EnemyFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            Instantiate(_enemyLaserPrefab, _gunLeft.position, Quaternion.identity);
            Instantiate(_enemyLaserPrefab, _gunRight.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _enemyspeed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 6f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            EnemyDeath();
            _audioManager.PlayDeathExplosion();
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10);
            }
            EnemyDeath();
            _audioManager.PlayDeathExplosion();
        }
    }

    private void EnemyDeath()
    {
        if (_enemyFireCoroutine != null)
        {
            StopCoroutine(_enemyFireCoroutine);
        }

        _deathAnim.SetTrigger("OnEnemyDeath");
        GetComponent<Collider2D>().enabled = false;
        _enemyspeed = 1f;
        StartCoroutine(DestroyAfterAnim());
    }

    private IEnumerator DestroyAfterAnim()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(this.gameObject);
    }
}
