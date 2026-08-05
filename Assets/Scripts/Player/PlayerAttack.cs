using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerAttack : NetworkBehaviour
{
    public GameObject attackEffect;

    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttackRpc();
        }
    }

    [Rpc(SendTo.Server)]
    void AttackRpc()
    {
        PlayerAttackEffectRpc();
    }

    [Rpc(SendTo.Everyone)]
    void PlayerAttackEffectRpc()
    {
        StartCoroutine(ShowAttackEffect());
    }

    IEnumerator ShowAttackEffect()
    {
        attackEffect.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        attackEffect.SetActive(false);
    }
}
