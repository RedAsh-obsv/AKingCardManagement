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
        private DataTemplateItem thisData;
        public RawImage thisTexture;
        public Text thisText;
        public RectTransform FrontView;
        public Text nameText;
        public TextMeshProUGUI widthText;
        public TextMeshProUGUI heightText;

        public RectTransform XRect;
        public RectTransform YRect;

        public TextMeshProUGUI XText;
        public TextMeshProUGUI YText;

        void Start() { }
        void Update() { }

        public void Init(DataTemplateItem data)
        {
            thisData = data;
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
            if(data.type == DataTemplateItemType.Image)
            {
                DataTemplateItemImage imageData = (DataTemplateItemImage)data;
                if (!string.IsNullOrEmpty(imageData.texturePath))
                {
                    thisTexture.enabled = true;
                    thisText.enabled = false;
                    StartCoroutine(LoadTexture(((DataTemplateItemImage)data).texturePath));
                }
                else
                {
                    thisTexture.enabled = true;
                    thisText.enabled = false;
                }
            }
            else if (data.type == DataTemplateItemType.Text)
            {
                DataTemplateItemText textData = (DataTemplateItemText)data;
                if (!string.IsNullOrEmpty(textData.fontPath))
                {
                    thisTexture.enabled = true;
                    thisText.enabled = false;
                    Font BuildInFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
                    thisText.fontSize = textData.textSize;
                    thisText.color = textData.textColor;
                    thisText.text = textData.textContent;
                    switch (textData.align)
                    {
                        case DataTemplateItemAlign.Left:
                            thisText.alignment = TextAnchor.UpperLeft;
                            break;
                        case DataTemplateItemAlign.Center:
                            thisText.alignment = TextAnchor.UpperCenter;
                            break;
                        case DataTemplateItemAlign.Right:
                            thisText.alignment = TextAnchor.UpperRight;
                            break;
                    }
                }
                else
                {
                    thisTexture.enabled = true;
                    thisText.enabled = false;
                }
            }
        }
        private IEnumerator LoadTexture(string path)
        {
            WWW www = new WWW("file://" + path);
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                LogManager.Log("Load Image Error " + www.error);
                yield break;
            }

            yield return thisTexture.texture = www.texture;
        }
        private IEnumerator LoadFont(string path)
        {
            yield return 0;
        }

        public void InfoOn()
        {
            nameText.gameObject.SetActive(true);
            widthText.gameObject.SetActive(true);
            heightText.gameObject.SetActive(true);
            XRect.gameObject.SetActive(true);
            YRect.gameObject.SetActive(true);
            XText.gameObject.SetActive(true);
            YText.gameObject.SetActive(true);
            FrontView.gameObject.SetActive(true);
            Init(thisData);
        }
        public void InfoOff()
        {
            nameText.gameObject.SetActive(false);
            widthText.gameObject.SetActive(false);
            heightText.gameObject.SetActive(false);
            XRect.gameObject.SetActive(false);
            YRect.gameObject.SetActive(false);
            XText.gameObject.SetActive(false);
            YText.gameObject.SetActive(false);
            FrontView.gameObject.SetActive(false);
        }
    }
}

