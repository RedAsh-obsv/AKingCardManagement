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
        private const string LogTag = "PageTemplateManage";
        public Transform alertAdd;
        public Transform scrollContent;
        public RectTransform previewParent;
        public TMP_InputField inputName;
        public TMP_InputField inputWidth;
        public TMP_InputField inputHeight;
        public Animator toastWarning;
        public RectTransform toastWarningRect;
        public TextMeshProUGUI toastWarningText;
        public TextMeshProUGUI previewTextTitle;
        public TextMeshProUGUI previewTextSize;

        private Regex intReg = new Regex("^[0-9]*[1-9][0-9]*$");

        private List<DataTemplate> cardTemplates = new List<DataTemplate>();

        private DataTemplate currentTemplate;
        void Start()
        {
            LogManager.Log($"[{LogTag}] Start");
            alertAdd.gameObject.SetActive(false);
            previewTextTitle.text = "-";
            previewTextSize.text = $"- x -";
            List<DataTemplate> dataTemplates = SaveJsonManager.instance.ReadTemplates();
            for (int i = 0; i < dataTemplates.Count; i++)
            {
                CreateNewListItem(dataTemplates[i]);
            }

        }

        void Update() {

        }
        public void InitEdit()
        {
            LogManager.Log($"[{LogTag}] InitEdit");
            if (currentTemplate == null)
            {
                toastWarningText.SetText("必须选择一个模板");
                LayoutRebuilder.ForceRebuildLayoutImmediate(toastWarningRect);
                toastWarning.Play("TemplateAlertAddToastClosing", 0, 0);
            }
            else
            {
                PageManager.instance.OpenTemplateEditPage();
                PageManager.instance.pageTemplateEdit.Init(currentTemplate);
            }
        }
        public void ShowAlertAdd()
        {
            LogManager.Log($"[{LogTag}] ShowAlertAdd");
            alertAdd.gameObject.SetActive(true);
            inputName.SetTextWithoutNotify("");
            inputWidth.SetTextWithoutNotify("600");
            inputHeight.SetTextWithoutNotify("800");
            toastWarning.Play("TemplateAlertAddToastHide", 0, 0);
        }
        public void HideAlertAdd()
        {
            LogManager.Log($"[{LogTag}] HideAlertAdd");
            alertAdd.gameObject.SetActive(false);
        }
        public void ConfirmAlertAdd()
        {
            LogManager.Log($"[{LogTag}] ConfirmAlertAdd");
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
                toastWarningText.SetText("宽度必须为正整数(像素)");
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
                toastWarningText.SetText("高度必须为正整数(像素)");
                LayoutRebuilder.ForceRebuildLayoutImmediate(toastWarningRect);
                toastWarning.Play("TemplateAlertAddToastClosing", 0, 0);
                return;
            }

            DataTemplate newData = new DataTemplate(UnityWebRequest.EscapeURL(inputName.text), new Vector2(Convert.ToInt32(inputWidth.text), Convert.ToInt32(inputHeight.text)));
            SaveJsonManager.instance.SaveTemplate(newData);
            CreateNewListItem(newData);

            alertAdd.gameObject.SetActive(false);
        }

        private void CreateNewListItem(DataTemplate newData)
        {
            LogManager.Log($"[{LogTag}] CreateNewListItem");
            cardTemplates.Add(newData);
            GameObject newObject = Instantiate(PrefabManager.instance.ListItemTemplate, scrollContent);
            newObject.transform.SetAsFirstSibling();
            newObject.GetComponent<ListItemTemplate>().Init(newData, OnClickListItem, DeleteTemplateItemSort);
            newObject.GetComponent<Button>().Select();
            //OnClickListItem(newData);
        }

        private void OnClickListItem(DataTemplate itemData)
        {
            LogManager.Log($"[{LogTag}] OnClickListItem");
            currentTemplate = itemData;
            previewTextTitle.text = UnityWebRequest.UnEscapeURL(itemData.name);
            previewTextSize.text = $"{itemData.size.x} x {itemData.size.y}";
            CreateNewTemplatePreView(itemData);
        }
        private void DeleteTemplateItemSort(GameObject deleteObj)
        {
            ListItemTemplate deleteItem = deleteObj.GetComponent<ListItemTemplate>();
            SaveJsonManager.instance.DeleteTemplate(deleteItem.thisData);
            DestroyImmediate(deleteObj);
            ClearTemplatePreView();
        }
        private void ClearTemplatePreView()
        {
            LogManager.Log($"[{LogTag}] ClearTemplatePreView");
            if (previewParent.childCount != 0)
            {
                foreach (Transform child in previewParent)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        private void CreateNewTemplatePreView(DataTemplate data)
        {
            LogManager.Log($"[{LogTag}] CreateNewTemplatePreView");
            ClearTemplatePreView();
            
            GameObject PreviewRoot = Instantiate(PrefabManager.instance.PreviewRoot, previewParent);
            
            RectTransform PreviewRootRect = PreviewRoot.GetComponent<RectTransform>();
            PreviewRootRect.sizeDelta = data.cardItems[0].size;

            for (int i = 0; i < data.cardItems.Count; i++)
            {
                GameObject newItem = Instantiate(PrefabManager.instance.PreviewItem, PreviewRootRect);
                RectTransform newItemRect = newItem.GetComponent<RectTransform>();
                newItemRect.localPosition = data.cardItems[i].position;
                PreviewItem newItemPreview = newItem.GetComponent<PreviewItem>();
                newItemPreview.Init(data.cardItems[i]);
            }

            Vector2 contentSize = data.cardItems[0].size;
            Vector2 containerSize = previewParent.sizeDelta;
            if (contentSize.x / contentSize.y > containerSize.x / containerSize.y)  //更宽
            {
                Vector2 targetSize = Vector2.one;
                targetSize.x = containerSize.x / contentSize.x;
                targetSize.y = (contentSize.y * (containerSize.x / contentSize.x)) / contentSize.y;
                PreviewRootRect.localScale = targetSize;
            }
            else //更高
            {
                Vector2 targetSize = Vector2.one;
                targetSize.y = containerSize.y / contentSize.y;
                targetSize.x = (contentSize.x * (containerSize.y / contentSize.y)) / contentSize.x;
                PreviewRootRect.localScale = targetSize;
            }
        }
    }
}