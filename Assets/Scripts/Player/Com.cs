using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Com : BasePlayer {
    Block submitBlock;
    bool canSubmit;

    public IEnumerator TurnAction() {
        yield return StartCoroutine(Think());

        if (!canSubmit)
            yield return false;
        yield return new WaitForSeconds(1f);
        Action();
    }

    IEnumerator Think() {
        //自分の手札を把握
        var myHandList = GetMyHandBlocks();
        yield return null;
        if (myHandList.Count == 0) { //強制終了
            ForceFinish();
            canSubmit = false;
            yield break;
        }

        //ランダムに提出
        var rand = Random.Range(0, myHandList.Count);

        submitBlock = myHandList[rand];
        canSubmit = true;
    }

    List<Block> GetMyHandBlocks() {
        return handBlocks.Where(item => item.blockState == BlockState.HAND).ToList();
    }

    void Action() {
        Submit(submitBlock);
    }
}
