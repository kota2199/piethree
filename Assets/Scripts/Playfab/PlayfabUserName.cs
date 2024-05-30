using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class PlayfabUserName : MonoBehaviour
{
    //public void UpdateUserName(string name)
    //{
    //    //���[�U�����w�肵�āAUpdateUserTitleDisplayNameRequest�̃C���X�^���X�𐶐�
    //    var request = new UpdateUserTitleDisplayNameRequest
    //    {
    //        DisplayName = name
    //    };
    //    //���[�U���̍X�V
    //    Debug.Log($"���[�U���̍X�V�J�n");
    //    PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUpdateUserNameSuccess, OnUpdateUserNameFailure);
    //}
    ////���[�U���̍X�V����
    //private void OnUpdateUserNameSuccess(UpdateUserTitleDisplayNameResult result)
    //{
    //    //result.DisplayName�ɍX�V������̃��[�U���������Ă�
    //    Debug.Log($"���[�U���̍X�V���������܂��� : {result.DisplayName}");
    //}

    ////���[�U���̍X�V���s
    //private void OnUpdateUserNameFailure(PlayFabError error)
    //{
    //    Debug.LogError($"���[�U���̍X�V�Ɏ��s���܂���\n{error.GenerateErrorReport()}");
    //}

    public async Task<bool> UpdateUserNameAsync(string name)
    {
        string userName = PlayerPrefs.GetString("UserName");

        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("���[�U������ł��B���������[�U����ݒ肵�Ă��������B");
            return false;
        }

        //���[�U�����w�肵�āAUpdateUserTitleDisplayNameRequest�̃C���X�^���X�𐶐�
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name
        };

        //���[�U���̍X�V
        Debug.Log("���[�U���̍X�V�J�n");

        var taskCompletionSource = new TaskCompletionSource<bool>();

        PlayFabClientAPI.UpdateUserTitleDisplayName(request,
            result => {
                Debug.Log($"���[�U���̍X�V���������܂��� : {result.DisplayName}");
                taskCompletionSource.SetResult(true);
            },
            error => {
                Debug.LogError($"���[�U���̍X�V�Ɏ��s���܂���\n{error.GenerateErrorReport()}");
                taskCompletionSource.SetResult(false);
            });

        return await taskCompletionSource.Task;
    }
}
