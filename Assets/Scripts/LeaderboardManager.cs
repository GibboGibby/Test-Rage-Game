using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dan.Main;

public class LeaderboardManager : MonoBehaviour
{
    private string userNameKey = "Username";

    private string publicKey = "73a86103165174701f5470c18f8d14d4f9e4aa1887fbfe12b39c1cd9992ab283";
    private string secretKey = "7ac321de3aa7a00142eb9c6bf5cca075a1d8c3ed68796271a6a5ef9e7cc012a5042382bf3e4ab09dba40da3350fe739c75d6b9a50cbe5dba7faec78559ac671d83780452f4cf4e21202dd9096885b59b3a4c806cdab5fe828c47b2cd3d9a0ad741b92d0b914692b517ac9730604fcc0907f4e981acd8aca3304468852a14b2ca";
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(userNameKey))
        {
            Debug.Log("Has key");
        }
        else
        {
            string player = string.Empty;
            player = "Player" + Random.Range(1000, 10000000).ToString();
            PlayerPrefs.SetString(userNameKey, player);
        }
    }

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicKey, ((msg) =>
        {
            Debug.Log(msg);
        }));
    }

    public void UpdateLeaderboardItems(UILeaderboardCreator uilc)
    {
        LeaderboardCreator.GetLeaderboard(publicKey, ((msg) =>
        {
            int loopLength = (msg.Length < uilc.GetSize()) ? msg.Length : uilc.GetSize();
            for (int i = 0; i < loopLength; i++)
            {
                uilc.SetLeaderboardElement(i, msg[i].Username, msg[i].Score, msg[i].Extra);
            }
        }));
    }

    public void AddNewEntry(string username, int height, int time)
    {
        LeaderboardCreator.UploadNewEntry(publicKey, username, height, time.ToString(), ((msg) =>
        {
            GetLeaderboard();
        }));
    }

    public void ChangeName(string name)
    {
        //string oldName = PlayerPrefs.GetString(userNameKey);
        PlayerPrefs.SetString(userNameKey, name);
        LeaderboardCreator.UpdateEntryUsername(publicKey, name);
    }

    int height = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            AddNewEntry(PlayerPrefs.GetString(userNameKey), height, 0);
            height++;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            ChangeName("Jame");
        }
    }
}
