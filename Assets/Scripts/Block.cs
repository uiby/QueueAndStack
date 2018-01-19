using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour {
    [SerializeField] BlockType blockType;
    [SerializeField] int value;
    Text valueText;
    RectTransform rectTrans;

    [HideInInspector] public float width{get; private set;}
    [HideInInspector] public float height{get; private set;}

	// Use this for initialization
	void Start () {
        rectTrans = GetComponent<RectTransform>();
        valueText = GetComponentInChildren<Text>();
        valueText.text = value.ToString();
        var size = GetComponent<RectTransform>().sizeDelta;
        width = size.x;
        height = size.y;
	}

    public void MovePosition(Vector2 pos) {
        rectTrans.localPosition = pos;
    }

    public void TransParent(Transform parent) {
        transform.parent = null;
        transform.SetParent(parent);
    }
}
