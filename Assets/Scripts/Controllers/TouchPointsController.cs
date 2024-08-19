using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPointsController : MonoBehaviour
{
    public TouchPoints touchPoints;
    void Awake()
    {
        touchPoints = new TouchPoints();

        //Inicialização do valor
        touchPoints.SetQuantityOfTouches(0);
    }

    // Update is called once per frame
    void Update()
    {
        DontDestroyOnLoad(gameObject);
    }
}
