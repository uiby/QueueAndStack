using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour {
    [SerializeField, Range(1, 6)] int value = 1;
    public int GetValue() {
        return value;
    }
    public Owner owner{get; private set;}
    public BlockState blockState{get; private set;}
    Text valueText;
    RectTransform rectTrans;
    Color initColor;

    [HideInInspector] public float width{get; private set;}
    [HideInInspector] public float height{get; private set;}

	// Use this for initialization
	void Awake () {
        rectTrans = GetComponent<RectTransform>();
        valueText = GetComponentInChildren<Text>();
        var size = GetComponent<RectTransform>().sizeDelta;
        width = size.x;
        height = size.y;
        initColor = GetComponent<Image>().color;
	}

    public void Initialize(Owner _owner) {
        owner = _owner;
        valueText.text = value.ToString();
        if (value == 6)
            valueText.text = "Joker";
        GetComponent<Image>().color = initColor;
    }

    public void ChangeState(BlockState state) {
        blockState = state;
    }

    public void MovePosition(Vector2 pos) {
        //rectTrans.localPosition = pos;
        StartCoroutine(Move(pos));
    }

    public void TransParent(Transform parent) {
        //transform.parent = this.transform.parent;
        transform.SetParent(parent);
    }

    public void Lose() {
        GetComponent<Image>().color = new Color (1, 1, 1, 1);
        valueText.text = "";
    }

    IEnumerator Move(Vector2 idealPos) {
        var startPos = (Vector2)rectTrans.localPosition;
        var endPos = idealPos;
        var timer = 0f;
        var rate = 0f;
        var duration = 0.4f;

        while (rate < 1) {
            timer += Time.deltaTime;
            rate = Mathf.Clamp(timer/duration, 0f, 1f);

            rectTrans.localPosition = Vector2.Lerp(startPos, endPos, rate);
            yield return null;
        }
    }
}
