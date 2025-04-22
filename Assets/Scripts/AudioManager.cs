using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioClip backGroundClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip dashClip;
    [SerializeField] private AudioClip runClip;
    [SerializeField] private AudioClip wallJumpClip;
    [SerializeField] private AudioClip attackClip;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayBackGroundMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBackGroundMusic()
    {
        backgroundAudioSource.clip = backGroundClip;
        backgroundAudioSource.Play();
    }

    public void PlayCoinSound()
    {
        effectAudioSource.PlayOneShot(coinClip);
    }
    public void PlayJumpSound()
    {
        effectAudioSource.PlayOneShot(jumpClip);
    }
    public void PlayWallJumpSound()
    {
        effectAudioSource.PlayOneShot(wallJumpClip);
    }
    public void PlayDashSound()
    {
        effectAudioSource.PlayOneShot(dashClip);
    }
    //public void PlayRunSound()
    //{
    //    effectAudioSource.PlayOneShot(runClip);
    //}
    public void PlayAttackSound()
    {
        effectAudioSource.PlayOneShot(attackClip);
    }
}
