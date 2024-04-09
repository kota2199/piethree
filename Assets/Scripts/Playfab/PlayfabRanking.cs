using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class PlayfabRanking : MonoBehaviour
{
    [SerializeField]
    private Text _rankingText = default;

    public enum GameMode
    {
        Haloween, Xmas, Valentine
    }

    public GameMode gameMode;

    private string leaderboardName;

    private void Start()
    {
        ChangeLeaderBoard();
    }

    //=================================================================================
    //スコア
    //=================================================================================

    /// <summary>
    /// スコア(統計情報)を更新する
    /// </summary>
    public void UpdatePlayerStatistics(int score)
    {
        //UpdatePlayerStatisticsRequestのインスタンスを生成
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>{
        new StatisticUpdate{
          StatisticName = leaderboardName,   //ランキング名(統計情報名)
          Value = score, //スコア(int)
        }
      }
        };

        //ユーザ名の更新
        Debug.Log($"スコア(統計情報)の更新開始");
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnUpdatePlayerStatisticsSuccess, OnUpdatePlayerStatisticsFailure);
    }

    //スコア(統計情報)の更新成功
    private void OnUpdatePlayerStatisticsSuccess(UpdatePlayerStatisticsResult result)
    {
        Debug.Log($"スコア(統計情報)の更新が成功しました");
        GetLeaderboard();
    }

    //スコア(統計情報)の更新失敗
    private void OnUpdatePlayerStatisticsFailure(PlayFabError error)
    {
        Debug.LogError($"スコア(統計情報)更新に失敗しました\n{error.GenerateErrorReport()}");
    }


    //=================================================================================
    //ランキング取得
    //=================================================================================

    /// <summary>
    /// ランキング(リーダーボード)を取得
    /// </summary>
    public void GetLeaderboard()
    {
        //GetLeaderboardRequestのインスタンスを生成
        var request = new GetLeaderboardRequest
        {
            StatisticName = leaderboardName, //ランキング名(統計情報名)
            StartPosition = 0,                 //何位以降のランキングを取得するか
            MaxResultsCount = 3                  //ランキングデータを何件取得するか(最大100)
        };

        //ランキング(リーダーボード)を取得
        Debug.Log($"ランキング(リーダーボード)の取得開始");
        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnGetLeaderboardFailure);
    }

    //ランキング(リーダーボード)の取得成功
    private void OnGetLeaderboardSuccess(GetLeaderboardResult result)
    {
        Debug.Log($"ランキング(リーダーボード)の取得に成功しました");

        //result.Leaderboardに各順位の情報(PlayerLeaderboardEntry)が入っている
        _rankingText.text = "";
        foreach (var entry in result.Leaderboard)
        {
            _rankingText.text += $"\n　{entry.Position + 1} 位,　{entry.DisplayName}, {entry.StatValue} 個";
        }

        Invoke("GetLeaderboard", 5f);
    }

    //ランキング(リーダーボード)の取得失敗
    private void OnGetLeaderboardFailure(PlayFabError error)
    {
        Debug.LogError($"ランキング(リーダーボード)の取得に失敗しました\n{error.GenerateErrorReport()}");
    }

    public void GetLeaderboardAroundPlayer()
    {
        //GetLeaderboardAroundPlayerRequestのインスタンスを生成
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = leaderboardName, //ランキング名(統計情報名)
            MaxResultsCount = 3                  //自分を含め前後何件取得するか
        };

        //自分の順位周辺のランキング(リーダーボード)を取得
        Debug.Log($"自分の順位周辺のランキング(リーダーボード)の取得開始");
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnGetLeaderboardAroundPlayerSuccess, OnGetLeaderboardAroundPlayerFailure);
    }

    //自分の順位周辺のランキング(リーダーボード)の取得成功
    private void OnGetLeaderboardAroundPlayerSuccess(GetLeaderboardAroundPlayerResult result)
    {
        Debug.Log($"自分の順位周辺のランキング(リーダーボード)の取得に成功しました");

        //result.Leaderboardに各順位の情報(PlayerLeaderboardEntry)が入っている
        _rankingText.text = "";
        foreach (var entry in result.Leaderboard)
        {
            _rankingText.text += $"\n順位 : {entry.Position}, スコア : {entry.StatValue} 個 , 名前 : {entry.DisplayName}";
        }
    }

    //自分の順位周辺のランキング(リーダーボード)の取得失敗
    private void OnGetLeaderboardAroundPlayerFailure(PlayFabError error)
    {
        Debug.LogError($"自分の順位周辺のランキング(リーダーボード)の取得に失敗しました\n{error.GenerateErrorReport()}");
    }

    void ChangeLeaderBoard()
    {
        switch (gameMode)
        {
            case GameMode.Haloween:
                leaderboardName = "HighScore_Haloween";
            break;

            case GameMode.Xmas:
                leaderboardName = "HighScore_Xmas";
                break;

            case GameMode.Valentine:
                leaderboardName = "HighScore_Valentine";
                break;
        }
    }
}
