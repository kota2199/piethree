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
    //�X�R�A
    //=================================================================================

    /// <summary>
    /// �X�R�A(���v���)���X�V����
    /// </summary>
    public void UpdatePlayerStatistics(int score)
    {
        //UpdatePlayerStatisticsRequest�̃C���X�^���X�𐶐�
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>{
        new StatisticUpdate{
          StatisticName = leaderboardName,   //�����L���O��(���v���)
          Value = score, //�X�R�A(int)
        }
      }
        };

        //���[�U���̍X�V
        Debug.Log($"�X�R�A(���v���)�̍X�V�J�n");
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnUpdatePlayerStatisticsSuccess, OnUpdatePlayerStatisticsFailure);
    }

    //�X�R�A(���v���)�̍X�V����
    private void OnUpdatePlayerStatisticsSuccess(UpdatePlayerStatisticsResult result)
    {
        Debug.Log($"�X�R�A(���v���)�̍X�V���������܂���");
        GetLeaderboard();
    }

    //�X�R�A(���v���)�̍X�V���s
    private void OnUpdatePlayerStatisticsFailure(PlayFabError error)
    {
        Debug.LogError($"�X�R�A(���v���)�X�V�Ɏ��s���܂���\n{error.GenerateErrorReport()}");
    }


    //=================================================================================
    //�����L���O�擾
    //=================================================================================

    /// <summary>
    /// �����L���O(���[�_�[�{�[�h)���擾
    /// </summary>
    public void GetLeaderboard()
    {
        //GetLeaderboardRequest�̃C���X�^���X�𐶐�
        var request = new GetLeaderboardRequest
        {
            StatisticName = leaderboardName, //�����L���O��(���v���)
            StartPosition = 0,                 //���ʈȍ~�̃����L���O���擾���邩
            MaxResultsCount = 3                  //�����L���O�f�[�^�������擾���邩(�ő�100)
        };

        //�����L���O(���[�_�[�{�[�h)���擾
        Debug.Log($"�����L���O(���[�_�[�{�[�h)�̎擾�J�n");
        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnGetLeaderboardFailure);
    }

    //�����L���O(���[�_�[�{�[�h)�̎擾����
    private void OnGetLeaderboardSuccess(GetLeaderboardResult result)
    {
        Debug.Log($"�����L���O(���[�_�[�{�[�h)�̎擾�ɐ������܂���");

        //result.Leaderboard�Ɋe���ʂ̏��(PlayerLeaderboardEntry)�������Ă���
        _rankingText.text = "";
        foreach (var entry in result.Leaderboard)
        {
            _rankingText.text += $"\n�@{entry.Position + 1} ��,�@{entry.DisplayName}, {entry.StatValue} ��";
        }

        Invoke("GetLeaderboard", 5f);
    }

    //�����L���O(���[�_�[�{�[�h)�̎擾���s
    private void OnGetLeaderboardFailure(PlayFabError error)
    {
        Debug.LogError($"�����L���O(���[�_�[�{�[�h)�̎擾�Ɏ��s���܂���\n{error.GenerateErrorReport()}");
    }

    public void GetLeaderboardAroundPlayer()
    {
        //GetLeaderboardAroundPlayerRequest�̃C���X�^���X�𐶐�
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = leaderboardName, //�����L���O��(���v���)
            MaxResultsCount = 3                  //�������܂ߑO�㉽���擾���邩
        };

        //�����̏��ʎ��ӂ̃����L���O(���[�_�[�{�[�h)���擾
        Debug.Log($"�����̏��ʎ��ӂ̃����L���O(���[�_�[�{�[�h)�̎擾�J�n");
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnGetLeaderboardAroundPlayerSuccess, OnGetLeaderboardAroundPlayerFailure);
    }

    //�����̏��ʎ��ӂ̃����L���O(���[�_�[�{�[�h)�̎擾����
    private void OnGetLeaderboardAroundPlayerSuccess(GetLeaderboardAroundPlayerResult result)
    {
        Debug.Log($"�����̏��ʎ��ӂ̃����L���O(���[�_�[�{�[�h)�̎擾�ɐ������܂���");

        //result.Leaderboard�Ɋe���ʂ̏��(PlayerLeaderboardEntry)�������Ă���
        _rankingText.text = "";
        foreach (var entry in result.Leaderboard)
        {
            _rankingText.text += $"\n���� : {entry.Position}, �X�R�A : {entry.StatValue} �� , ���O : {entry.DisplayName}";
        }
    }

    //�����̏��ʎ��ӂ̃����L���O(���[�_�[�{�[�h)�̎擾���s
    private void OnGetLeaderboardAroundPlayerFailure(PlayFabError error)
    {
        Debug.LogError($"�����̏��ʎ��ӂ̃����L���O(���[�_�[�{�[�h)�̎擾�Ɏ��s���܂���\n{error.GenerateErrorReport()}");
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
