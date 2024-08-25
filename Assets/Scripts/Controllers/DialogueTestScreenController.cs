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

    [SerializeField] TextMeshProUGUI btnNextSentenceText;

    [SerializeField] LevelLoaderController levelLoaderController;

    ArtifactPointsController artifactPointsController;
    TouchPointsController touchPointsController;
    PlayerNameDefinitionController playerNameController;

    async void Start()
    {
        await Awaitable.WaitForSecondsAsync(1f);
        dialogueController.OpenDialogueAnim();
        await Awaitable.WaitForSecondsAsync(1.5f);
        await dialogueController.NextSentence(dialogueTest);
        btnNextSentence.SetActive(true);
    }

    public async void ClickNextSentence()
    {
        btnNextSentence.SetActive(false);
        await dialogueController.NextSentence(dialogueTest);

        if (dialogueController.GetIndex() != 0)
            btnNextSentence.SetActive(true);

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

    public async void BackToMainMenu()
    {
        artifactPointsController = GameObject.FindGameObjectWithTag("ArtifactController").GetComponent<ArtifactPointsController>();
        touchPointsController = GameObject.FindGameObjectWithTag("TouchController").GetComponent<TouchPointsController>();
        playerNameController = GameObject.FindGameObjectWithTag("PlayerNameDefinition").GetComponent<PlayerNameDefinitionController>();

        if (artifactPointsController != null)
            Destroy(artifactPointsController);

        if (touchPointsController != null)
            Destroy(touchPointsController);

        if (playerNameController != null)
            Destroy(playerNameController);

        await levelLoaderController.LoadLevelAsync(Enums.Scenes.InitialScreen);
    }

    public async Task AdvanceToNextSceneInBuildIndex()
    {
        await levelLoaderController.LoadLevelWithIndexAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
