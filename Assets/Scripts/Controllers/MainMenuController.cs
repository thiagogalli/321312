using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] LevelLoaderController levelLoader;
    [SerializeField] DB db;
    [SerializeField] MainMenuView mainMenuView;

    string nameText;
    public List<string> playerList;

    public void Start()
    {
        playerList = new List<string>();
        PopulateNames();
    }

    public void PopulateNames()
    {
        playerList = db.GetPlayers();
        mainMenuView.PopulateDropdown(playerList);
    }

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

    public void CreateNewPlayer()
    {
        nameText = mainMenuView.GetNameInput().text;

        if (nameText == string.Empty)
        {
            mainMenuView.SetTextText(mainMenuView.GetTextException(), "Campo obrigatório");
            mainMenuView.GetTextExceptionGameObject().SetActive(true);
        }
        else
        {
            var exists = db.CheckIfPlayerExists(nameText);

            if (!exists)
            {
                db.InsertPlayer(nameText);

                PopulateNames();

                mainMenuView.GetTextExceptionGameObject().SetActive(false);
            }
            else if (!mainMenuView.GetTextExceptionGameObject().activeSelf)
            {
                mainMenuView.SetTextText(mainMenuView.GetTextException(), "Jogador já existe!");
                mainMenuView.GetTextExceptionGameObject().SetActive(true);
            }
        }
    }
}
