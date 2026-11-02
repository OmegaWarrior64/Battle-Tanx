using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<GameManager>();
                if (instance == null)
                {
                    var go = new GameObject("Game Manager");
                    instance = go.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    public event Action ScoreChanged;

    private int currentScore;
    public int CurrentScore
    {
        get => currentScore;

        set
        {
            currentScore = value;
            ScoreChanged?.Invoke();
        }
    }

    private InputActionMap playerInput;
    private UIFader loadingScreen;


    void Awake()
    {
        transform.SetParent(null);
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        playerInput = InputSystem.actions.FindActionMap("Player");
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
        GetLoadingScreen().SetInputBlocking(true);
        GetLoadingScreen().FadeOut();
        yield return new WaitForSecondsRealtime(GetLoadingScreen().GetFadeDuration());

        SceneManager.LoadScene(sceneName);
        yield return null;
        
        GetLoadingScreen().FadeIn();
        yield return new WaitForSecondsRealtime(GetLoadingScreen().GetFadeDuration());
        GetLoadingScreen().SetInputBlocking(false);
        UnPause();
    }
}
