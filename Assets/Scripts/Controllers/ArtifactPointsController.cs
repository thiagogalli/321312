using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactPointsController : MonoBehaviour
{
    public ArtifactPoints artifactPoints;
    void Awake()
    {
        artifactPoints = new ArtifactPoints();
    }

    void Update()
    {
        DontDestroyOnLoad(gameObject);
    }
}
