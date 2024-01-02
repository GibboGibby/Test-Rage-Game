using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private LeaderboardManager leaderboardManager;
    [SerializeField] private TMP_FontAsset fontAsset;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public LeaderboardManager GetLeaderboardManager()
    {
        return leaderboardManager;
    }

    public TMP_FontAsset GetAlienFontAsset()
    {
        return fontAsset;
    }
}
