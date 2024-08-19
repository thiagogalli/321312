using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] LevelLoaderController levelLoader;
    
    public async void TutorialScreen()
    {
        await levelLoader.LoadLevelAsync(Enums.Scenes.TutorialScreen);
    }

    public async void CreditsScreen()
    {
        await levelLoader.LoadLevelAsync(Enums.Scenes.CreditsScreen);
    }

    public void ExitGame()
    {
        Debug.Log("Fechou a aplicação");
        Application.Quit();
    }

    public async void InitialDialogScreen()
    {
        await levelLoader.LoadLevelAsync(Enums.Scenes.InitialDialogScreen);
    }
}
