using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerAttack : NetworkBehaviour
{
    public GameObject attackEffect;
    private float nextAttackTime;
    public Transform attackPoint;
    [SerializeField] private PlayerHealth playerHealth;

    void Update()
    {

        if (!IsOwner) return;
        if (playerHealth.IsDead()) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttackRpc();
        }
    }

    [Rpc(SendTo.Server)]
    void AttackRpc()
    {
        if (Time.time < nextAttackTime) return;

        Collider[] hits = Physics.OverlapSphere(attackPoint.position, 1f);
        foreach (Collider hit in hits)
        {
            NetworkObject target = hit.GetComponentInParent<NetworkObject>();

            if (target.NetworkObjectId == NetworkObjectId) continue;

            PlayerHealth targetHealth = target.GetComponent<PlayerHealth>();
            if (hit.CompareTag("Player") && targetHealth != null)
            {
                targetHealth.TakeDamage(1);
                Debug.Log("플레이어발견" + hit.name);
            }
        }
        nextAttackTime = Time.time + 1f;
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
