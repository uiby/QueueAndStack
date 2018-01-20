using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSubmitMethod : MonoBehaviour {
    [SerializeField] GameSystem gameSystem;
    [SerializeField] Text setText;
    [SerializeField] Text selectText;
    [SerializeField] ButtonParts queueButton;
    [SerializeField] ButtonParts stackButton;
    [SerializeField] RectTransform[] fixChildren;
    Image background;
    // Use this for initialization
	void Awake () {
        background = GetComponent<Image>();
	}

    //セット単位のリセット
    public void Reset() {
        queueButton.TransParent(transform);
        queueButton.MovePosition(new Vector2(-100, 0), 0.2f);
        stackButton.TransParent(transform);
        stackButton.MovePosition(new Vector2(100, 0), 0.2f);
    }

    public void Hide() {
        for (int n = 0; n < fixChildren.Length; n++)
            fixChildren[n].gameObject.SetActive(false);
        background.enabled = false;
    }

    public void Show(Owner selecter, int setCount) {
        if (setCount == 1) setText.text = "1'st set";
        else if (setCount == 2) setText.text = "2'nd set";
        else setText.text = "final set";
        selectText.text = selecter == Owner.PLAYER ? "YOU SELECT" : "COM SELECT";
        for (int n = 0; n < fixChildren.Length; n++)
            fixChildren[n].gameObject.SetActive(true);
        background.enabled = true;
    }

    //dataStruct 0:QUEUE, 1:STACK
    public void OnSelectSubmitMethod(int dataStruct) {
        if (gameSystem.selecter == Owner.COM) return;
        if (gameSystem.selected == true) return;
        gameSystem.SelectSubmitMethod((DataStruct)dataStruct);
    }

    public void MoveButtonParts(DataStruct playerData) {
        var playerPos = new Vector2(-70, -140);
        var comPos = new Vector2(70, -140);

        queueButton.MovePosition(playerData == DataStruct.QUEUE ? playerPos : comPos, 0.2f);
        stackButton.MovePosition(playerData == DataStruct.STACK ? playerPos : comPos, 0.2f);
    }
}
