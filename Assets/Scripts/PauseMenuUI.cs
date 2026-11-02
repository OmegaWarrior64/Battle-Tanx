using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    private Button pauseButton;
    private Button resumeButton;
    private Button quitButton;

    private GameObject pauseMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseButton = transform.Find("Pause Button").GetComponent<Button>();
        pauseMenu = transform.Find("Pause Menu").gameObject;

        resumeButton = pauseMenu.transform.Find("Resume Button").GetComponent<Button>();
        quitButton = pauseMenu.transform.Find("Quit Button").GetComponent<Button>();

        pauseButton.onClick.AddListener(() =>
        {
            GameManager.Instance.Pause();
            pauseMenu.SetActive(true);
        });
        
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.UnPause();
            pauseMenu.SetActive(false);
        });

        quitButton.onClick.AddListener(() => GameManager.Instance.TransitionToScene("Main Menu"));
    }
}
