using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTitleDontDestroyController : MonoBehaviour
{
    static SoundTitleDontDestroyController instance;

    // Update is called once per frame
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
