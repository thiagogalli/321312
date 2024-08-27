using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueTestScreenController : MonoBehaviour
{
    [SerializeField] DialogueController dialogueController;
    [SerializeField] Dialogue dialogueTest;

    [SerializeField] GameObject btnNextSentence;
    [SerializeField] GameObject btnBackToMainMenu;
    [SerializeField] GameObject btnSkipDialogue;

    [SerializeField] TextMeshProUGUI btnNextSentenceText;

    [SerializeField] LevelLoaderController levelLoaderController;

    ArtifactPointsController artifactPointsController;
    TouchPointsController touchPointsController;
    PlayerNameDefinitionController playerNameController;

    async void Start()
    {
        btnSkipDialogue.SetActive(false);
        await Awaitable.WaitForSecondsAsync(1f);
        dialogueController.OpenDialogueAnim();
        await Awaitable.WaitForSecondsAsync(1.5f);
        await dialogueController.NextSentence(dialogueTest);
        btnNextSentence.SetActive(true);
        btnSkipDialogue.SetActive(true);
    }

    public async void ClickNextSentence()
    {
        btnNextSentence.SetActive(false);
        btnSkipDialogue.SetActive(false);
        await dialogueController.NextSentence(dialogueTest);

        if (dialogueController.GetIndex() != 0)
        {
            btnNextSentence.SetActive(true);
            btnSkipDialogue.SetActive(true);
        }

        if (dialogueTest.GetDialogue.Length == dialogueController.GetIndex())
        {
            if (SceneManager.GetActiveScene().buildIndex == (int)Enums.Scenes.LevelThreeFinalScreen)
                btnNextSentenceText.text = "CONTINUAR";
            else
            {
                btnNextSentenceText.text = "VAMOS LÁ!";
                btnBackToMainMenu.SetActive(true);
            }
        }
        else if (dialogueController.GetIndex() == 0)
            await AdvanceToNextSceneInBuildIndex();
    }
    public async void SkipDialogue()
    {
        btnSkipDialogue.SetActive(false);
        await AdvanceToNextSceneInBuildIndex();
    }

    public async void BackToMainMenu()
    {
        artifactPointsController = GameObject.FindGameObjectWithTag("ArtifactController")?.GetComponent<ArtifactPointsController>();
        touchPointsController = GameObject.FindGameObjectWithTag("TouchController")?.GetComponent<TouchPointsController>();
        //playerNameController = GameObject.FindGameObjectWithTag("PlayerNameDefinition")?.GetComponent<PlayerNameDefinitionController>();

        if (artifactPointsController != null)
            Destroy(artifactPointsController.gameObject);

        if (touchPointsController != null)
            Destroy(touchPointsController.gameObject);

        //if (playerNameController != null)
        //    Destroy(playerNameController);

        await levelLoaderController.LoadLevelAsync(Enums.Scenes.InitialScreen);
    }

    public async Task AdvanceToNextSceneInBuildIndex()
    {
        await levelLoaderController.LoadLevelWithIndexAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
