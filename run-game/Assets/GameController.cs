using TMPro;
using UnityEngine;
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

    private int leftFootPresses = 0;
    private int rightFootPresses = 0;
    private int pressesRequired;
    private int currentStage = 1;

    void Start()
    {
        nextStageButton.gameObject.SetActive(false);
        victoryMessage.gameObject.SetActive(false);
        SetStage(currentStage);

        leftFootButton.onClick.AddListener(OnLeftFootPressed);
        rightFootButton.onClick.AddListener(OnRightFootPressed);
        //nextStageButton.onClick.AddListener(NextStage);
    }
    void UpdateStepsText()
    {
        int totalSteps = (leftFootPresses + rightFootPresses) / 2;
        stepsText.text = $"Steps: {totalSteps}/{pressesRequired}";
    }

    //!!!funtion left foot and right foot could just combine for 1 function no
    //update later !
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
        pressesRequired = stage * 10;
        leftFootPresses = 0;
        rightFootPresses = 0;
        progressBar.value = 0;
        mask.fillAmount = 0;
        UpdateStepsText();
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
}
