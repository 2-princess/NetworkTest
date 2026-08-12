using Unity.Netcode;
using UnityEngine;

public class GameStateManager : NetworkBehaviour
{
    public static GameStateManager Instance;
    public NetworkVariable<int> round = new NetworkVariable<int>(1);
    public NetworkList<ulong> readyPlayers = new NetworkList<ulong>();
    public NetworkVariable<GameState> gameState = new NetworkVariable<GameState>(GameState.Ready);

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
    }
    public override void OnNetworkDespawn()
    {
        round.OnValueChanged -= OnchangeRound;
        gameState.OnValueChanged -= OnchangeState;
    }

    private void OnchangeRound(int oldValue, int newValue)
    {
        Debug.Log("라운드 변경 : " + oldValue + "->" + newValue);
    }
    private void OnchangeState(GameState oldValue, GameState newValue)
    {
        Debug.Log("상태 변경 : " + oldValue + "->" + newValue);
    }

    public void EndRound()
    {
        if (!IsServer) return;
        gameState.Value = GameState.RoundEnd;
        // NextRound(); 후에 결과보여주고
    }

    private void StartRound()
    {
        if (!IsServer) return;
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
