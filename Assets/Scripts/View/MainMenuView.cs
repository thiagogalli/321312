using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdownPlayersNames;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TextMeshProUGUI textException;
    [SerializeField] GameObject textExceptionGameObject;
    [SerializeField] Button btnStartGame;
    PlayerNameDefinitionController playerNameDefinition;

    public TMP_InputField GetNameInput() => nameInput;

    public TextMeshProUGUI GetTextException()
    {
        return textException;
    }
    public GameObject GetTextExceptionGameObject()
    {
        return textExceptionGameObject;
    }
    public void SetTextText(TextMeshProUGUI textTMPro, string text)
    {
        textTMPro.text = text;
    }

    public void Start()
    {
        playerNameDefinition = GameObject.FindGameObjectWithTag("PlayerNameDefinition").GetComponent<PlayerNameDefinitionController>();
        SetPlayerNameAfterSelection();
    }

    public void PopulateDropdown(List<string> playerList)
    {
        var selectedOption = new TMP_Dropdown.OptionData();

        if (dropdownPlayersNames.options.Count > 0)
            selectedOption = dropdownPlayersNames.options[dropdownPlayersNames.value];

        dropdownPlayersNames.ClearOptions();

        foreach (var element in playerList)
            dropdownPlayersNames.options.Add(new TMP_Dropdown.OptionData(element));

        var dropdownValue = GetDropdownValueByOptionText(selectedOption.text);

        if (selectedOption.text != null &&
            selectedOption.text != string.Empty
            && dropdownValue != -1)
        {
            dropdownPlayersNames.value = dropdownValue;
            SetPlayerNameAfterSelection();
        }
        else if (dropdownPlayersNames.options.Count > 0 && !dropdownPlayersNames.options.Any(x => x.text == selectedOption.text))
        {
            dropdownPlayersNames.value = 0;
            SetPlayerNameAfterSelection();
        }

        dropdownPlayersNames.RefreshShownValue();
    }

    private int GetDropdownValueByOptionText(string optionText)
    {
        for (int i = 0; i < dropdownPlayersNames.options.Count; i++)
        {
            if (dropdownPlayersNames.options[i].text == optionText)
                return i;
        }
        return -1; // Retorna -1 se não encontrar correspondência
    }

    public void SetPlayerNameAfterSelection()
    {
        if (dropdownPlayersNames.options.Count > 0)
        {
            var playerName = dropdownPlayersNames.options[dropdownPlayersNames.value].text;
            playerNameDefinition.GetScoreboard().name = playerName;
        }
        
        EnableDisableStartButton();
    }
    public void EnableDisableStartButton()
    {
        if (dropdownPlayersNames.options.Count > 0 && playerNameDefinition.name != string.Empty && playerNameDefinition.name != null && playerNameDefinition.name != "")
            btnStartGame.interactable = true;
        else
            btnStartGame.interactable = false;
    }
}
