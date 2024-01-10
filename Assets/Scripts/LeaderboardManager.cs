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
            TryToSetStartingName();
            
        }
    }

    private void TryToSetStartingName()
    {
        string player = "Player" + Random.Range(1000, 99999999).ToString();
        LeaderboardCreator.UploadNewEntry(publicKey, player, 0, "", ((msg) =>
        {
            PlayerPrefs.SetString(userNameKey, player);
        }), (msg) =>
        {
            TryToSetStartingName();
        });
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

    public void ChangeName(string name, PauseController pc)
    {
        StartCoroutine(ChangeNameCoroutine(name, pc));
        //if (PlayerPrefs.GetString(userNameKey) == name) return true;
    }

    private IEnumerator ChangeNameCoroutine(string name, PauseController pc)
    {
        int code = 0;
        LeaderboardCreator.UpdateEntryUsername(publicKey, name, (msg) =>
        {
            code = 1;
        }, (msg) =>
        {
            code = -1;
        });

        yield return new WaitWhile(() => code == 0);

        if (code == 1) 
        {
            PlayerPrefs.SetString(userNameKey, name);
            pc.NameChangedSuccessfully();
        }
        else
        {
            pc.NameChangeFailed();
        }

    }


    //int height = 0;
    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.B))
        {
            AddNewEntry(PlayerPrefs.GetString(userNameKey), height, 0);
            height++;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            ChangeName("Jame");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LeaderboardCreator.DeleteEntry(publicKey);
        }
        */
    }

    public void DeleteAndUpdateLeaderboard(string username, int height, string time)
    {
        //StartCoroutine(DeleteAndReAddCoroutine(username, height, time));
        LeaderboardCreator.DeleteEntry(publicKey, ((msg) =>
        {
            LeaderboardCreator.UploadNewEntry(publicKey, username, height, time);
        }));
    }

    private IEnumerator DeleteAndReAddCoroutine(string username, int height, string time)
    {
        LeaderboardCreator.UploadNewEntry(publicKey, username, height, time);
        yield return new WaitForSeconds(1.0f);
        LeaderboardCreator.DeleteEntry(publicKey);

        yield return new WaitForSeconds(1.0f);
        LeaderboardCreator.UploadNewEntry(publicKey, username, height, time);
    }

    public string GetUsername()
    {
        return PlayerPrefs.GetString(userNameKey);
    }
}
