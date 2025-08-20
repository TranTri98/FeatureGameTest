using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI centerText;
    [SerializeField] private Button btnRestart;

    private void Start()
    {
        centerText.gameObject.SetActive(false);
        btnRestart.gameObject.SetActive(false); 
        btnRestart.onClick.AddListener(RestartGame);
    }

    private void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
        Time.timeScale = 1.0f;
        GameManager.Instance.State = GameManager.GameState.Playing;
    }

    private void OnEnable()
    {
        GameManager.OnGameWon += ShowWin;
        GameManager.OnGameLost += ShowLose;
    }
    private void OnDisable()
    {
        GameManager.OnGameWon -= ShowWin;
        GameManager.OnGameLost -= ShowLose;
    }

    private void ShowWin()
    {
        btnRestart.gameObject.SetActive(true);
        centerText.gameObject.SetActive(true);
        centerText.text = "YOU ESCAPED!";
    }
    private void ShowLose()
    {
        btnRestart.gameObject.SetActive(true);
        centerText.gameObject.SetActive(true);
        centerText.text = "YOU DIED";
    }  
}
