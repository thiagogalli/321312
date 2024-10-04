using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundCloseButtonController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject soundWindow;
    [SerializeField] GameObject fadeElement;

    public void OnPointerClick(PointerEventData eventData)
    {
        soundWindow.SetActive(false);
        fadeElement.SetActive(false);
    }
}
