using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameplayController : MonoBehaviour
{
    //Imagem Padrão
    [field: Header("Cartas")]
    [field: SerializeField]
    public Sprite CardBack { get; set; }

    [field: SerializeField] public JogoMemoriaImagemCard[] CardData { get; private set; } // Imagens das cartas

    [field: Space]
    [field: Header("Tamanho")]
    [field: SerializeField]
    private int GridSizeX { get; set; }

    [field: SerializeField] private int GridSizeY { get; set; }

    [SerializeField] private GameObject[] slots;
    [field: Space][field: Header("Sons")] private AudioSource AudioSource { get; set; }
    [SerializeField] private AudioClip SomClick { get; set; }
    [SerializeField] private AudioClip SomUnmatch { get; set; }
    [SerializeField] private AudioClip SomMatch { get; set; }

    [Space]
    [Header("Components")]
    [SerializeField]
    private GameObject cardPrefab; // Prefab da carta

    public GameObject toastPrefab;
    public GameObject winPrefab;

    private List<int> _availableIDs; // Lista de IDs disponíveis para as cartas
    [field: SerializeField] private List<CardController> FlippedCards { get; set; } // Lista das cartas viradas
    private List<CardController> MatchedCards { get; set; } // Lista das cartas combinadas
    private List<CardController> AllRemainingCards { get; set; } // Lista de todas as cartas disponíveis

    [Space]
    [Header("Points and Touches Attributes and Components")]
    ArtifactPointsController artifactController;
    TextMeshProUGUI artifactPointsText;

    TouchPointsController touchPointsController;
    TextMeshProUGUI touchPointsText;
    PlayerNameDefinitionController playerNameDefinitionController;

    [SerializeField] LevelLoaderController levelLoaderController;

    [SerializeField] Image popupWindowCardImage;
    [SerializeField] TextMeshProUGUI popupWindowCardTitle;
    [SerializeField] TextMeshProUGUI popupWindowCardDescription;
    [SerializeField] Animator popupWindowAnim;
    [SerializeField] Animator popupFadeAnim;

    [SerializeField] Button exchangeArtifactPoints;

    private void Start()
    {
        artifactController = GameObject.FindGameObjectWithTag("ArtifactController").GetComponent<ArtifactPointsController>();
        artifactPointsText = GameObject.FindGameObjectWithTag("PointText").GetComponent<TextMeshProUGUI>();

        touchPointsController = GameObject.FindGameObjectWithTag("TouchController").GetComponent<TouchPointsController>();
        touchPointsText = GameObject.FindGameObjectWithTag("TouchText").GetComponent<TextMeshProUGUI>();

        playerNameDefinitionController = GameObject.FindGameObjectWithTag("PlayerNameDefinition").GetComponent<PlayerNameDefinitionController>();

        var actualPoints = artifactController.artifactPoints.GetPoints();
        artifactPointsText.text = actualPoints.ToString();

        var actualTouchPoints = touchPointsController.touchPoints.GetQuantityOfTouches();
        touchPointsText.text = actualTouchPoints.ToString();

        //Lógica para habilitar/desabilitar o botão de acordo com a quantidade de pontos que o usuário tem:
        if (actualPoints >= 5)
            exchangeArtifactPoints.interactable = true;
        else
            exchangeArtifactPoints.interactable = false;

        Components();
        Shuffle(CardData);
        FlippedCards = new List<CardController>();
        MatchedCards = new List<CardController>();
        AllRemainingCards = new List<CardController>();
        InitializeCards();
    }

    private void Components()
    {
        AudioSource = gameObject.GetComponent<AudioSource>();
    }

    private void InitializeCards()
    {
        var cards = GameObject.Find("Cards");
        var totalPairs = (GridSizeX * GridSizeY) / 2;
        _availableIDs = new List<int>();

        for (var i = 0; i < totalPairs; i++)
        {
            _availableIDs.Add(i);
            _availableIDs.Add(i);
        }


        Shuffle(_availableIDs); // Embaralhar a lista de IDs
        for (var x = 0; x < GridSizeX; x++)
        {
            for (var y = 0; y < GridSizeY; y++)
            {
                var index = y * GridSizeX + x; // Índice baseado na posição do grid

                var cardObj = Instantiate(cardPrefab,
                    slots[index].transform.position,
                    Quaternion.identity,
                    slots[index].transform);

                var card = cardObj.GetComponent<CardController>();

                card.id = _availableIDs[index];

                AllRemainingCards.Add(card);
            }
        }
    }

    private static void Shuffle<T>(List<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = Random.Range(0, n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    private static void Shuffle<T>(T[] array)
    {
        var n = array.Length;
        for (var i = 0; i < n - 1; i++)
        {
            var j = i + UnityEngine.Random.Range(0, n - i);
            (array[j], array[i]) = (array[i], array[j]);
        }
    }


    public void FlipCard(CardController card, bool fromExchange = false)
    {
        exchangeArtifactPoints.interactable = false;

        if (!fromExchange)
        {
            //Aumento do touch:
            var actualTouches = touchPointsController.touchPoints.GetQuantityOfTouches() + 1;
            touchPointsController.touchPoints.SetQuantityOfTouches(actualTouches);

            //Alteração gráfica (text):
            touchPointsText.text = actualTouches.ToString();

            playerNameDefinitionController.GetScoreboard().points = actualTouches;
        }

        if (!card.isFlipped && FlippedCards.Count < 2)
        {
            StartCoroutine(card.Flip());
            //SoundPlay(SomClick);
            FlippedCards.Add(card);
            if (FlippedCards.Count == 2)
            {
                StartCoroutine(CheckForMatch());
            }
        }
    }

    private IEnumerator CheckForMatch()
    {
        yield return new WaitForSeconds(1f);

        if (FlippedCards[0].id == FlippedCards[1].id)
        {
            //SoundPlay(SomMatch);
            //Remove as cartas que deram match da lista de cards disponíveis:
            AllRemainingCards.Remove(FlippedCards[0]);
            AllRemainingCards.Remove(FlippedCards[1]);

            //Seta as informações da match na popupwindow:
            popupWindowCardImage.sprite = CardData[FlippedCards[0].id].cardSprite;
            popupWindowCardTitle.text = CardData[FlippedCards[0].id].cardName;
            popupWindowCardDescription.text = CardData[FlippedCards[0].id].cardDescription;

            FlippedCards[0].Match();
            FlippedCards[1].Match();

            //Aumento dos pontos:
            var actualPoints = artifactController.artifactPoints.GetPoints();
            artifactController.artifactPoints.SetPoints(actualPoints + 1);

            //Alteração visual do elemento gráfico responsável por essa alteração;
            artifactPointsText.text = artifactController.artifactPoints.GetPoints().ToString();

            MatchedCards.Add(FlippedCards[0]);
            MatchedCards.Add(FlippedCards[1]);

            StartCoroutine(PopUpInAnimations());

            //if (MatchedCards.Count == GridSizeX * GridSizeY)
            //{
            //    StartCoroutine(WinCoroutine());
            //}
        }
        else
        {
            FlippedCards[0].StartCoroutine((FlippedCards[0].Flip()));
            FlippedCards[1].StartCoroutine((FlippedCards[1].Flip()));
            //SoundPlay(SomUnmatch);
        }

        FlippedCards.Clear();

        if (artifactController.artifactPoints.GetPoints() >= 5 && !exchangeArtifactPoints.interactable)
            exchangeArtifactPoints.interactable = true;
    }

    private IEnumerator PopUpInAnimations()
    {
        popupFadeAnim.Play("FadeIn");
        yield return new WaitForSeconds(.10f);
        popupWindowAnim.Play("PopupWindowIn");
    }

    private IEnumerator PopUpOutAnimations()
    {
        popupFadeAnim.Play("FadEOut");
        yield return new WaitForSeconds(.10f);
        popupWindowAnim.Play("PopupWindowOut");

        if (MatchedCards.Count == GridSizeX * GridSizeY)
        {
            StartCoroutine(WinCoroutine());
        }
    }

    private IEnumerator WinCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        yield return new WaitUntil(() =>
            !FindFirstObjectByType<ToastScript>() && !FindFirstObjectByType<CardController>());
        Debug.Log("ganhou");

        levelLoaderController.LoadLevelWithIndex(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SoundPlay(AudioClip audio)
    {
        //        AudioSource.PlayOneShot(audio);
    }

    public void ClosePopupButton()
    {
        StartCoroutine(PopUpOutAnimations());

        if (MatchedCards.Count == GridSizeX * GridSizeY)
        {
            StartCoroutine(WinCoroutine());
        }
    }

    public void ExchangePoints()
    {
        var actualPoints = artifactController.artifactPoints.GetPoints() - 5;
        artifactController.artifactPoints.SetPoints(actualPoints);

        //Alteração visual do elemento gráfico responsável por essa alteração;
        artifactPointsText.text = artifactController.artifactPoints.GetPoints().ToString();

        if (artifactController.artifactPoints.GetPoints() < 5 && exchangeArtifactPoints.interactable)
            exchangeArtifactPoints.interactable = false;

        var index = Random.Range(0, AllRemainingCards.Count);

        var cardOne = AllRemainingCards[index];
        var cardTwo = AllRemainingCards.Where(x => x.id == cardOne.id && x != cardOne).FirstOrDefault();

        //Flipa as duas cartas simultaneamente
        FlipCard(cardOne, true);
        FlipCard(cardTwo, true);
    }
}