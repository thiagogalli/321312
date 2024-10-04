using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWindowDontDestroyController : MonoBehaviour
{
    static SoundWindowDontDestroyController instance;

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
