using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    private Button pauseButton;
    private Button resumeButton;
    private Button quitButton;

    private GameObject pauseMenu;

    private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseButton = transform.Find("Pause Button").GetComponent<Button>();
        pauseMenu = transform.Find("Pause Menu").gameObject;

        resumeButton = pauseMenu.transform.Find("Resume Button").GetComponent<Button>();
        quitButton = pauseMenu.transform.Find("Quit Button").GetComponent<Button>();

        pauseButton.onClick.AddListener(() =>
        {
            GetGameManager().Pause();
            pauseMenu.SetActive(true);
        });
        
        resumeButton.onClick.AddListener(() =>
        {
            GetGameManager().UnPause();
            pauseMenu.SetActive(false);
        });

        quitButton.onClick.AddListener(() => GetGameManager().TransitionToScene("Main Menu"));
    }

    private GameManager GetGameManager()
    {
        if (gameManager == null) gameManager = FindAnyObjectByType<GameManager>();
        return gameManager;
    }
}
