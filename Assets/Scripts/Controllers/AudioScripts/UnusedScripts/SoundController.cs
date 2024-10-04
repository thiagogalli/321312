using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    AudioSource musicSource;
    
    Slider soundSlider;
    Image soundIcon;

    [SerializeField] Sprite withSoundSprite;
    [SerializeField] Sprite withoutSoundSprite;

    static SoundController instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);

        soundSlider = FindFirstObjectByType<Slider>(FindObjectsInactive.Include).GetComponentInChildren<Slider>();
        soundIcon = GameObject.FindGameObjectWithTag("SoundIcon").GetComponent<Image>();

        musicSource = GetComponent<AudioSource>();
        musicSource.volume = soundSlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        if (soundSlider is null)
            soundSlider = FindFirstObjectByType<Slider>(FindObjectsInactive.Include).GetComponentInChildren<Slider>();

        if (soundIcon is null)
            soundIcon = GameObject.FindGameObjectWithTag("SoundIcon").GetComponent<Image>();

        musicSource.volume = soundSlider.value;

        if (musicSource.volume <= 0)
            soundIcon.sprite = withoutSoundSprite;
        else
            soundIcon.sprite = withSoundSprite;
    }
}
