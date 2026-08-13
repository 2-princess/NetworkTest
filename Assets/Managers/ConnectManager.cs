using Unity.Netcode;
using UnityEngine;

public class ConnectManager : MonoBehaviour
{
    public static ConnectManager Instance;

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
    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        NetworkManager.Singleton.StartHost();
    }
}
