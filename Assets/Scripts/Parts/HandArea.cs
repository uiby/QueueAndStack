using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandArea : MonoBehaviour {

    //初期化
    public void Initialize(List<Block> blockList) {
        if (blockList.Count == 0) return;
        var x = blockList[0].width;
        var y = blockList[0].height;
        var pos = new Vector2(0, -100f);
        for (int n = 0; n < blockList.Count; n++) {
            if (blockList[n].blockState != BlockState.HAND) continue;
            blockList[n].ChangeState(BlockState.HAND);
            blockList[n].TransParent(transform);
            blockList[n].MovePosition(pos);
            pos.y += y + 10;
        }
    }

    public void AddToHand(List<Block> blockList) {
        Initialize(blockList);
    }

    public void Refresh(List<Block> blockList) {
        Initialize(blockList);
    }
}
