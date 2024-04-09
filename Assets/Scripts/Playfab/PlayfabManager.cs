using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;

public class PlayfabManager : MonoBehaviour
{
    const string STATISTICS_NAME = "HighScore�QValentine";

    void Start()
    {
        PlayFabClientAPI.LoginWithCustomID(
            new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true },
            result => Debug.Log("���O�C�������I"),
            error => Debug.Log("���O�C�����s"));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SubmitScore(400);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RequestLeaderBoard();
        }
    }

    void SubmitScore(int playerScore)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(
            new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>()
                {
                    new StatisticUpdate
                    {
                        StatisticName = STATISTICS_NAME,
                        Value = playerScore
                    }
                }
            },
            result =>
            {
                Debug.Log("�X�R�A���M");
            },
            error =>
            {
                Debug.Log(error.GenerateErrorReport());
            }
            );
    }


    void RequestLeaderBoard()
    {
        PlayFabClientAPI.GetLeaderboard(
            new GetLeaderboardRequest
            {
                StatisticName = STATISTICS_NAME,
                StartPosition = 0,
                MaxResultsCount = 10
            },
            result =>
            {
                result.Leaderboard.ForEach(
                    x => Debug.Log(string.Format("{0}��:{1} �X�R�A{2}", x.Position + 1, x.DisplayName, x.StatValue))
                    );
            },
            error =>
            {
                Debug.Log(error.GenerateErrorReport());
            }
            );
    }
}
