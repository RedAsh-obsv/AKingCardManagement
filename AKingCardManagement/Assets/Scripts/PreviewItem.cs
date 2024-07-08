using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreviewItem : MonoBehaviour
{
    private RectTransform thisRect;
    public TextMeshProUGUI widthText;
    public TextMeshProUGUI heightText;

    public RectTransform XRect;
    public RectTransform YRect;

    public TextMeshProUGUI XText;
    public TextMeshProUGUI YText;

    void Start() { }
    void Update() { }

    public void Init(Vector2 size, Vector2 pos)
    {
        thisRect = GetComponent<RectTransform>();
        thisRect.sizeDelta = new Vector2(size.x, size.y);

        widthText.text = size.x.ToString();
        heightText.text = size.y.ToString();

        XText.text = pos.x == 0 ? "" : pos.x.ToString();
        YText.text = pos.y == 0 ? "" : pos.y.ToString();

        XRect.sizeDelta = new Vector2(pos.x, XRect.sizeDelta.y);
        YRect.sizeDelta = new Vector2(pos.y, YRect.sizeDelta.y);
    }
}
