using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;



public class PlayFabManager : MonoBehaviour
{
    public Player player;
    public TMP_InputField nomeRanking;
    public TMP_Text textRanking;
    // Start is called before the first frame update
    void Start()
    {
        Login();
    }
    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request,OnSuccess,OnError);
    }
    void OnSuccess(LoginResult result)
    {
        Debug.Log("Conectou");
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("Não Conectou");
        Debug.Log(error.GenerateErrorReport());
    }

 

    public void SubmitName()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nomeRanking.text
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUpdateName, OnError);
    }
    void OnUpdateName(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Duccess");
    }
    public void SendLeaderboard()
    {
        SubmitName();
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Pontuação",
                    Value = player.coletados
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
        GetLeaderBoard();
    }

 

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Success");
    }

 

    public void GetLeaderBoard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "Pontuação",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }
    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        string ranking = "";
        foreach (var item in result.Leaderboard)
        {
            ranking += item.Position + " " + item.DisplayName + " " + item.StatValue + "\n";
        }
        textRanking.text = ranking;
    }
}