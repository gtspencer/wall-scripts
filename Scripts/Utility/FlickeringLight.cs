using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light flickeringLight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 2f;
    public float flickerSpeed = 0.1f;
    public float offChance = 0.1f; // Chance to turn off completely

    private float targetIntensity;
    private float smoothTime = 0.1f;

    [SerializeField] private AudioClip[] _buzzSfx;
    [SerializeField] private AudioClip _humSfx;

    [SerializeField] private AudioSource _humSource;
    [SerializeField] private AudioSource _buzzSource;

    void Start()
    {
        if (flickeringLight == null)
            flickeringLight = GetComponent<Light>();

        targetIntensity = flickeringLight.intensity;
    }

    void FixedUpdate()
    {
        if (Random.value < flickerSpeed)
        {
            if (Random.value < offChance)
            {
                if (_fadedIn)
                {
                    _buzzSource.clip = _buzzSfx[Random.Range(0, _buzzSfx.Length)];
                    _buzzSource.Play();
                }
                
                flickeringLight.enabled = false;
            }
            else
            {
                flickeringLight.enabled = true;
                targetIntensity = Random.Range(minIntensity, maxIntensity);
            }
        }

        if (flickeringLight.enabled)
        {
            flickeringLight.intensity = Mathf.Lerp(flickeringLight.intensity, targetIntensity, Time.deltaTime / smoothTime);
        }
    }

    private bool _fadedIn;
    public void FadeInSFX()
    {
        StartCoroutine(FadeInSFX_Coroutine());
    }

    [SerializeField] private float _fadeInSfxDuration = 10f;
    private IEnumerator FadeInSFX_Coroutine()
    {
        _humSource.loop = true;
        _humSource.Play();

        _fadedIn = true;
        yield return null;

        /*var elapsed = 0f;
        while (elapsed < _fadeInSfxDuration)
        {
            elapsed += Time.deltaTime;

            var val = elapsed / _fadeInSfxDuration;

            _humSource.volume = val;
            _buzzSource.volume = val;

            yield return null;
        }*/
    }
}
