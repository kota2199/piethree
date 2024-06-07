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
            Debug.LogError("ユーザ名が空です。正しいユーザ名を設定してください。");
            return false;
        }

        //ユーザ名を指定して、UpdateUserTitleDisplayNameRequestのインスタンスを生成
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name
        };

        //ユーザ名の更新
        Debug.Log("ユーザ名の更新開始");

        var taskCompletionSource = new TaskCompletionSource<bool>();

        PlayFabClientAPI.UpdateUserTitleDisplayName(request,
            result => {
                Debug.Log($"ユーザ名の更新が成功しました : {result.DisplayName}");
                taskCompletionSource.SetResult(true);
            },
            error => {
                Debug.LogError($"ユーザ名の更新に失敗しました\n{error.GenerateErrorReport()}");
                taskCompletionSource.SetResult(false);
            });

        return await taskCompletionSource.Task;
    }
}
