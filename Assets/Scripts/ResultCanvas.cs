using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultCanvas : MonoBehaviour {
    [SerializeField] List<Image> playerWinPoints;
    [SerializeField] List<Image> comWinPoints;
    [SerializeField] Text resultText;

    public void Init() {
        for (int n = 0; n < playerWinPoints.Count; n++)
            playerWinPoints[n].enabled = false;
        for (int n = 0; n < comWinPoints.Count; n++)
            comWinPoints[n].enabled = false;
    }

    public void ShowCrown(Owner owner, int winCount) {
        if (owner == Owner.PLAYER) {
            playerWinPoints[winCount].enabled = true;
        } else {
            comWinPoints[winCount].enabled = true;
        }
    }

    public void HideResultText() {
        resultText.enabled = false;
    }

    public void ShowResultText(string sentence) {
        resultText.text = sentence;
        resultText.enabled = true;
    }
}
