using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// this is too basic to make up something of it
public class OnboardingMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
