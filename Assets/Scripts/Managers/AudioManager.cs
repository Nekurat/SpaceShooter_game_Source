using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _deathExplosion;
    [SerializeField]
    private AudioSource _powerupSound;

    public void PlayDeathExplosion()
    {
        _deathExplosion.Play();
    }

    public void PlayPowerupSound()
    {
        _powerupSound.Play();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
