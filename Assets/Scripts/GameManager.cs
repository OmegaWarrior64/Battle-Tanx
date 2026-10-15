using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private InputActionMap playerInput;
    private UIFader loadingScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetLoadingScreen().FadeIn();
        playerInput = InputSystem.actions.FindActionMap("Player");
        DontDestroyOnLoad(gameObject);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        playerInput.Disable();
        AudioListener.pause = true;
    }

    public void UnPause()
    {
        Time.timeScale = 1f;
        playerInput.Enable();
        AudioListener.pause = false;
    }

    public void TransitionToScene(string sceneName) => StartCoroutine(TransitionCoroutine(sceneName));

    private UIFader GetLoadingScreen()
    {
        if (loadingScreen == null) loadingScreen = FindAnyObjectByType<UIFader>();
        return loadingScreen;
    }

    private IEnumerator TransitionCoroutine(string sceneName)
    {
        Pause();
        GetLoadingScreen().FadeOut();
        GetLoadingScreen().SetInputBlocking(true);
        yield return new WaitForSecondsRealtime(GetLoadingScreen().GetFadeDuration());

        SceneManager.LoadScene(sceneName);
        yield return null;
        
        GetLoadingScreen().FadeIn();
        yield return new WaitForSecondsRealtime(GetLoadingScreen().GetFadeDuration());
        GetLoadingScreen().SetInputBlocking(false);
        UnPause();
    }
}
