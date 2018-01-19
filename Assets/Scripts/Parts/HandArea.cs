using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandArea : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    //初期化
    public void Initialize(List<Block> blockList) {
        if (blockList.Count == 0) return;
        var x = blockList[0].width;
        var y = blockList[0].height;
        var pos = new Vector2(0, -100f);
        for (int n = 0; n < blockList.Count; n++) {
            blockList[n].TransParent(transform);
            blockList[n].MovePosition(pos);
            pos.y += y + 10;
        }
    }
}
