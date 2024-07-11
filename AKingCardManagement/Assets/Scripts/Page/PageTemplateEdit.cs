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
    public enum CardItemType
    {
        Image,
        Text,
    }

    public class PageTemplateEdit : MonoBehaviour
    {
        private const string LogTag = "PageTemplateEdit";
        public PageManager pageManager;
        public Transform alertBack;
        public Transform scrollContent;
        public RectTransform previewParent;
        public RectTransform previewRoot;
        public TextMeshProUGUI previewTextTitle;
        public TextMeshProUGUI previewTextSize;
        public GameObject ConfigPanel;
        public TMP_InputField ConfigPanelTitle;
        public TMP_InputField ConfigPanelWidth;
        public TMP_InputField ConfigPanelHeight;
        public TMP_InputField ConfigPanelX;
        public TMP_InputField ConfigPanelY;
        public TMP_InputField ConfigPanelTexture;
        public Button ConfigPanelButtonTexture;

        public Transform alertAdd;
        public Button alertAddButtonImage;
        public Button alertAddButtonText;
        public Transform alertAddButtonHighlight;
        public TMP_InputField inputName;
        public TMP_InputField inputWidth;
        public TMP_InputField inputHeight;
        public Animator toastWarning;
        public RectTransform toastWarningRect;
        public TextMeshProUGUI toastWarningText;

        private Regex intReg = new Regex("^[0-9]*[1-9][0-9]*$");
        private CardItemType addingItemType = CardItemType.Image;

        private DataTemplate currentTemplate;
        private bool changeHasSaved = false;
        private DateTime changeTimer;
        void Start()
        {
            Debug.Log($"[{LogTag}] Start");
            alertBack.gameObject.SetActive(false);
            alertAdd.gameObject.SetActive(false);
            previewTextTitle.text = "-";
            previewTextSize.text = $"- x -";
            changeTimer = DateTime.Now;
        }

        void Update()
        {
            if ((changeTimer - DateTime.Now)>new TimeSpan(0,0,1))
            {
                CheckChangeSaved();
                changeTimer = DateTime.Now;
            }
        }

        public void Init(DataTemplate data)
        {
            Debug.Log($"[{LogTag}] Init");
            CreateNewTemplatePreView(data);
            LoadSavedListItems(data);
            previewTextTitle.text = UnityWebRequest.UnEscapeURL(data.name);
            previewTextSize.text = $"{data.size.x} x {data.size.y}";
            ConfigPanel.SetActive(false);
            currentTemplate = data;

        } 
        public void ShowAlertBack()
        {
            Debug.Log($"[{LogTag}] ShowAlertBack");
            alertBack.gameObject.SetActive(true);
        }
        public void HideAlertBack()
        {
            Debug.Log($"[{LogTag}] HideAlertBack");
            alertBack.gameObject.SetActive(false);
        }
        public void CheckBack()
        {
            Debug.Log($"[{LogTag}] CheckBack");
            if (changeHasSaved)
                pageManager.OpenTemplateManagePage();
            else
                ShowAlertBack();
        }
        public void CheckChangeSaved()//!!
        {
            Debug.Log($"[{LogTag}] CheckChangeSaved");
            if (currentTemplate.Equals(currentTemplate))
                changeHasSaved = true;
            else
                changeHasSaved = false;
        }

        public void SaveChange()
        {

        }

        public void ShowAlertAdd()
        {
            Debug.Log($"[{LogTag}] ShowAlertAdd");
            alertAdd.gameObject.SetActive(true);
            inputName.SetTextWithoutNotify("");
            inputWidth.SetTextWithoutNotify("100");
            inputHeight.SetTextWithoutNotify("100");
            addingItemType = CardItemType.Image;
            alertAddButtonImage.Select();
            alertAddButtonHighlight.position = alertAddButtonImage.transform.position;
        }
        public void HideAlertAdd()
        {
            Debug.Log($"[{LogTag}] HideAlertAdd");
            alertAdd.gameObject.SetActive(false);
        }
        public void OnSelectAddingImage()
        {
            Debug.Log($"[{LogTag}] OnSelectAddingImage");
            addingItemType = CardItemType.Image;
            alertAddButtonImage.Select();
            alertAddButtonHighlight.position = alertAddButtonImage.transform.position;
        }
        public void OnSelectAddingText()
        {
            Debug.Log($"[{LogTag}] OnSelectAddingText");
            addingItemType = CardItemType.Text;
            alertAddButtonText.Select();
            alertAddButtonHighlight.position = alertAddButtonText.transform.position;
        }
        public void ConfirmAlertAdd()
        {
            Debug.Log($"[{LogTag}] ConfirmAlertAdd");
            if (string.IsNullOrEmpty(inputName.text))
            {
                toastWarningText.SetText("组件名不能为空");
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
            if (addingItemType == CardItemType.Image)
            {
                CreateNewListItemImage(inputName.text, Convert.ToInt32(inputWidth.text), Convert.ToInt32(inputHeight.text));
            }
            else
            {
                CreateNewListItemText(inputName.text, Convert.ToInt32(inputWidth.text), Convert.ToInt32(inputHeight.text));
            }


            alertAdd.gameObject.SetActive(false);
        }
        private void LoadSavedListItems(DataTemplate data)
        {
            Debug.Log($"[{LogTag}] LoadSavedListItems");

            for (int i = 0; i < data.cardItems.Count; i++)
            {
                GameObject newObject = Instantiate(PrefabManager.instance.ListItemImage, scrollContent);
                newObject.transform.SetAsFirstSibling();
                newObject.GetComponent<ListItemEditImage>().Init(data.cardItems[i], OnClickListItem);
                newObject.GetComponent<Button>().Select();
                CreateNewPreViewItemImage(data.cardItems[i], previewRoot);
            }
        }

        private void CreateNewListItemImage(string Name, int width, int height)
        {
            Debug.Log($"[{LogTag}] CreateNewListItemImage");
            long nextIndex = currentTemplate.cardItems.Count;
            CardItemImage newData = new CardItemImage(currentTemplate, nextIndex, UnityWebRequest.EscapeURL(Name), new Vector2(width, height), Vector2.zero);
            currentTemplate.cardItems.Add(newData);
            GameObject newObject = Instantiate(PrefabManager.instance.ListItemImage, scrollContent);
            newObject.transform.SetAsFirstSibling();
            newObject.GetComponent<ListItemEditImage>().Init(newData, OnClickListItem);
            newObject.GetComponent<Button>().Select();
            CreateNewPreViewItemImage(newData, previewRoot);
            OnClickListItem(newData);
        }
        private void CreateNewListItemText(string Name, int width, int height)
        {
            Debug.Log($"[{LogTag}] CreateNewListItemText");
        }
        private void OnClickListItem(CardItem itemData)
        {
            Debug.Log($"[{LogTag}] OnClickListItem");
            ShowConfigPanel(itemData);
        }
        private void ShowConfigPanel(CardItem itemData)
        {
            Debug.Log($"[{LogTag}] ShowConfigPanel");
            ConfigPanel.SetActive(true);
            ConfigPanelTitle.text = itemData.name;
            ConfigPanelWidth.text = itemData.size.x.ToString();
            ConfigPanelHeight.text = itemData.size.y.ToString();
            ConfigPanelX.text = itemData.position.x.ToString();
            ConfigPanelY.text = itemData.position.y.ToString();
            ConfigPanelTexture.text = itemData.content;
        }
        public void HideConfigPanel()
        {
            Debug.Log($"[{LogTag}] ShowConfigPanel");
            ConfigPanel.SetActive(false);
        }
        private void CreateNewTemplatePreView(DataTemplate data)
        {
            Debug.Log($"[{LogTag}] CreateNewTemplatePreView");
            if (previewParent.childCount != 0)
            {
                foreach (Transform child in previewParent)
                {
                    Destroy(child.gameObject);
                }
            }
            GameObject PreviewRootObject = Instantiate(PrefabManager.instance.PreviewRoot, previewParent);

            previewRoot = PreviewRootObject.GetComponent<RectTransform>();
            previewRoot.sizeDelta = data.cardItems[0].size;

            Vector2 contentSize = data.cardItems[0].size;
            Vector2 containerSize = previewParent.sizeDelta;
            if (contentSize.x / contentSize.y > containerSize.x / containerSize.y)  //更宽
            {
                Vector2 targetSize = Vector2.one;
                targetSize.x = containerSize.x / contentSize.x;
                targetSize.y = (contentSize.y * (containerSize.x / contentSize.x)) / contentSize.y;
                previewRoot.localScale = targetSize;
            }
            else //更高
            {
                Vector2 targetSize = Vector2.one;
                targetSize.y = containerSize.y / contentSize.y;
                targetSize.x = (contentSize.x * (containerSize.y / contentSize.y)) / contentSize.x;
                previewRoot.localScale = targetSize;
            }
        }

        private void CreateNewPreViewItemImage(CardItem itemData, RectTransform parent)
        {
            GameObject newItem = Instantiate(PrefabManager.instance.PreviewItem, parent);
            RectTransform newItemRect = newItem.GetComponent<RectTransform>();
            newItemRect.localPosition = itemData.position;
            PreviewItem newItemPreview = newItem.GetComponent<PreviewItem>();
            newItemPreview.Init(itemData.size, itemData.position);
            newItemRect.localScale = Vector3.one;
        }
    }
}