using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playButton.onClick.AddListener(() => GetGameManager().TransitionToScene("Level"));
        quitButton.onClick.AddListener(() => Application.Quit());
    }

    private GameManager GetGameManager()
    {
        if (gameManager == null) gameManager = FindAnyObjectByType<GameManager>();
        return gameManager;
    }
}
