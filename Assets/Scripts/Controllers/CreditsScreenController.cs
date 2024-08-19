using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScreenController : MonoBehaviour
{
    [SerializeField] LevelLoaderController LevelLoaderController;

    public async void BackToInitialScreen()
    {
        await LevelLoaderController.LoadLevelAsync(Enums.Scenes.InitialScreen);
    }
}
