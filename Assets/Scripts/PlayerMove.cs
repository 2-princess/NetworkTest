using Unity.Netcode;
using UnityEngine;

public class PlayerMove : NetworkBehaviour
{
    public Renderer playerRenderer;
    private NetworkVariable<int> colorIndex = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        colorIndex.OnValueChanged += ChangeColor;

        if (IsServer)
        {
            colorIndex.Value = (int)OwnerClientId;
        }

        ChangeColor(0, colorIndex.Value);
    }

    void Update()
    {
        if (!IsOwner) return;
        if (IsServer)
        {
            colorIndex.Value = (int)OwnerClientId;
        }

        float h = Input.GetAxisRaw("Horizontal");
        transform.position += Vector3.right * h * 3f * Time.deltaTime;
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
}
