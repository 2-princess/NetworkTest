using Unity.Netcode;
using UnityEngine;

public class PlayerMove : NetworkBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.position += new Vector3(h, 0, v) * 5 * Time.deltaTime;

    }
}
