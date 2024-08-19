using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;


public class CardController : MonoBehaviour
{
    public int id;
    public bool isFlipped;
    public Image CardImage { get; set; }
    private GameplayController Controller { get; set; }
    private void Awake()
    {
        Controller = FindFirstObjectByType<GameplayController>();
        CardImage = gameObject.GetComponentInChildren<Image>();
        isFlipped = false;
    }
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => Controller.FlipCard(this));
    }
    public IEnumerator Flip()
    {
        var rectTransform = GetComponent<RectTransform>();

        if (isFlipped)
        {
            for (var i = 180f; i <= 360; i += 10f)
            {
                rectTransform.rotation = Quaternion.Euler(0, i, 0);
                if (i == 270) CardImage.sprite = Controller.CardBack;
                isFlipped = false;
                yield return new WaitForSeconds(0.01f);
                //rectTransform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            for (var i = 360f; i >= 180f; i -= 10f)
            {
                rectTransform.rotation = Quaternion.Euler(0, i, 0);
                if (i == 270) CardImage.sprite = Controller.CardData[id].cardSprite;
                isFlipped = true;
                yield return new WaitForSeconds(0.01f);
                //rectTransform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    public void Match()
    {
        Debug.Log("Formou par!");
        StartCoroutine(FadeImageToAlphaZero());
        Destroy(this.gameObject, 1f);
    }

    public IEnumerator FadeImageToAlphaZero()
    {
        float alpha = 1f;
        for (float i = 1; i > 0.0f; i = i - 0.1f)
        {
            alpha = alpha - 0.10f;
            CardImage.color = new Color(1, 1, 1, alpha);
            yield return new WaitForSeconds(0.009f);
        }
    }

    public bool CheckForArtifacts()
    {
        if (this.gameObject.CompareTag("Artifact"))
            return true;
        else
            return false;
    }
}