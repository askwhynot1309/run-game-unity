using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Slider progressBar;
    public Button leftFootButton;
    public Button rightFootButton;
    public TextMeshProUGUI victoryMessage;
    public Button nextStageButton;
    public TextMeshProUGUI stepsText;
    public Image mask;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI stageText;
    public GameObject timeUpPanel;
    public TextMeshProUGUI finalStageText;
    public Button restartButton;
    public TextMeshProUGUI highStageText;

    private int leftFootPresses = 0;
    private int rightFootPresses = 0;
    private int pressesRequired;
    private int currentStage = 1;
    private bool isGameActive = true;
    private float timeRemaining = 6f;


    void Start()
    {
        SoundManager.Instance.PlayMusic();
        nextStageButton.gameObject.SetActive(false);
        victoryMessage.gameObject.SetActive(false);
        timeUpPanel.SetActive(false);
        SetStage(currentStage);
        stageText.text = "Stage: " + currentStage;

        restartButton.onClick.AddListener(RestartGame);
        leftFootButton.onClick.AddListener(OnLeftFootPressed);
        rightFootButton.onClick.AddListener(OnRightFootPressed);
        //nextStageButton.onClick.AddListener(NextStage);
        StartCoroutine(GameAPI.Instance.GetHighScore(
        score =>
        {
            Debug.Log("Fetched high score: " + score);
            highStageText.text = "Best stage: " + score.ToString();
        },
        error =>
        {
            Debug.LogError("Failed to fetch high score: " + error);
        }));
    }
    void UpdateStepsText()
    {
        int totalSteps = (leftFootPresses + rightFootPresses) / 2;
        stepsText.text = $"Steps: {totalSteps}/{pressesRequired}";
    }

    public void OnLeftFootPressed()
    {
        if (progressBar.value < 1f)
        {
            leftFootPresses++;
            UpdateProgress();
            UpdateStepsText();
        }
    }

    public void OnRightFootPressed()
    {
        if (progressBar.value < 1f)
        {
            rightFootPresses++;
            UpdateProgress();
            UpdateStepsText();
        }
    }

    void UpdateProgress()
    {
        float totalPresses = (leftFootPresses + rightFootPresses)/2;
        progressBar.value = totalPresses / pressesRequired;
        mask.fillAmount = totalPresses / pressesRequired;

        if (progressBar.value >= 1f)
        {
            Victory();
        }
    }

    void SetStage(int stage)
    {
        pressesRequired = stage * 7;
        leftFootPresses = 0;
        rightFootPresses = 0;
        progressBar.value = 0;
        mask.fillAmount = 0;
        UpdateStepsText();
        stageText.text = "Stage: " + currentStage;
    }

    void Victory()
    {
        victoryMessage.gameObject.SetActive(true);
        nextStageButton.gameObject.SetActive(true);
    }

    void NextStage()
    {
        currentStage++;
        SetStage(currentStage);
        victoryMessage.gameObject.SetActive(false);
        nextStageButton.gameObject.SetActive(false);
    }
    public void OnNextStagePressed()
    {
        NextStage();
    }

    void EndGame()
    {
        isGameActive = false;
        timeUpPanel.SetActive(true);
        finalStageText.text = "Final Stage: " + currentStage;

        StartCoroutine(GameAPI.Instance.PostPlayHistory(currentStage,
                    onSuccess: () =>
                    {
                        Debug.Log("Score posted successfully.");
                    },
                    onError: (error) =>
                    {
                        Debug.LogError($"Failed to post score: {error}");
                    }));
    }

    void Update()
    {
        if (!isGameActive) return;

        timeRemaining -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining).ToString();

        if (timeRemaining <= 0)
        {
            EndGame();
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
