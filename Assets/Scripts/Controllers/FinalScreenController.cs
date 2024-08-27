using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalScreenController : MonoBehaviour
{
    [SerializeField] LevelLoaderController levelLoaderController;

    TouchPointsController touchPointsController;
    ArtifactPointsController artifactPointsController;
    PlayerNameDefinitionController playerNameDefinitionController;
    [SerializeField] TextMeshProUGUI textTouchTotal;
    [SerializeField] TextMeshProUGUI firstPosName;
    [SerializeField] TextMeshProUGUI secondPosName;
    [SerializeField] TextMeshProUGUI thirdPosName;

    [SerializeField] TextMeshProUGUI firstPosPoints;
    [SerializeField] TextMeshProUGUI secondPosPoints;
    [SerializeField] TextMeshProUGUI thirdPosPoints;

    [SerializeField] DB db;

    public void Start()
    {
        artifactPointsController = GameObject.FindGameObjectWithTag("ArtifactController").GetComponent<ArtifactPointsController>();
        touchPointsController = GameObject.FindGameObjectWithTag("TouchController").GetComponent<TouchPointsController>();
        playerNameDefinitionController = GameObject.FindGameObjectWithTag("PlayerNameDefinition").GetComponent<PlayerNameDefinitionController>();
        textTouchTotal.text = touchPointsController.touchPoints.GetQuantityOfTouches().ToString();

        var bestScores = db.InsertDBAndSearch(playerNameDefinitionController.GetScoreboard().name, playerNameDefinitionController.GetScoreboard().points);

        firstPosName.text = bestScores[0].name;
        firstPosPoints.text = bestScores[0].points.ToString();

        if (bestScores.Count > 1)
        {
            secondPosName.text = bestScores[1].name;
            secondPosPoints.text = bestScores[1].points.ToString();
        }
        
        if (bestScores.Count > 2)
        {
            thirdPosName.text = bestScores[2].name;
            thirdPosPoints.text = bestScores[2].points.ToString();
        }
    }
    public void BackToMainMenu()
    {
        Destroy(artifactPointsController.gameObject);
        Destroy(touchPointsController.gameObject);
        Destroy(playerNameDefinitionController.gameObject);
        levelLoaderController.LoadLevel(Enums.Scenes.InitialScreen);
    }
}
