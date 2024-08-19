using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPoints
{
    public int quantityOfTouches { get; private set; }
    public int GetQuantityOfTouches() => quantityOfTouches;
    public void SetQuantityOfTouches(int _quantityOfTouches) => quantityOfTouches = _quantityOfTouches;
}
