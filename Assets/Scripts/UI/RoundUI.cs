using TMPro;
using Unity.Netcode;
using UnityEngine;

public class RoundUI : MonoBehaviour
{
    public TMP_Text roundTimeText;
    public TMP_Text roundStateText;

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.Instance == null) return;

        if (GameStateManager.Instance.gameState.Value == GameStateManager.GameState.Ready)
        {
            roundTimeText.text = "";
            roundStateText.text = "READY";
        }
        else if (GameStateManager.Instance.gameState.Value == GameStateManager.GameState.Playing)
        {
            roundStateText.text = "";
            double remainTime = GameStateManager.Instance.roundEndTime.Value - NetworkManager.Singleton.ServerTime.Time;
            if (remainTime > 0)
            {
                roundTimeText.text = remainTime.ToString("F0");
            }
            else
            {
                roundTimeText.text = "0";
            }
        }
        else if (GameStateManager.Instance.gameState.Value == GameStateManager.GameState.RoundEnd)
        {
            roundTimeText.text = "0";
            roundStateText.text = "ROUND END";
        }
    }
}
