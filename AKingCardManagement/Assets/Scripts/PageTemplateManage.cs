using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace AKingCard
{
    public class PageTemplateManage : MonoBehaviour
    {
        public GameObject PrefabListItemTemplate;
        public GameObject PrefabPreViewItem;
        public Transform alertAdd;
        public Transform ScrollContent;
        public RectTransform PreviewParent;
        public TMP_InputField inputName;
        public TMP_InputField inputWidth;
        public TMP_InputField inputHeight;
        public Animator toastWarning;
        public RectTransform toastWarningRect;
        public TextMeshProUGUI toastWarningText;
        public TextMeshProUGUI previewTextTitle;
        public TextMeshProUGUI previewTextSize;
        private Regex intReg = new Regex("^[0-9]*[1-9][0-9]*$");

        private List<CardTemplate> cardTemplates = new List<CardTemplate>();

        private CardTemplate currentTemplate;
        void Start()
        {
            alertAdd.gameObject.SetActive(false);
            previewTextTitle.text = "-";
            previewTextSize.text = $"- x -";
        }

        void Update() {

        }
        public void ShowAlertAdd()
        {
            alertAdd.gameObject.SetActive(true);
            inputName.SetTextWithoutNotify("");
            inputWidth.SetTextWithoutNotify("");
            inputHeight.SetTextWithoutNotify("");
            toastWarning.Play("TemplateAlertAddToastHide", 0, 0);
        }
        public void HideAlertAdd()
        {
            alertAdd.gameObject.SetActive(false);
        }
        public void ConfirmAlertAdd()
        {
            if (string.IsNullOrEmpty(inputName.text))
            {
                toastWarningText.SetText("模板名不能为空");
                LayoutRebuilder.ForceRebuildLayoutImmediate(toastWarningRect);
                toastWarning.Play("TemplateAlertAddToastClosing", 0, 0);
                return;
            }
            if (string.IsNullOrEmpty(inputWidth.text))
            {
                toastWarningText.SetText("宽度不能为空");
                LayoutRebuilder.ForceRebuildLayoutImmediate(toastWarningRect);
                toastWarning.Play("TemplateAlertAddToastClosing", 0, 0);
                return;
            }
            if (!intReg.IsMatch(inputWidth.text))
            {
                toastWarningText.SetText("宽度必须为正整数(像素数)");
                LayoutRebuilder.ForceRebuildLayoutImmediate(toastWarningRect);
                toastWarning.Play("TemplateAlertAddToastClosing", 0, 0);
                return;
            }
            if (string.IsNullOrEmpty(inputHeight.text))
            {
                toastWarningText.SetText("高度不能为空");
                LayoutRebuilder.ForceRebuildLayoutImmediate(toastWarningRect);
                toastWarning.Play("TemplateAlertAddToastClosing", 0, 0);
                return;
            }
            if (!intReg.IsMatch(inputHeight.text))
            {
                toastWarningText.SetText("高度必须为正整数(像素数)");
                LayoutRebuilder.ForceRebuildLayoutImmediate(toastWarningRect);
                toastWarning.Play("TemplateAlertAddToastClosing", 0, 0);
                return;
            }

            CreateNewData(inputName.text, Convert.ToInt32(inputWidth.text), Convert.ToInt32(inputHeight.text));

            alertAdd.gameObject.SetActive(false);
        }

        private void CreateNewData(string Name, int width, int height)
        {
            CardTemplate newData = new CardTemplate(UnityWebRequest.EscapeURL(Name), new Vector2(width, height));
            newData.cardItems.Add(new CardItemImage(1, "test", new Vector2(100, 100), new Vector2(100, 100)));
            newData.cardItems.Add(new CardItemImage(2, "test2", new Vector2(200, 200), new Vector2(400, 400)));
            cardTemplates.Add(newData);
            GameObject newObject = Instantiate(PrefabListItemTemplate, ScrollContent);
            newObject.transform.SetAsFirstSibling();
            newObject.GetComponent<ListItemTemplate>().Init(newData, OnClickListItem);
            newObject.GetComponent<Button>().Select();
            OnClickListItem(newData);
        }

        private void OnClickListItem(CardTemplate itemData)
        {
            currentTemplate = itemData;
            previewTextTitle.text = UnityWebRequest.UnEscapeURL(itemData.name);
            previewTextSize.text = $"{itemData.size.x} x {itemData.size.y}";
            CreateNewTemplatePreView(itemData);
        }
        private void CreateNewTemplatePreView(CardTemplate data)
        {
            if (PreviewParent.childCount != 0)
            {
                foreach(Transform child in PreviewParent)
                {
                    Destroy(child.gameObject);
                }
            }
            GameObject rootObject = new GameObject("RootObject");
            GameObject Background = Instantiate(PrefabPreViewItem, PreviewParent);
            RectTransform BackgroundRect = Background.GetComponent<RectTransform>();
            PreviewItem BackgroundPreview = Background.GetComponent<PreviewItem>();
            BackgroundPreview.Init(data.cardItems[0].size, data.cardItems[0].position);

            for (int i = 1; i < data.cardItems.Count; i++)
            {
                GameObject newItem = Instantiate(PrefabPreViewItem, BackgroundRect);
                RectTransform newItemRect = newItem.GetComponent<RectTransform>();
                newItemRect.localPosition = data.cardItems[i].position;
                PreviewItem newItemPreview = newItem.GetComponent<PreviewItem>();
                newItemPreview.Init(data.cardItems[i].size, data.cardItems[i].position);
            }

            Vector2 contentSize = data.cardItems[0].size;
            Vector2 containerSize = PreviewParent.sizeDelta;
            if (contentSize.x / contentSize.y > containerSize.x / containerSize.y)  //更宽
            {
                Vector2 targetSize = Vector2.one;
                targetSize.x = containerSize.x / contentSize.x;
                targetSize.y = (contentSize.y * (containerSize.x / contentSize.x)) / contentSize.y;
                BackgroundRect.localScale = targetSize;
            }
            else //更高
            {
                Vector2 targetSize = Vector2.one;
                targetSize.y = containerSize.y / contentSize.y;
                targetSize.x = (contentSize.x * (containerSize.y / contentSize.y)) / contentSize.x;
                BackgroundRect.localScale = targetSize;
            }
        }
    }
}