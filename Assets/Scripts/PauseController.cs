using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool pauseOpen = false;
    [SerializeField] private GameObject pauseMenu;

    [Header("Menus")]
    [SerializeField] private GameObject pauseMainMenu;
    [SerializeField] private GameObject pauseSettings;
    [SerializeField] private GameObject pauseLeaderboardMenu;
    [SerializeField] private GameObject pauseLeaderboardPrefab;

    [SerializeField] private UILeaderboardCreator leaderboardCreator;
    [SerializeField] private LeaderboardManager leaderboardManager;

    [SerializeField] private TMP_InputField inputText;
    [SerializeField] private TextMeshProUGUI badText;
    [SerializeField] private TextMeshProUGUI goodText;

    [SerializeField] private PlayerController pc;

    private IEnumerator goodTextCoroutine;
    private IEnumerator badTextCoroutine;

    enum PausePage
    {
        MAINPAUSE,
        SETTINGS,
        LEADERBOARD,
    };
    void Start()
    {
        HideAll();
        CheckPause();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Tihs is being called");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseOpen)
                pauseOpen = false;
            else 
                pauseOpen = true;
            CheckPause();
        }
    }

    void CheckPause()
    {
        if (pauseOpen)
        {
            SetPausePage(PausePage.MAINPAUSE);
            pauseMenu.SetActive(true);
            pc.enabled = false;
            ResumeGame();
        }
        else { 
            SetPausePage(PausePage.MAINPAUSE);
            pauseMenu.SetActive(false);
            pc.enabled = true;
            ResumeGame();
        }
    }


    public void ContinueGame()
    {
        pauseOpen = false;
        CheckPause();
    }

    void SetPausePage(PausePage pausePage)
    {
        switch(pausePage)
        {
            case PausePage.MAINPAUSE:
                HideAll();
                pauseMainMenu.SetActive(true);
                break;
            case PausePage.SETTINGS:
                HideAll();
                pauseSettings.SetActive(true);
                break;
            case PausePage.LEADERBOARD:
                HideAll();
                pauseLeaderboardMenu.SetActive(true);
                pauseLeaderboardPrefab.SetActive(true);
                break;
        }
    }


    void HideAll()
    {
        pauseMainMenu.SetActive(false);
        pauseLeaderboardMenu.SetActive(false);
        pauseLeaderboardPrefab.SetActive(false);
        pauseSettings.SetActive(false);
    }


    public void SwitchToMainPause()
    {
        SetPausePage(PausePage.MAINPAUSE);
    }

    public void SwitchToSettings()
    {
        SetPausePage(PausePage.SETTINGS);
        inputText.text = PlayerPrefs.GetString("Username");
    }
    
    public void SwitchToLeaderboard()
    {
        SetPausePage(PausePage.LEADERBOARD);
        leaderboardManager.UpdateLeaderboardItems(leaderboardCreator);
    }

    public void ResumeGame()
    {
        Time.timeScale = pauseOpen ? 0 : 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CheckName()
    {
        if (!CheckNameIsValid(inputText.text)) return;

        GameManager.Instance.GetLeaderboardManager().ChangeName(inputText.text, this);

    }

    public void NameChangedSuccessfully()
    {
        StopAllCoroutines();
        badText.gameObject.SetActive(false);
        goodText.gameObject.SetActive(true);
        StartCoroutine(HideTextAfterTime(2.0f, goodText));
    }

    public void NameChangeFailed()
    {
        PrintInvalidName("Username is taken or another error has occured");
    }

    private IEnumerator HideTextAfterTime(float time, TextMeshProUGUI text)
    {
        yield return new WaitForSeconds(time);
        text.gameObject.SetActive(false);

    }

    private bool CheckNameIsValid(string name)
    {
        if (name.Length > 10)
            return PrintInvalidName("Name must have less than 10 characters");
        if (name.Length <= 3)
            return PrintInvalidName("Name must have more than 3 characters");
        if (name.Contains(" "))
            return PrintInvalidName("Name cannot have spaces");
        return true;
    }

    private bool PrintInvalidName(string error)
    {
        // Change text thing
        StopAllCoroutines();
        badText.text = error;
        goodText.gameObject.SetActive(false);
        badText.gameObject.SetActive(true);
        StartCoroutine(HideTextAfterTime(2.0f, badText));
        return false;
    }
}
