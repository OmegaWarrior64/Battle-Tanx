using TMPro;
using UnityEngine;

public class ScoreCounterUI : MonoBehaviour
{
    private TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        
        GameManager.Instance.ScoreChanged += () =>
        {
            text.text = "Score: " + GameManager.Instance.CurrentScore;
        };
    }
}
