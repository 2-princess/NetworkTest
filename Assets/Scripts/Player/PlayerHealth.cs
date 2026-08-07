using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    private NetworkVariable<int> hp = new NetworkVariable<int>(10);
    private NetworkVariable<bool> isDead = new NetworkVariable<bool>(false);
    private SpawnPoint currentSpawnPoint;
    [SerializeField] private PlayerNetwork playerNetwork;
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private float respawnDelay = 3f;
    public TMP_Text hpText;


    public override void OnNetworkSpawn()
    {
        hp.OnValueChanged += ChangeHp;
        hpText.text = hp.Value.ToString();
        isDead.OnValueChanged += ChangeDeadState;
    }

    [Rpc(SendTo.Owner)]
    private void MoveToRespawnPointRpc(Vector3 position)
    {
        transform.position = position;
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        Respawn();
    }
    public void Respawn()
    {
        if (!IsServer) return;
        hp.Value = 100;
        isDead.Value = false;
        if (currentSpawnPoint != null)
        {
            currentSpawnPoint.isUsed = false;
        }
        currentSpawnPoint = SpawnManager.Instance.GetEmptySpawnPoint();
        MoveToRespawnPointRpc(currentSpawnPoint.transform.position);
        playerNetwork.ApplyCurrentColorRpc();
    }

    private void ChangeHp(int oldHp, int newHp)
    {
        Debug.Log($"{name} 체력: {oldHp} → {newHp}");
        hpText.text = newHp.ToString();
    }

    public void TakeDamage(int damage)
    {
        if (!IsServer) return;

        hp.Value -= damage;
        if (hp.Value <= 0)
        {
            isDead.Value = true;
            StartCoroutine(RespawnRoutine());
        }
    }

    private void ChangeDeadState(bool oldValue, bool newValue)
    {
        Debug.Log($"{name} 사망사태: {oldValue} -> {newValue}");
        if (newValue)
        {
            playerRenderer.material.color = Color.gray;
        }
    }
    public bool IsDead()
    {
        return isDead.Value;
    }

}

