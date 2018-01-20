using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour {
    [SerializeField] SelectSubmitMethod selectSubmitCanvas;
    [SerializeField] Player player;
    [SerializeField] Com com;

    Owner selecter;
    Owner playerTurn;
    bool selected;
    int  turnCount;

	// Use this for initialization
	void Start () {
        selecter = Owner.PLAYER; //最初はプレイヤーが選べる
		player.Initialize();
        com.Initialize();

        StartCoroutine(GameLoop());
	}

    void Reset() {
        selected = false;
        turnCount = 0;
    }

    public void SelectSubmitMethod(DataStruct dataStruct) {
        var opData = dataStruct == DataStruct.QUEUE ? DataStruct.STACK : DataStruct.QUEUE;
        //データ構造の決定
        if (selecter == Owner.PLAYER) {
            player.DecideDataStruct(dataStruct);
            com.DecideDataStruct(opData);
        } else {
            com.DecideDataStruct(dataStruct);
            player.DecideDataStruct(opData);
        }
        selected = true;
    }

    void DeceideFirstTurnPlayer() {
        playerTurn = player.dataStruct == DataStruct.STACK ? Owner.PLAYER : Owner.COM;
    }

    IEnumerator GameLoop() {
        Reset();
        //各プレイヤーの情報のリセット
        player.Reset();
        com.Reset();
        yield return null;

        selectSubmitCanvas.Show();
        //データ構造の決定
        while (selected == false) {
            yield return null;
        }
        selectSubmitCanvas.Hide();

        //先行後行の決定
        DeceideFirstTurnPlayer();
        yield return null;

        //----ゲーム開始----//
        //8回行う
          //先行ターン
          //後行ターン
        while (turnCount < 16) {
            Debug.Log("turn:"+turnCount+" player:"+ playerTurn);
            if (playerTurn == Owner.PLAYER) {
                player.InitState();
                yield return StartCoroutine(player.TurnLoop());
            } else {
                com.InitState();
                yield return StartCoroutine(com.TurnAction());
                yield return StartCoroutine(com.TurnLoop());                
            }
            turnCount++;
            playerTurn = playerTurn == Owner.PLAYER ? Owner.COM : Owner.PLAYER;
        }

        Debug.Log("finish turn");
        //勝敗
        yield return StartCoroutine(ShowResult(player.GetSubmitBlocks(), com.GetSubmitBlocks()));

        //if 2勝した場合、ゲーム終了

        //else 負けた方が先行後行選べる
    }

    IEnumerator ShowResult(List<Block> playerBlocks, List<Block> comBlocks) {
        var playerWinCount = 0;
        var comWinCount = 0;
        var battleCount = playerBlocks.Count;
        for (int n = 0; n < battleCount; n++) {
            var playerValue = playerBlocks[n].GetValue();
            var comValue = comBlocks[n].GetValue();

            if ((playerValue == 1 && comValue == 6) || (playerValue > comValue)) { //playerの勝利
                playerWinCount++;
                player.WinBattle(n);
                comBlocks[n].Lose();
            } else if ((comValue == 1 && playerValue == 6) || (comValue > playerValue)) { //comの勝利
                comWinCount++;
                com.WinBattle(n);
                playerBlocks[n].Lose();
            }

            yield return new WaitForSeconds(0.3f);
        }

        yield return null;
    }
}
