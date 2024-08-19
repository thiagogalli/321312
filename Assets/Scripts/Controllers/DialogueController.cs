using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] float dialogSpeed;
    [SerializeField] Animator dialogueBoxAnim;
    [SerializeField] Animator dialogueAvatarAnim;

    int index;
    public int GetIndex() => index;

    // Start is called before the first frame update
    void Start()
    {
        dialogText.text = string.Empty;
        index = 0;
    }

    public void OpenDialogueAnim()
    {
        dialogueBoxAnim.Play("DialogueBoxIn");
        dialogueAvatarAnim.Play("AvatarIn");
    }

    private void CloseDialogueAnim()
    {
        dialogueBoxAnim.Play("DialogueBoxOut");
        dialogueAvatarAnim.Play("AvatarOut");
    }

    public async Task NextSentence(Dialogue dialogueObject)
    {
        if (index <= dialogueObject.GetDialogue.Length - 1)
        {
            var sceneIndex = SceneManager.GetActiveScene().buildIndex;
            var postAnimationString = string.Empty;

            if (sceneIndex == (int)Enums.Scenes.InitialDialogScreen)
            {
                dialogueAvatarAnim.Play("AvatarTalkingInit");
                postAnimationString = "AvatarIdleInScreen";
            }
            else if (sceneIndex == (int)Enums.Scenes.LevelOneStartScreen || sceneIndex == (int)Enums.Scenes.LevelOneFinalScreen)
            {
                dialogueAvatarAnim.Play("AvatarTalkingLevel1");
                postAnimationString = "AvatarIdleLevel1";
            }
            else if (sceneIndex == (int)Enums.Scenes.LevelTwoStartScreen || sceneIndex == (int)Enums.Scenes.LevelTwoFinalScreen)
            {
                dialogueAvatarAnim.Play("AvatarTalkingLevel2");
                postAnimationString = "AvatarIdleLevel2";
            }
            else if (sceneIndex == (int)Enums.Scenes.LevelThreeStartScreen || sceneIndex == (int)Enums.Scenes.LevelThreeFinalScreen)
            {
                dialogueAvatarAnim.Play("AvatarTalkingLevel3");
                postAnimationString = "AvatarIdleLevel3";
            }

            dialogText.text = string.Empty;
            await TypeWriterEffect(dialogueObject);
            dialogueAvatarAnim.Play(postAnimationString);
            
        }
        else
        {
            CloseDialogueAnim();
            index = 0;
        }
    }

    public async Task TypeWriterEffect(Dialogue dialogueObject)
    {
        foreach (char character in dialogueObject.GetDialogue[index].ToCharArray())
        {
            dialogText.text += character;
            await Awaitable.WaitForSecondsAsync(dialogSpeed);
        }
        index++;
    }
}
