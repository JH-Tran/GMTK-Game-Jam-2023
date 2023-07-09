using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoalManager : MonoBehaviour
{
    private bool isTwoPictureObjective = false;
    [SerializeField] private bool generatingTwoPictureObjective = false;
    private bool isTwoFinishLine = false;

    private List<PictureObjective> pictureObjectiveRight = new List<PictureObjective>();
    private List<PictureObjective> pictureObjectiveLeft = new List<PictureObjective>();
    //Timer
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Slider timerSliderRight;
    [SerializeField] private Slider timerSliderLeft;

    [SerializeField] private float gameTimeRight = 100;
    [SerializeField] private float gameTimeLeft = 100;
    private float maxTimeValue = 40;
    private float mistakeTimeLoss = 7;
    [SerializeField] private bool stopTimer;
    [SerializeField] private bool stopTimerLeft = true;
    //Flavour text
    [SerializeField] private TMP_Text promptsText;
    [SerializeField] private TMP_Text imageCompletedText;
    [SerializeField] int completedImages = 0;
    [SerializeField] int currentImageGoal = 10;
    //10, 20, 40, 64
    [SerializeField] int imageGoal1 = 10;
    [SerializeField] int imageGoal2 = 30;
    [SerializeField] int endGameGoal = 64;
    private List<string> errorSentences = new List<string>
    {
        "User: What I have to refresh my page?!",
        "User: Internet connection error? I'll try again.",
        "User: Why are there so many pop-ups? I'm going to reload the page."
    };
    private List<string> completedSentences = new List<string> {
        "User: Thanks DPU!",
        "User: WOW! This works",
        "User: Nice! This is what I was looking for.",
        "User: ty!"
    };
    private List<string> mistakeSentences = new List<string> {
        "User: This isn't what I wanted, try again",
        "User: What?! This isn't what I asked for! Try again",
        "User: ...",
        "User: Wow, is this DPU still new?",
        "User: This isn't even close!"
    };
    //Extra Finish Machine && Button
    [SerializeField] private GameObject finishMachineAndButtons;
    [SerializeField] private TMP_Text leftPromptsText;
    [SerializeField] private TMP_Text rightPromptsText;
    //Burning Floor
    [SerializeField] private GameObject fireFloorObjects;
    //Game Over
    [SerializeField] GameObject GameOverCanvas;
    [SerializeField] GameObject GameOverButtonGroup;
    //Pause game
    [SerializeField] GameObject pauseGameMenu;
    private bool isGamePause = false;
    private bool endGame = false;
    //End game
    [SerializeField] GameObject EndScreen;

    private void Awake()
    {
        Initalise();
    }

    public void Initalise()
    {
        isGamePause = false;
        endGame = false;
        CreateObjective(ScreenSide.None);
        SetImageCompletedText();
        currentImageGoal = imageGoal1;
        generatingTwoPictureObjective = false;
        completedImages = 0;
        gameTimeRight = maxTimeValue;
        isTwoFinishLine = false;
        stopTimer = false;
        timerSlider.maxValue = maxTimeValue;
        timerSlider.value = gameTimeRight;
        timerSliderRight.maxValue = maxTimeValue;
        timerSliderLeft.maxValue = maxTimeValue;
        GameOverCanvas.SetActive(false);
        GameOverButtonGroup.SetActive(false);
        SetActiveText(true, false);
        SetActiveSliders(true, false);
        finishMachineAndButtons.SetActive(false);
        fireFloorObjects.SetActive(false);
        pauseGameMenu.SetActive(false);
        EndScreen.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && endGame == false)
        {
            OnButtonPause();
        }
        if (isGamePause == true)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void OnButtonPause()
    {
        isGamePause = !isGamePause;
        pauseGameMenu.SetActive(isGamePause);
    }
    void FixedUpdate()
    {
        if (gameTimeRight <= 0 && endGame == false)
        {
            stopTimer = true;
            if (isTwoFinishLine == false)
                timerSlider.value = 0;
            else
                timerSliderRight.value = 0;
            GameOver();
        }
        if (gameTimeLeft <= 0 && endGame == false)
        {
            stopTimer = true;
            timerSliderLeft.value = 0;
            GameOver();
        }
        if (stopTimer == false && isTwoFinishLine == false)
        {
            gameTimeRight -= Time.deltaTime;
            timerSlider.value = gameTimeRight;
        }
        if (stopTimer == false && isTwoFinishLine == true)
        {
            gameTimeRight -= Time.deltaTime;
            timerSliderRight.value = gameTimeRight;
        }
        if (stopTimerLeft == false && isTwoFinishLine == true)
        {
            gameTimeLeft -= Time.deltaTime;
            timerSliderLeft.value = gameTimeLeft;
        }
    }
    private void UpdateObjectiveText(TMP_Text TMP)
    {
        if ((isTwoPictureObjective == false && TMP == promptsText) || (isTwoPictureObjective == false && TMP == rightPromptsText))
        {
            TMP.text = $"User: Create a {pictureObjectiveRight[0]._PictureColour.ToString().ToLower()} canvas";
        }
        else if((isTwoPictureObjective == true && TMP == promptsText) || (isTwoPictureObjective == true && TMP == rightPromptsText))
        {
            TMP.text = $"User: Create a {pictureObjectiveRight[1]._PictureColour.ToString().ToLower()} {pictureObjectiveRight[1]._PictureShape.ToString().ToLower()} with a {pictureObjectiveRight[0]._PictureColour.ToString().ToLower()} background";
        }
        else if ((isTwoPictureObjective == false && TMP == leftPromptsText))
        {
            TMP.text = $"User: Create a {pictureObjectiveLeft[0]._PictureColour.ToString().ToLower()} canvas";
        }
        else if ((isTwoPictureObjective == true && TMP == leftPromptsText))
        {
            TMP.text = $"User: Create a {pictureObjectiveLeft[1]._PictureColour.ToString().ToLower()} {pictureObjectiveLeft[1]._PictureShape.ToString().ToLower()} with a {pictureObjectiveLeft[0]._PictureColour.ToString().ToLower()} background";
        }
    }
    private void ObjectiveCompleteText(ScreenSide screenSide)
    {
        int ranNum = Random.Range(0, completedSentences.Count);
        if (screenSide == ScreenSide.None)
        {
            promptsText.text = completedSentences[ranNum];
            gameTimeRight = maxTimeValue;
        }
        else if (screenSide == ScreenSide.Left)
        {
            leftPromptsText.text = completedSentences[ranNum];
            gameTimeLeft = maxTimeValue;
        }
        else if (screenSide == ScreenSide.Right)
        {
            rightPromptsText.text = completedSentences[ranNum];
            gameTimeRight = maxTimeValue;
        }
        if (screenSide == ScreenSide.Left)
        {
            stopTimerLeft = true;
        }
        else
        {
            stopTimer = true;
        }
    }
    public bool VerifyRequirement(Picture[] image, ScreenSide screenSide = ScreenSide.None)
    {
        int imageListCounter = image.Length - 1;
        if (screenSide == ScreenSide.Left)
        {
            if (image.Length < pictureObjectiveLeft.Count || image.Length > pictureObjectiveLeft.Count) { return false; }
            foreach (PictureObjective picture in pictureObjectiveLeft)
            {
                if (image[imageListCounter].GetColor() != picture._PictureColour || image[imageListCounter].GetShape() != picture._PictureShape)
                {
                    Debug.Log($"Image: {image[imageListCounter].GetColor()} || {image[imageListCounter].GetShape()}");
                    Debug.Log($"Criteria: {picture._PictureColour} || {picture._PictureShape} || {pictureObjectiveLeft.Count}");
                    StartCoroutine(PlayerIncorrectImage(screenSide));
                    return false;
                }
                imageListCounter--;
            }
            completedImages++;
            StartCoroutine(PlayerCompleteObjective(screenSide));
            return true;
        }
        if (image.Length < pictureObjectiveRight.Count || image.Length > pictureObjectiveRight.Count) { return false; }
        if (isTwoFinishLine == false) { screenSide = ScreenSide.None; } 
        foreach (PictureObjective picture in pictureObjectiveRight)
        {
            if (image[imageListCounter].GetColor() != picture._PictureColour || image[imageListCounter].GetShape() != picture._PictureShape)
            {
                Debug.Log($"Image: {image[imageListCounter].GetColor()} || {image[imageListCounter].GetShape()}") ;
                Debug.Log($"Criteria: {picture._PictureColour} || {picture._PictureShape} || {pictureObjectiveRight.Count}");
                StartCoroutine(PlayerIncorrectImage(screenSide));
                return false;
            }
            imageListCounter--;
        }
        completedImages++;
        StartCoroutine(PlayerCompleteObjective(screenSide));
        return true;
    }
    private IEnumerator PlayerCompleteObjective(ScreenSide screenSide)
    {
        ObjectiveCompleteText(screenSide);
        SetImageCompletedText();
        if (screenSide == ScreenSide.Right || screenSide == ScreenSide.None)
            pictureObjectiveRight.Clear();
        else if (screenSide == ScreenSide.Left)
            pictureObjectiveLeft.Clear();
        if (completedImages >= 3)
        {
            generatingTwoPictureObjective = true;
        }
        yield return new WaitForSeconds(2);
        if (completedImages >= currentImageGoal)
        {
            ProgressToNextPhase();
        }
        else
        {
            CreateObjective(screenSide);
            if (screenSide == ScreenSide.Left)
            {
                stopTimerLeft = false;
            }
            else
            {
                stopTimer = false;
            }
        }
    }
    private IEnumerator PlayerIncorrectImage(ScreenSide screenSide)
    {
        ObjectiveMistake(screenSide);
        yield return new WaitForSeconds(2);
        if (screenSide == ScreenSide.None)
        {
            UpdateObjectiveText(promptsText);
        }
        else if (screenSide == ScreenSide.Left)
        {
            UpdateObjectiveText(leftPromptsText);
        }
        else if (screenSide == ScreenSide.Right)
        {
            UpdateObjectiveText(rightPromptsText);
        }
    }
    private void ObjectiveMistake(ScreenSide screen)
    {
        int ranNum = Random.Range(0, mistakeSentences.Count);
        if (screen == ScreenSide.Left)
        {
            leftPromptsText.text = mistakeSentences[ranNum];
            gameTimeLeft -= mistakeTimeLoss;
        }
        else if (screen == ScreenSide.None)
        {
            promptsText.text = mistakeSentences[ranNum];
            gameTimeRight -= mistakeTimeLoss;
        }
        else if (screen == ScreenSide.Right)
        {
            rightPromptsText.text = mistakeSentences[ranNum];
            gameTimeRight -= mistakeTimeLoss;
        }
    }
    private void SetImageCompletedText()
    {
        imageCompletedText.text = $"Tasks Fulfilled: {completedImages} / {currentImageGoal}";
    }
    private void CreateObjective(ScreenSide screenSide)
    {
        if (completedImages < imageGoal1)
        {
            screenSide = ScreenSide.None;
        }
        RandomRequirementGenerator(screenSide);
        if (screenSide == ScreenSide.None)
            UpdateObjectiveText(promptsText);
        else if (screenSide == ScreenSide.Right)
            UpdateObjectiveText(rightPromptsText);
        else if (screenSide == ScreenSide.Left)
            UpdateObjectiveText(leftPromptsText);
    }
    private void ProgressToNextPhase()
    {
        stopTimer = true;
        stopTimerLeft = true;
        promptsText.text = "";
        SetActiveText(true, false);
        SetActiveSliders(false, false);
        if (completedImages >= endGameGoal)
        {
            EndScreen.SetActive(true);
            endGame = true;
        }
        else if (!finishMachineAndButtons.activeSelf)
        {
            StartCoroutine(LeftFinishMachineInitalise());
        }
        else if (!fireFloorObjects.activeSelf)
        {
            StartCoroutine(FireFloorObjects());
        }
    }
    //8
    private float waitTime = 8;
    private IEnumerator LeftFinishMachineInitalise()
    {
        promptsText.text = $"Creator: Well done! Looks like you fulfilled {completedImages} Users so far. The system efficiency can still be improved.";
        yield return new WaitForSeconds(waitTime);
        promptsText.text = $"Creator: I'm going to set the amount of users to 2. It will make it easier for the DPU to reach the {endGameGoal} quota!";
        yield return new WaitForSeconds(waitTime);
        promptsText.text = $"Creator: There will be some buttons that will send an error message to them and buy more time to buffer!";
        yield return new WaitForSeconds(waitTime);
        finishMachineAndButtons.SetActive(true);
        currentImageGoal = imageGoal2;
        InitaliseBothFinishLine();
    }
    private IEnumerator FireFloorObjects()
    {
        promptsText.text = $"Creator: Don't be alarm, but there has been a breach in our firewall. There is no estimated time of when it will be fixed so you will have to manage.";
        yield return new WaitForSeconds(waitTime);
        promptsText.text = $"Creator: The firewall in our system cannot detect properly detect threats and will slow down the system.";
        yield return new WaitForSeconds(waitTime);
        promptsText.text = $"Creator: On the brightside you completed {completedImages}! Well I'll leave the rest up to you.";
        yield return new WaitForSeconds(waitTime);
        promptsText.text = $"Creator has close this channel.";
        yield return new WaitForSeconds(3f);
        fireFloorObjects.SetActive(true);
        currentImageGoal = endGameGoal;
        InitaliseBothFinishLine();
    }
    private void InitaliseBothFinishLine()
    {
        pictureObjectiveLeft.Clear();
        pictureObjectiveRight.Clear();
        isTwoFinishLine = true;
        SetActiveText(false, true);
        SetActiveSliders(false, true);
        gameTimeLeft = maxTimeValue;
        gameTimeRight = maxTimeValue;
        CreateObjective(ScreenSide.Left);
        CreateObjective(ScreenSide.Right);
        stopTimer = false;
        stopTimerLeft = false;
    }
    public void RandomRequirementGenerator(ScreenSide side)
    {
        PictureShape shape = RandomShapePicker();
        if (side == ScreenSide.Right || side == ScreenSide.None)
            pictureObjectiveRight.Add(new PictureObjective(RandomColourPicker(), PictureShape.None));
        else
            pictureObjectiveLeft.Add(new PictureObjective(RandomColourPicker(), PictureShape.None)); 
        if (generatingTwoPictureObjective == false) { return; }
        if (!IsTwoObjective()) { return; }
        while (shape == PictureShape.None)
        {
            shape = RandomShapePicker();
        }
        if (side == ScreenSide.Right || side == ScreenSide.None)
            pictureObjectiveRight.Add(new PictureObjective(RandomColourPicker(), shape));
        else
            pictureObjectiveLeft.Add(new PictureObjective(RandomColourPicker(), shape));
    }
    private PictureColor RandomColourPicker()
    {
        return (PictureColor)Random.Range(0, System.Enum.GetValues(typeof(PictureColor)).Length);
    }
    private PictureShape RandomShapePicker()
    {
        return (PictureShape)Random.Range(0, System.Enum.GetValues(typeof(PictureShape)).Length);
    }
    private bool IsTwoObjective()
    {
        isTwoPictureObjective = Random.value <= .5 ? true : false;
        return isTwoPictureObjective;
    }
    private void SetActiveText(bool middle, bool leftRight)
    {
        promptsText.gameObject.SetActive(middle);
        leftPromptsText.gameObject.SetActive(leftRight);
        rightPromptsText.gameObject.SetActive(leftRight);
    }
    private void SetActiveSliders(bool middle, bool leftRight)
    {
        timerSlider.gameObject.SetActive(middle);
        timerSliderLeft.gameObject.SetActive(leftRight);
        timerSliderRight.gameObject.SetActive(leftRight);
    }
    private void GameOver()
    {
        endGame = true;
        GameOverCanvas.SetActive(true);
        StartCoroutine(DelayGameOverButtons());
    }
    private IEnumerator DelayGameOverButtons()
    {
        yield return new WaitForSeconds(2f);
        GameOverButtonGroup.SetActive(true);
        isGamePause = true;
    }
    float extraTime = 8f;
    public void AddTime(ScreenSide screen)
    {
        if (screen == ScreenSide.Right || screen == ScreenSide.None)
        {
            gameTimeRight += extraTime;
            if (gameTimeRight > maxTimeValue) { gameTimeRight = maxTimeValue; }
        }
        else
        {
            gameTimeLeft += extraTime;
            if (gameTimeLeft > maxTimeValue) { gameTimeLeft = maxTimeValue; }
        }
        StartCoroutine(ErrorMessage(screen));
    }
    private IEnumerator ErrorMessage(ScreenSide screenSide)
    {
        int ranNum = Random.Range(0, errorSentences.Count);
        if (screenSide == ScreenSide.Left)
        {
            leftPromptsText.text = errorSentences[ranNum];
        }
        else if (screenSide == ScreenSide.None)
        {
            promptsText.text = errorSentences[ranNum];
        }
        else if (screenSide == ScreenSide.Right)
        {
            rightPromptsText.text = errorSentences[ranNum];
        }
        yield return new WaitForSeconds(3f);
        if (screenSide == ScreenSide.None)
        {
            UpdateObjectiveText(promptsText);
        }
        else if (screenSide == ScreenSide.Left)
        {
            UpdateObjectiveText(leftPromptsText);
        }
        else if (screenSide == ScreenSide.Right)
        {
            UpdateObjectiveText(rightPromptsText);
        }
    }
    public void OnButtonTryAgain()
    {
        isGamePause = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnButtonMainMenu()
    {
        isGamePause = false;
        SceneManager.LoadScene(0);
    }
}

public class PictureObjective
{
    private PictureColor pictureColor;
    private PictureShape pictureShape;

    public PictureColor _PictureColour => pictureColor;
    public PictureShape _PictureShape => pictureShape;

    public PictureObjective(PictureColor color, PictureShape shape)
    {
        pictureColor = color;
        pictureShape = shape;
    }
}
