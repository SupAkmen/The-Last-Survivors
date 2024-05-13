using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp
    }


    public GameState currentState;
    public GameState previousState;

    [Header("Damage Text Setting")]
    public Canvas damageTextCanvas; // dung de hien thi text
    public float textFontSize = 30;
    public TMP_FontAsset textFont; // su dung de gan phong chu neu muon
    public Camera referenceCamera; // chuyen toa do the gioi ve toa do man hinh

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;

    [Header("Coin")]
    public TextMeshProUGUI currentMatchCoin;
    public int currentCoin = 0;
    public int totalCoin;
    private const string totalCoinKey = "TotalCoin";
    private const string currentMatchCoinKey = "CurrentMatchCoin";

    [Header("Results Screen Display")]
    public Image chosenCharacterImage;
    public TextMeshProUGUI chosenCharacterName;
    public TextMeshProUGUI levelReachedDisplay;
    public TextMeshProUGUI timeSurvivedDisplay;
    public List<Image> chosenWeaponUI = new List<Image>(6);
    public List<Image> chosenPassiveItemUI = new List<Image>(6);

    [Header("StopWatch")]
    public float timeLimit; // thoi gian toi da
    float stopwatchTime; // thoi gian game chay
    public TextMeshProUGUI stopwatchDisplay;


    public bool isGameOver = false;
    public bool choosingUpgrade;
    // Reference to the player's gameobject
    public GameObject playerObject;
    AudioManager audioManager;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DisableScreen();
    }
    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void Update()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                UpdateStopWatch();
                UpdateCurrentMatchCoin();
                break;
            case GameState.Paused:
                CheckForPauseAndResume();
                break;
            case GameState.GameOver:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    DisplayResults();
                    UpdateTotalCoin();
                   
                }
                break;
            case GameState.LevelUp:
                if (!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0f;
                    levelUpScreen.SetActive(true);
                }
                break;
            default:
                Debug.LogWarning("State doen't exist");
                break;
        }
    }

    IEnumerator GenerateFloatingTextCoroutine(string text,Transform target, float duration = 1f, float speed = 50f)
    {
    // starting generate the floating text
    GameObject textObj = new GameObject("Damage Floating Text");
    RectTransform rect = textObj.AddComponent<RectTransform>();
    TextMeshProUGUI tmPro = textObj.AddComponent<TextMeshProUGUI>();
    tmPro.text = text;
    tmPro.horizontalAlignment = HorizontalAlignmentOptions.Center;
    tmPro.verticalAlignment = VerticalAlignmentOptions.Middle;
    tmPro.fontSize = textFontSize;
    if(textFont)
    {
        tmPro.font = textFont;
    }
    rect.position = referenceCamera.WorldToScreenPoint(target.position);

    // make sure thi is destroyed after the duration finishes
    Destroy(textObj,duration);

    // Parent the generated text object to the canvas
    textObj.transform.SetParent(instance.damageTextCanvas.transform);
    textObj.transform.SetSiblingIndex(0);

    // Pan the text upwards and fade it way over time
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0;
        float yOffset = 0;
        Vector3 lastKnowPosition = target.position;
        while (t < duration)
        {
            if (!rect) break;
            // Fade the text to the right alpha value
            tmPro.color = new Color(tmPro.color.r, tmPro.color.g, tmPro.color.b, (1 - t) / duration);

            // Update the enymy position if it is still around.
            if(target) lastKnowPosition = target.position;
            // Pan the text upwards
            yOffset += speed * Time.deltaTime;
            rect.position = referenceCamera.WorldToScreenPoint(lastKnowPosition + new Vector3(0, yOffset, 0));

            // wait for a frame and update the time
            yield return w;
            t += Time.deltaTime;
        }
    }

    public static void GenerateFloatingText(string text,Transform target,float duration = 1f, float speed = 1f)
    {
        // neu canvas ko dc set thi bo qua ham
        if (!instance.damageTextCanvas)
            return;
        // find a revelant camera that we can use to convert the world position to a screen position
        if(!instance.referenceCamera)
        {
            instance.referenceCamera = Camera.main;
        }
        instance.StartCoroutine(instance.GenerateFloatingTextCoroutine(text,target, duration, speed));
    }


    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
           // Debug.Log("Game is paused");
        }

    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1;
            pauseScreen.SetActive(false);

        }
    }

    void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void DisableScreen()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        timeSurvivedDisplay.text = stopwatchDisplay.text;
        ChangeState(GameState.GameOver);
        audioManager.PlaySFX(audioManager.endGame);
    }

    void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }

    public void AssignChosenCharacter(CharacterData chosenCharacterData)
    {
        chosenCharacterImage.sprite = chosenCharacterData.Icon;
        chosenCharacterName.text = chosenCharacterData.Name;
    }
    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }

    public void AssignChosenWeaponAndPassiveItemUI(List<Image> chosenWeaponData, List<Image> chosenPassiveItemData)
    {
        if (chosenWeaponData.Count != chosenWeaponUI.Count || chosenPassiveItemData.Count != chosenWeaponUI.Count)
        {
            Debug.Log("Chosen weapon and passive item data lists have different lenghths");
            return;
        }

        for (int i = 0; i < chosenWeaponUI.Count; i++)
        {
            if (chosenWeaponData[i].sprite)
            {
                chosenWeaponUI[i].enabled = true;
                chosenWeaponUI[i].sprite = chosenWeaponData[i].sprite;
            }
            else
            {
                chosenWeaponUI[i].enabled = false;
            }
        }

        for (int i = 0; i < chosenPassiveItemUI.Count; i++)
        {
            if (chosenPassiveItemData[i].sprite)
            {
                chosenPassiveItemUI[i].enabled = true;
                chosenPassiveItemUI[i].sprite = chosenPassiveItemData[i].sprite;
            }
            else
            {
                chosenPassiveItemUI[i].enabled = false;
            }
        }
    }

    void UpdateStopWatch()
    {
        stopwatchTime += Time.deltaTime;
        UpdateStopWatchDisplay();

        if (stopwatchTime >= timeLimit)
        {
            playerObject.SendMessage("Kill");
        }
    }

    void UpdateStopWatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);

        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
        audioManager.PlaySFX(audioManager.levelUp);
    }
    public void EndLevelUp()
    {
        choosingUpgrade = false;
        Time.timeScale = 1f;
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }


    public void AddCoinToCurrentMatch(int amount)
    {
        currentCoin += amount;
    }

    public void UpdateCurrentMatchCoin()
    {
        currentMatchCoin.text = currentCoin.ToString();
    }

    public void UpdateTotalCoin()
    {
        // Lấy giá trị tổng coin từ PlayerPrefs
        totalCoin = PlayerPrefs.GetInt(totalCoinKey, 0);

        // Cộng giá trị currentCoin vào tổng coin
        totalCoin += currentCoin;

        // Lưu giá trị tổng coin vào PlayerPrefs
        PlayerPrefs.SetInt(totalCoinKey, totalCoin);
        PlayerPrefs.SetInt(currentMatchCoinKey, 0);
        PlayerPrefs.Save();
    }
}
