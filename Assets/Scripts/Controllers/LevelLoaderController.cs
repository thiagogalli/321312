using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderController : MonoBehaviour
{
    [SerializeField] Animator transitionAnim;
    [SerializeField] float transitionTime = 0.5f;

    public async Task LoadLevelAsync(Enums.Scenes sceneIndex)
    {
        transitionAnim.SetTrigger("Start");
        await Awaitable.WaitForSecondsAsync(transitionTime);
        SceneManager.LoadScene((int)sceneIndex);
    }

    public async Task LoadLevelWithIndexAsync(int index)
    {
        transitionAnim.SetTrigger("Start");
        await Awaitable.WaitForSecondsAsync(transitionTime);
        SceneManager.LoadScene(index);
    }

    public void LoadLevel(Enums.Scenes sceneIndex)
    {
        StartCoroutine(LoadLevelCoroutine(sceneIndex));
    }
    public void LoadLevelWithIndex(int index)
    {
        StartCoroutine(LoadLevelWithIndexCoroutine(index));
    }

    IEnumerator LoadLevelCoroutine(Enums.Scenes sceneIndex)
    {
        transitionAnim.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene((int)sceneIndex);
    }

    IEnumerator LoadLevelWithIndexCoroutine(int index)
    {
        transitionAnim.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(index);
    }
}
