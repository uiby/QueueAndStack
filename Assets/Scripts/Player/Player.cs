using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BasePlayer {

    public void OnSubmit(Block block) {
        if (block.owner == Owner.PLAYER && block.blockState == BlockState.HAND)
            Submit(block);
    }

    public void OnRecall() {
        Recall();
    }
}
