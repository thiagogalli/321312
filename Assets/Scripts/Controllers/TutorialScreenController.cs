using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreenController : MonoBehaviour
{
    [SerializeField] LevelLoaderController levelLoader;

    public async void BackToMainMenu()
    {
        await levelLoader.LoadLevelAsync(Enums.Scenes.InitialScreen);
    }
}
