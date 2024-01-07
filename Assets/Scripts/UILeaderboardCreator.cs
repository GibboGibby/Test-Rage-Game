using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILeaderboardCreator : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject[] LeaderboardItems;

    [Header("Bad")]
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private GameObject firstEntry;
    [SerializeField] private float topPadding;
    [SerializeField] private float leftPadding;
    // Start is called before the first frame update
    void Start()
    {
        ResetLeaderboard();
    }

    // Update is called once per frame
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            UpdateLeaderboard();
        }
        
    }

    public void AddItem(int boardNum, string name, int height, string time)
    {
        GameObject entry = Instantiate(entryPrefab, content.transform);
        //entry.transform.parent = content.transform;
        entry.GetComponent<RectTransform>().localPosition = firstEntry.GetComponent<RectTransform>().localPosition;
        entry.GetComponent<RectTransform>().anchoredPosition = firstEntry.GetComponent<RectTransform>().anchoredPosition;
        entry.GetComponent<RectTransform>().localPosition = new Vector3(entry.GetComponent<RectTransform>().localPosition.x + leftPadding, entry.GetComponent<RectTransform>().localPosition.y + -boardNum * topPadding, 0);
        //entry.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
        //entry.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = height.ToString();
        //entry.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = time;
    }

    public void ResetLeaderboard()
    {
        for (int i = 0; i < LeaderboardItems.Length; i++)
        {
            LeaderboardItems[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            LeaderboardItems[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
            LeaderboardItems[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    public int GetSize()
    {
        return LeaderboardItems.Length;
    }

    public void SetLeaderboardElement(int pos, string name, int height, string time)
    {
        LeaderboardItems[pos].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = name;
        LeaderboardItems[pos].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = height.ToString();
        LeaderboardItems[pos].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = time;
    }

    public void UpdateLeaderboard()
    {
        GameManager.Instance.GetLeaderboardManager().UpdateLeaderboardItems(this);
    }

}
