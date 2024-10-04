using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDescriptionDontDestroyController : MonoBehaviour
{
    static SoundDescriptionDontDestroyController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);
    }
}
