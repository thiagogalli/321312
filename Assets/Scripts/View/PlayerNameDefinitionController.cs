using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerNameDefinitionController : MonoBehaviour
{
    Scoreboard scoreboard;

    public Scoreboard GetScoreboard() => scoreboard;
    void Awake()
    {
        var playerNameDefinitionControllers = GameObject.FindGameObjectsWithTag("PlayerNameDefinition").ToList();

        if (playerNameDefinitionControllers.Count > 1)
            Destroy(this.gameObject);
        else
            scoreboard = new Scoreboard(0, "");
    }

    // Update is called once per frame
    void Update()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
