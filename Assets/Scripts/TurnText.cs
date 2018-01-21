using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TurnText : GuiParts {
    public int turnCount{get; private set;}
    Text text;
    Color initColor;

    protected override void Awake() {
        base.Awake();
        turnCount = 0;
        text = GetComponentInChildren<Text>();
        text.text = "";
        initColor = GetComponent<Text>().color;
    }

    public void ChagnePosition(Owner player) {
        var playerPos = new Vector2(-150, -200);
        var comPos = new Vector2(150, -200);

        MovePosition(player == Owner.PLAYER ? playerPos : comPos, 0.2f);
    }

    public void UpdateTurn(int turn) {
        turnCount = turn;
        text.text = turn < 8 ? turn+" TURN" : "LAST TURN";
    }

    public void FinishTurn() {
        text.text = "RESULT";
        MovePosition(new Vector2(0, -200), 0.3f);
    }
}
