using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using System.Threading.Tasks;
using UnityEngine;


public class PlayfabUserName : MonoBehaviour
{
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
