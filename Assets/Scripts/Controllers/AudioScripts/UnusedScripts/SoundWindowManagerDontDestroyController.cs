using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWindowManagerController : MonoBehaviour
{
    static SoundWindowManagerController instance;

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
