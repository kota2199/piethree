using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PlayFab�̃��O�C���������s���N���X
/// </summary>
public class PlayFabLogin : MonoBehaviour
{

    //�A�J�E���g���쐬���邩
    private bool _shouldCreateAccount;

    //���O�C�����Ɏg��ID
    private string _customID;
    //=================================================================================
    //���O�C������
    //=================================================================================

    PlayFabLogin instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Start()
    {
        Login();
    }

    //���O�C�����s
    private void Login()
    {
        _customID = LoadCustomID();
        var request = new LoginWithCustomIDRequest { CustomId = _customID, CreateAccount = _shouldCreateAccount };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    //���O�C������
    private void OnLoginSuccess(LoginResult result)
    {
        //�A�J�E���g���쐬���悤�Ƃ����̂ɁAID�����Ɏg���Ă��āA�o���Ȃ������ꍇ
        if (_shouldCreateAccount && !result.NewlyCreated)
        {
            Debug.LogWarning($"CustomId : {_customID} �͊��Ɏg���Ă��܂��B");
            Login();//���O�C�����Ȃ���
            return;
        }

        //�A�J�E���g�쐬����ID��ۑ�
        if (result.NewlyCreated)
        {
            SaveCustomID();
        }
        Debug.Log($"PlayFab�̃��O�C���ɐ���\nPlayFabId : {result.PlayFabId}, CustomId : {_customID}\n�A�J�E���g���쐬������ : {result.NewlyCreated}");



        //GetComponent<PlayfabUserName>().UpdateUserName();
    }

    //���O�C�����s
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError($"PlayFab�̃��O�C���Ɏ��s\n{error.GenerateErrorReport()}");
    }

    //=================================================================================
    //�J�X�^��ID�̎擾
    //=================================================================================

    //ID��ۑ����鎞��KEY
    private static readonly string CUSTOM_ID_SAVE_KEY = "CUSTOM_ID_SAVE_KEY";

    //ID���擾
    private string LoadCustomID()
    {
        //ID���擾
        string id = PlayerPrefs.GetString(CUSTOM_ID_SAVE_KEY);

        //�ۑ�����Ă��Ȃ���ΐV�K����
        _shouldCreateAccount = string.IsNullOrEmpty(id);
        return _shouldCreateAccount ? GenerateCustomID() : id;
    }

    //ID�̕ۑ�
    private void SaveCustomID()
    {
        PlayerPrefs.SetString(CUSTOM_ID_SAVE_KEY, _customID);
    }

    //=================================================================================
    //�J�X�^��ID�̐���
    //=================================================================================

    //ID�Ɏg�p���镶��
    private static readonly string ID_CHARACTERS = "0123456789abcdefghijklmnopqrstuvwxyz";

    //ID�𐶐�����
    private string GenerateCustomID()
    {
        int idLength = 32;//ID�̒���
        StringBuilder stringBuilder = new StringBuilder(idLength);
        var random = new System.Random();

        //�����_����ID�𐶐�
        for (int i = 0; i < idLength; i++)
        {
            stringBuilder.Append(ID_CHARACTERS[random.Next(ID_CHARACTERS.Length)]);
        }

        return stringBuilder.ToString();
    }

}