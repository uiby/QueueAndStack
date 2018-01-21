using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Com : BasePlayer {
    [SerializeField] GameSystem gameSystem;
    [SerializeField] Player player; //プレイヤーの情報
    Block submitBlock;
    bool canSubmit;

    public IEnumerator TurnAction() {
        yield return StartCoroutine(Think());

        if (!canSubmit)
            yield return false;
        yield return new WaitForSeconds(1f);
        Action();
    }

    public IEnumerator SelectDataStruct() {
        yield return new WaitForSeconds(2f);
        gameSystem.SelectSubmitMethod((DataStruct)Random.Range(0, 1));
    }

    IEnumerator Think() {
        canSubmit = true;
        //自分の手札を把握
        var myHandBlocks = GetMyHandBlocks();
        //敵の手札を把握
        var opHandBlocks = player.GetMyHandBlocks();
        yield return null;
        if (myHandBlocks.Count == 0) { //強制終了
            ForceFinish();
            canSubmit = false;
            yield break;
        }

        var index = 0;

        //自分の手札を場を把握
        var myHandValue = GetValueByBlocksList(myHandBlocks);
        var mySubmitValue = GetValueByBlocksList(GetSubmitBlocks());
        //敵の手札を場を把握
        var opHandValue = GetValueByBlocksList(opHandBlocks);
        var opSubmitValue = GetValueByBlocksList(player.GetSubmitBlocks());

        if (dataStruct == DataStruct.STACK) {
            var result = CaptureQueue(mySubmitValue, opSubmitValue, myHandValue, opHandValue, recallCount);
            index = myHandBlocks.FindIndex(item => item.GetValue() == result);
        } else {
            //ランダムに提出
            index = Random.Range(0, myHandBlocks.Count);
        }


        submitBlock = myHandBlocks[index];
    }

    void Action() {
        Submit(submitBlock);
    }

    //敵がキューだった場合の攻略方法 (自分はスタック)
    //必須条件　自分の手札がある
    //必要な情報　現在の提出状況(お互い)
    //お互いの手札
    //お互いの残り取り出し回数
    //
    int CaptureQueue(List<int> mySubmitList, List<int> opSubmitList, List<int> myHandList, List<int> opHandList, int myRecallCount) {
        var result = 0;
        //場の積み上げ数を比較する
        var submitedBlocksMany = mySubmitList.Count >= opSubmitList.Count ? Owner.COM : Owner.PLAYER; //積み上げ数が自分の方が多いならCOM

        if (submitedBlocksMany == Owner.COM) { //自分の方が積みあがってる場合: 常に相手の手札の最大コストに勝てる自分の最小コストをだす
            var opHandMax = opHandList.Max();
            myHandList.Sort(); //昇順(後半ほど大きい)にソート
            if (myHandList.Exists(value => value > opHandMax)) {                
                result = myHandList.Find(value => value > opHandMax);
            } else {
                result = myHandList.Min(); //敵に勝てない場合、最小値を積む
            }
        } else { //敵の方が積みあがってる場合: 
            var nextSubmitNumber = mySubmitList.Count;
            var opValue = opSubmitList[nextSubmitNumber];
            myHandList.Sort(); //昇順(後半ほど大きい)にソート
            if (myHandList.Exists(value => value > opValue)) { //場に勝てるカードがある場合
                result = myHandList.Find(value => value > opValue);
            } else { //勝てない場合
                result = myHandList.Min(); //敵に勝てない場合、最小値を積む
            }
        }

        Debug.Log("submit:"+ result);
        return result;
    }
}
