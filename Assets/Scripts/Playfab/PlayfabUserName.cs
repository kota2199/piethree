using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;


public class PlayfabUserName : MonoBehaviour
{
    // called before the first frame update
    public void UpdateUserName()
    {
        //���[�U�����w�肵�āAUpdateUserTitleDisplayNameRequest�̃C���X�^���X�𐶐�
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = PlayerPrefs.GetString("UserName")
        };

        //���[�U���̍X�V
        Debug.Log($"���[�U���̍X�V�J�n");
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUpdateUserNameSuccess, OnUpdateUserNameFailure);
    }

    //���[�U���̍X�V����
    private void OnUpdateUserNameSuccess(UpdateUserTitleDisplayNameResult result)
    {
        //result.DisplayName�ɍX�V������̃��[�U���������Ă�
        Debug.Log($"���[�U���̍X�V���������܂��� : {result.DisplayName}");
    }

    //���[�U���̍X�V���s
    private void OnUpdateUserNameFailure(PlayFabError error)
    {
        Debug.LogError($"���[�U���̍X�V�Ɏ��s���܂���\n{error.GenerateErrorReport()}");
    }
}
