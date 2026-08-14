using System;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField joinCodeInput;

    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();

            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            Debug.Log("로그인 성공 : " + AuthenticationService.Instance.PlayerId);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
    public async void JoinSession()
    {
        string joinCode = joinCodeInput.text;
        await MultiplayerService.Instance.JoinSessionByCodeAsync(joinCode);

        Debug.Log("세션 참가 성공");
    }
}
