using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Player")]
    [SerializeField] private AudioClip limbSeparation;
    [SerializeField] private AudioClip[] throwSounds; // B_Throw1 y B_Throw2
    [SerializeField] private AudioClip limbHit;
    [SerializeField] private AudioClip dashOne;
    [SerializeField] private AudioClip dashTwo;
    [SerializeField] private AudioClip jumpLanding;

    [Header("Selector")]
    [SerializeField] private AudioClip buttonOne;
    [SerializeField] private AudioClip buttonTwo;
    [SerializeField] private AudioClip selectorSpin;

    [Header("Horno")]
    [SerializeField] private AudioClip hornoAmbient;
    [SerializeField] private AudioClip hornoRegenerate;

    [Header("Music")]
    [SerializeField] private AudioClip backgroundMusic;

    [Header("Knife")]
    [SerializeField] private AudioClip knifeFall;

    private AudioSource sfxSource;
    private AudioSource musicSource;
    private AudioSource ambientSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // SFX source
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        // Music source - loop
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = 0.5f;

        // Ambient source - loop
        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.loop = true;
        ambientSource.playOnAwake = false;
        ambientSource.volume = 0.3f;
    }

    private void Start()
    {
        PlayMusic();
    }

    // MUSIC
    public void PlayMusic()
    {
        if (backgroundMusic == null) return;
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    // PLAYER
    public void PlayLimbSeparation() => sfxSource.PlayOneShot(limbSeparation);
    public void PlayThrow() => sfxSource.PlayOneShot(throwSounds[Random.Range(0, throwSounds.Length)]);
    public void PlayLimbHit() => sfxSource.PlayOneShot(limbHit);
    public void PlayDash(bool isFirst) => sfxSource.PlayOneShot(isFirst ? dashOne : dashTwo);
    public void PlayJumpLanding() => sfxSource.PlayOneShot(jumpLanding);

    // SELECTOR
    public void PlayButton(bool isFirst) => sfxSource.PlayOneShot(isFirst ? buttonOne : buttonTwo);
    public void PlaySelectorSpin() => sfxSource.PlayOneShot(selectorSpin);

    // HORNO
    public void PlayHornoAmbient()
    {
        ambientSource.clip = hornoAmbient;
        ambientSource.Play();
    }
    public void StopHornoAmbient() => ambientSource.Stop();
    public void PlayHornoRegenerate() => sfxSource.PlayOneShot(hornoRegenerate);

    // KNIFE
    public void PlayKnifeFall() => sfxSource.PlayOneShot(knifeFall);
}
