using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard 
{
    public int points { get; set; }
    public string name { get; set; }

    public Scoreboard(int points, string name)
    {
        this.name = name;
        this.points = points;
    }
}
