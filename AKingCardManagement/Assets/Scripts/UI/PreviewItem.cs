using AKingCard;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace AKingCard
{
    public class PreviewItem : MonoBehaviour
    {
        private RectTransform thisRect;
        public Text nameText;
        public TextMeshProUGUI widthText;
        public TextMeshProUGUI heightText;

        public RectTransform XRect;
        public RectTransform YRect;

        public TextMeshProUGUI XText;
        public TextMeshProUGUI YText;

        void Start() { }
        void Update() { }

        public void Init(DataCardItem data)
        {
            float sizex = data.size.x;
            float sizey = data.size.y;
            float posx = data.position.x;
            float posy = data.position.y;
            thisRect = GetComponent<RectTransform>();
            thisRect.sizeDelta = new Vector2(sizex, sizey);

            nameText.text = UnityWebRequest.UnEscapeURL(data.name);
            widthText.text = sizex.ToString();
            heightText.text = sizey.ToString();

            XText.text = posx == 0 ? "" : posx.ToString();
            YText.text = posy == 0 ? "" : posy.ToString();

            XRect.sizeDelta = new Vector2(posx, XRect.sizeDelta.y);
            YRect.sizeDelta = new Vector2(posy, YRect.sizeDelta.y);
        }
    }
}

