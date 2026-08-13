using TMPro;
using Unity.Netcode;
using UnityEngine;


public class GameStateManager : NetworkBehaviour
{
    public static GameStateManager Instance;
    public NetworkVariable<int> round = new NetworkVariable<int>(1);
    public NetworkList<ulong> readyPlayers = new NetworkList<ulong>();
    public NetworkVariable<GameState> gameState = new NetworkVariable<GameState>(GameState.Ready);
    public NetworkVariable<double> roundEndTime = new NetworkVariable<double>();
    public NetworkVariable<int> playerCount = new NetworkVariable<int>(1);
    private double nextRoundTime;

    public enum GameState
    {
        Ready,
        Playing,
        RoundEnd,
    }

    void Awake()
    {
        Instance = this;
    }
    public override void OnNetworkSpawn()
    {
        round.OnValueChanged += OnchangeRound;
        gameState.OnValueChanged += OnchangeState;
        NetworkManager.OnClientDisconnectCallback += OnClientDisconnect;
        NetworkManager.OnClientConnectedCallback += OnClientConnect;
    }
    public override void OnNetworkDespawn()
    {
        round.OnValueChanged -= OnchangeRound;
        gameState.OnValueChanged -= OnchangeState;
        NetworkManager.OnClientDisconnectCallback -= OnClientDisconnect;
        NetworkManager.OnClientConnectedCallback -= OnClientConnect;
    }

    private void OnchangeRound(int oldValue, int newValue)
    {
        Debug.Log("라운드 변경 : " + oldValue + "->" + newValue);
    }
    private void OnchangeState(GameState oldValue, GameState newValue)
    {
        Debug.Log("상태 변경 : " + oldValue + "->" + newValue);
    }

    private void OnClientConnect(ulong clientId)
    {
        if (!IsServer) return;
        Debug.Log("클라이언트 접속 ! : " + clientId);
        playerCount.Value++;
    }
    private void OnClientDisconnect(ulong clientId)
    {
        if (!IsServer) return;
        if (readyPlayers.Contains(clientId))
        {
            readyPlayers.Remove(clientId);
        }
        playerCount.Value--;
        if (gameState.Value == GameState.Ready && readyPlayers.Count == NetworkManager.Singleton.ConnectedClients.Count)
        {
            StartRound();
        }
    }

    void Update()
    {
        if (!IsServer) return;
        if (gameState.Value == GameState.Playing)
        {
            if (NetworkManager.ServerTime.Time >= roundEndTime.Value)
            {
                EndRound();
            }
        }
        if (gameState.Value == GameState.RoundEnd)
        {
            if (NetworkManager.ServerTime.Time >= nextRoundTime)
            {
                NextRound();
            }
        }
    }

    public void EndRound()
    {
        if (!IsServer) return;
        gameState.Value = GameState.RoundEnd;
        nextRoundTime = NetworkManager.ServerTime.Time + 5;
    }

    private void StartRound()
    {
        if (!IsServer) return;
        roundEndTime.Value = NetworkManager.ServerTime.Time + 60f;
        gameState.Value = GameState.Playing;
        readyPlayers.Clear();
    }

    [Rpc(SendTo.Server)]
    public void ReadyRpc(RpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;
        if (readyPlayers.Contains(clientId)) return;
        readyPlayers.Add(clientId);
        if (readyPlayers.Count == NetworkManager.Singleton.ConnectedClients.Count)
        {
            StartRound();
        }
    }

    public void NextRound()
    {
        if (!IsServer) return;
        round.Value++;
        readyPlayers.Clear();
        gameState.Value = GameState.Ready;
    }
}
