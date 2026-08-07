using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private NetworkVariable<int> colorIndex = new NetworkVariable<int>();
    public Renderer playerRenderer;

    public override void OnNetworkSpawn()
    {
        colorIndex.OnValueChanged += ChangeColor;

        if (IsServer)
        {
            colorIndex.Value = (int)OwnerClientId;
        }

        ChangeColor(0, colorIndex.Value);
    }

    [Rpc(SendTo.Everyone)]
    public void ApplyCurrentColor()
    {
        ChangeColor(0, colorIndex.Value);
    }

    private void ChangeColor(int oldValue, int newValue)
    {
        if (newValue == 0)
        {
            playerRenderer.material.color = Color.blue;
        }
        else
        {
            playerRenderer.material.color = Color.red;
        }
    }
    [Rpc(SendTo.Server)]
    private void ChangeColorRpc()
    {
        Debug.Log($"RPC 실행 위치 - IsServer: {IsServer}");
        if (colorIndex.Value == 0)
        {
            colorIndex.Value = 1;
        }
        else
        {
            colorIndex.Value = 0;
        }
    }
}
