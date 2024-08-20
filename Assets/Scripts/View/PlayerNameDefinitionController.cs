using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameDefinitionController : MonoBehaviour
{
    Scoreboard scoreboard;

    public Scoreboard GetScoreboard() => scoreboard;
    void Start()
    {
        scoreboard = new Scoreboard(0, "");
    }

    // Update is called once per frame
    void Update()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
