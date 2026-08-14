using TMPro;
using Unity.Netcode;
using Unity.Services.Multiplayer;
using UnityEngine;

public class ConnectManager : MonoBehaviour
{
    public static ConnectManager Instance;
    [SerializeField] private TMP_Text codeNumber;

    void Awake()
    {
        Instance = this;
    }

    public void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if (GameStateManager.Instance.gameState.Value == GameStateManager.GameState.Ready)
        {
            response.Approved = true;
            response.CreatePlayerObject = true;
        }
        else
        {
            response.Approved = false;
            response.Reason = "게임이 진행중이라 접속불가";
        }
    }
    private void OnClientDisconnect(ulong clientId)
    {
        Debug.Log(NetworkManager.Singleton.DisconnectReason);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        NetworkManager.Singleton.StartClient();
    }

    public async void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;

        SessionOptions options = new SessionOptions
        {
            MaxPlayers = 8
        }.WithRelayNetwork();

        var session = await MultiplayerService.Instance.CreateSessionAsync(options);
        codeNumber.text = session.Code;
        Debug.Log("방 코드 : " + session.Code);
    }
}
