using System;
using System.Collections.Generic;
using System.IO;
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
        public RectTransform scrollContentHighlight;
        public RectTransform previewParent;
        public RectTransform previewRoot;
        public TextMeshProUGUI previewTextTitle;
        public TextMeshProUGUI previewTextSize;

        public GameObject ConfigPanelImage;
        public TMP_InputField ConfigPanelImageName;
        public TMP_InputField ConfigPanelImageWidth;
        public TMP_InputField ConfigPanelImageHeight;
        public TMP_InputField ConfigPanelImageX;
        public TMP_InputField ConfigPanelImageY;
        public TMP_InputField ConfigPanelImageTexture;
        public Button ConfigPanelImageButtonTexture;
        
        public GameObject ConfigPanelText;
        public TMP_InputField ConfigPanelTextName;
        public TMP_InputField ConfigPanelTextWidth;
        public TMP_InputField ConfigPanelTextHeight;
        public TMP_InputField ConfigPanelTextX;
        public TMP_InputField ConfigPanelTextY;
        public TMP_InputField ConfigPanelTextFont;
        public Button ConfigPanelTextButtonFont;
        public TMP_InputField ConfigPanelTextSize;
        public TMP_InputField ConfigPanelTextColor;
        public TMP_InputField ConfigPanelTextContent;
        public Button ConfigPanelTextButtonLeft;
        public Button ConfigPanelTextButtonCenter;
        public Button ConfigPanelTextButtonRight;
        public RectTransform ConfigPanelTextButtonHighlight;

        public Button ButtonInfoOn;
        public Button ButtonInfoOff;
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
        public GameObject WarningDot;
        public GameObject WarningHint;

        private Regex intReg = new Regex("^[0-9]*[1-9][0-9]*$");
        private CardItemType addingItemType = CardItemType.Image;

        private DataTemplate currentTemplate;
        private DataTemplateItem currentDataItem;
        //index, ListItem, PreviewItem
        private Dictionary<string, KeyValuePair<ListItemEdit, PreviewItem>> UIItemPairs = new Dictionary<string, KeyValuePair<ListItemEdit, PreviewItem>>();
        private bool changeHasSaved = false;
        private DateTime changeTimer;
        void Start()
        {
            LogManager.Log($"[{LogTag}] Start");
            alertBack.gameObject.SetActive(false);
            alertAdd.gameObject.SetActive(false);
            previewTextTitle.text = "-";
            previewTextSize.text = $"- x -";
            changeTimer = DateTime.Now;
        }

        private void FixedUpdate()
        {
            if ((DateTime.Now - changeTimer) > new TimeSpan(0, 0, 1))
            {
                changeTimer = DateTime.Now;
                if (currentTemplate != null && currentDataItem != null)
                {
                    CheckChangeSaved();
                }
            }
        }

        public void Init(DataTemplate data)
        {
            LogManager.Log($"[{LogTag}] Init");
            CreateNewTemplatePreView(data);
            LoadSavedListItems(data);
            previewTextTitle.text = UnityWebRequest.UnEscapeURL(data.name);
            previewTextSize.text = $"{data.size.x} x {data.size.y}";
            ConfigPanelImage.SetActive(false);
            currentTemplate = data;
            ButtonInfoOn.gameObject.SetActive(false);
            ButtonInfoOff.gameObject.SetActive(true);
        } 
        public void ShowAlertBack()
        {
            LogManager.Log($"[{LogTag}] ShowAlertBack");
            alertBack.gameObject.SetActive(true);
        }
        public void HideAlertBack()
        {
            LogManager.Log($"[{LogTag}] HideAlertBack");
            alertBack.gameObject.SetActive(false);
        }
        public void CheckBack()
        {
            LogManager.Log($"[{LogTag}] CheckBack");
            if (changeHasSaved)
                pageManager.OpenTemplateManagePage();
            else
                ShowAlertBack();
        }


        public void OpenTexture()
        {
            string texturePath =  FileDialogManager.instance.OpenFile();
            if (!string.IsNullOrEmpty(texturePath))
            {
                FileInfo textureInfo = new FileInfo(texturePath);
                if (textureInfo.Extension != ".png" && textureInfo.Extension != ".jpg" && textureInfo.Extension != ".jpeg")
                {
                    toastWarningText.SetText("目前仅支持.png/.jpg./jpeg格式的图片");
                    LayoutRebuilder.ForceRebuildLayoutImmediate(toastWarningRect);
                    toastWarning.Play("TemplateAlertAddToastClosing", 0, 0);
                }
                else
                {
                    ConfigPanelImageTexture.text = texturePath;
                }
            }
        }
        public void CheckChangeSaved()
        {
            //LogManager.Log($"[{LogTag}] CheckChangeSaved");
            DataTemplate savedTemplate = SaveJsonManager.instance.ReadTemplate(currentTemplate.index);
            if (currentTemplate.Equals(savedTemplate))
            {
                changeHasSaved = true;
                WarningDot.SetActive(false);
            }
            else
            {
                changeHasSaved = false;
                WarningDot.SetActive(true);
            }
        }
        public void InfoOn()
        {
            foreach(var itemPair in UIItemPairs)
            {
                itemPair.Value.Value.InfoOn();
            }
            ButtonInfoOn.gameObject.SetActive(false);
            ButtonInfoOff.gameObject.SetActive(true);
        }
        public void InfoOff()
        {
            foreach (var itemPair in UIItemPairs)
            {
                itemPair.Value.Value.InfoOff();
            }
            ButtonInfoOn.gameObject.SetActive(true);
            ButtonInfoOff.gameObject.SetActive(false);
        }

        public void ShowAlertAdd()
        {
            LogManager.Log($"[{LogTag}] ShowAlertAdd");
            alertAdd.gameObject.SetActive(true);
            inputName.SetTextWithoutNotify("");
            inputWidth.SetTextWithoutNotify("100");
            inputHeight.SetTextWithoutNotify("100");
            addingItemType = CardItemType.Image;
            alertAddButtonHighlight.position = alertAddButtonImage.transform.position;
        }
        public void HideAlertAdd()
        {
            LogManager.Log($"[{LogTag}] HideAlertAdd");
            alertAdd.gameObject.SetActive(false);
        }
        public void OnSelectAddingImage()
        {
            LogManager.Log($"[{LogTag}] OnSelectAddingImage");
            addingItemType = CardItemType.Image;
            alertAddButtonHighlight.position = alertAddButtonImage.transform.position;
        }
        public void OnSelectAddingText()
        {
            LogManager.Log($"[{LogTag}] OnSelectAddingText");
            addingItemType = CardItemType.Text;
            alertAddButtonHighlight.position = alertAddButtonText.transform.position;
        }
        public void ConfirmAlertAdd()
        {
            LogManager.Log($"[{LogTag}] ConfirmAlertAdd");
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
            LogManager.Log($"[{LogTag}] LoadSavedListItems");

            UIItemPairs.Clear();
            if (scrollContent.childCount > 0)
            {
                foreach (Transform child in scrollContent)
                {
                    Destroy(child.gameObject); 
                }
            }
            for (int i = 0; i < data.cardItems.Count; i++)
            {
                GameObject newObject = Instantiate(PrefabManager.instance.ListItemImage, scrollContent);
                newObject.transform.SetAsFirstSibling();
                ListItemEditImage newListItemEditImage = newObject.GetComponent<ListItemEditImage>();
                newListItemEditImage.Init(data.cardItems[i], UpdateCardItemSort, DeleteCardItemSort, OnClickListItem);
                PreviewItem newItemPreview = CreateNewPreViewItem(data.cardItems[i], previewRoot);
                UIItemPairs.Add(data.cardItems[i].index, new KeyValuePair<ListItemEdit, PreviewItem>(newListItemEditImage, newItemPreview));
            }
            scrollContentHighlight.localScale = Vector3.zero;
        }

        private void CreateNewListItemImage(string Name, int width, int height)
        {
            LogManager.Log($"[{LogTag}] CreateNewListItemImage");
            long nextIndex = currentTemplate.cardItems.Count;
            DataTemplateItemImage newData = new DataTemplateItemImage($"{currentTemplate.index}{nextIndex.ToString().PadLeft(3, '0')}", UnityWebRequest.EscapeURL(Name), new Vector2(width, height), Vector2.zero);
            currentTemplate.cardItems.Add(newData);
            GameObject newObject = Instantiate(PrefabManager.instance.ListItemImage, scrollContent);
            newObject.transform.SetAsFirstSibling();
            ListItemEditImage newListItemEditImage = newObject.GetComponent<ListItemEditImage>();
            newListItemEditImage.Init(newData, UpdateCardItemSort, DeleteCardItemSort, OnClickListItem);
            scrollContentHighlight.localScale = Vector3.zero;
            PreviewItem newItemPreview = CreateNewPreViewItem(newData, previewRoot);
            UIItemPairs.Add(newData.index, new KeyValuePair<ListItemEdit, PreviewItem>(newListItemEditImage, newItemPreview));
            //OnClickListItem(newObject.transform, newData);
        }
        private void CreateNewListItemText(string Name, int width, int height)
        {
            LogManager.Log($"[{LogTag}] CreateNewListItemText");
        }
        private void OnClickListItem(Transform transObj, DataTemplateItem itemData)
        {
            LogManager.Log($"[{LogTag}] OnClickListItem");
            scrollContentHighlight.position = transObj.position;
            scrollContentHighlight.localScale = Vector3.one;
            currentDataItem = itemData;
            if (itemData.GetType() == typeof(DataTemplateItemImage))
                ShowConfigPanelImage((DataTemplateItemImage)itemData);
            if (itemData.GetType() == typeof(DataTemplateItemText))
                ShowConfigPanelText((DataTemplateItemText)itemData);
        }
        private void ShowConfigPanelImage(DataTemplateItemImage itemData)
        {
            LogManager.Log($"[{LogTag}] ShowConfigPanelImage");
            ConfigPanelImage.SetActive(true);
            ConfigPanelText.SetActive(false);
            ConfigPanelImageName.text = UnityWebRequest.UnEscapeURL(itemData.name);
            ConfigPanelImageWidth.text = itemData.size.x.ToString();
            ConfigPanelImageHeight.text = itemData.size.y.ToString();
            ConfigPanelImageX.text = itemData.position.x.ToString();
            ConfigPanelImageY.text = itemData.position.y.ToString();
            ConfigPanelImageTexture.text = itemData.texturePath;
        }
        private void ShowConfigPanelText(DataTemplateItemText itemData)
        {
            LogManager.Log($"[{LogTag}] ShowConfigPanelText");
            ConfigPanelText.SetActive(true);
            ConfigPanelText.SetActive(false);
            ConfigPanelTextName.text = UnityWebRequest.UnEscapeURL(itemData.name);
            ConfigPanelTextWidth.text = itemData.size.x.ToString();
            ConfigPanelTextHeight.text = itemData.size.y.ToString();
            ConfigPanelTextX.text = itemData.position.x.ToString();
            ConfigPanelTextY.text = itemData.position.y.ToString();
            ConfigPanelTextFont.text = itemData.fontPath;

            ConfigPanelTextContent.text = itemData.textContent;
            ConfigPanelTextSize.text = itemData.textSize.ToString();
            ConfigPanelTextColor.text = "#"+ColorUtility.ToHtmlStringRGBA(itemData.textColor);
            switch (itemData.align)
            {
                case DataTemplateItemAlign.Left:
                    ConfigPanelTextButtonHighlight.position = ConfigPanelTextButtonLeft.gameObject.transform.position;
                    break;
                case DataTemplateItemAlign.Center:
                    ConfigPanelTextButtonHighlight.position = ConfigPanelTextButtonCenter.gameObject.transform.position;
                    break;
                case DataTemplateItemAlign.Right:
                    ConfigPanelTextButtonHighlight.position = ConfigPanelTextButtonRight.gameObject.transform.position;
                    break;
            }
        }
        public void HideConfigPanel()
        {
            LogManager.Log($"[{LogTag}] HideConfigPanel");
            ConfigPanelImage.SetActive(false);
            ConfigPanelText.SetActive(false);
            scrollContentHighlight.localScale = Vector3.zero;
        }
        private void UpdateCardItemSort()
        {
            currentTemplate.cardItems.Clear();
            if (scrollContent.childCount > 0)
            {
                foreach(Transform item in scrollContent)
                {
                    currentTemplate.cardItems.Add(item.GetComponent<ListItemEditImage>().thisData);
                }
            }
            currentTemplate.cardItems.Reverse();
            CreateNewTemplatePreView(currentTemplate);
            LoadSavedListItems(currentTemplate);
        }
        private void DeleteCardItemSort(GameObject deleteObj)
        {
            DestroyImmediate(deleteObj);
            currentTemplate.cardItems.Clear();
            if (scrollContent.childCount > 0)
            {
                foreach (Transform item in scrollContent)
                {
                    currentTemplate.cardItems.Add(item.GetComponent<ListItemEditImage>().thisData);
                }
            }
            currentTemplate.cardItems.Reverse();
            CreateNewTemplatePreView(currentTemplate);
            LoadSavedListItems(currentTemplate);
            HideConfigPanel();
        }
        private void CreateNewTemplatePreView(DataTemplate data)
        {
            LogManager.Log($"[{LogTag}] CreateNewTemplatePreView");
            if (previewParent.childCount > 0)
            {
                foreach (Transform child in previewParent)
                {
                    Destroy(child.gameObject);
                }
            }
            GameObject PreviewRootObject = Instantiate(PrefabManager.instance.PreviewRoot, previewParent);

            previewRoot = PreviewRootObject.GetComponent<RectTransform>();
            previewRoot.sizeDelta = data.size;

            Vector2 contentSize = data.size;
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

        private PreviewItem CreateNewPreViewItem(DataTemplateItem itemData, RectTransform parent)
        {
            LogManager.Log($"[{LogTag}] CreateNewPreViewItemImage");
            GameObject newItem = Instantiate(PrefabManager.instance.PreviewItem, parent);
            RectTransform newItemRect = newItem.GetComponent<RectTransform>();
            newItemRect.localPosition = itemData.position;
            PreviewItem newItemPreview = newItem.GetComponent<PreviewItem>();
            newItemPreview.Init(itemData);
            newItemRect.localScale = Vector3.one;
            return newItemPreview;
        }

        public void UpdateConfigPanelImage()
        {
            LogManager.Log($"[{LogTag}] UpdateConfigPanelImage");

            if (currentTemplate == null || currentDataItem == null)
                return;

            if (!string.IsNullOrEmpty(ConfigPanelImageName.text))//名字
                currentDataItem.name = UnityWebRequest.EscapeURL(ConfigPanelImageName.text);
            else
                ConfigPanelImageName.text = UnityWebRequest.UnEscapeURL(currentDataItem.name);

            if (intReg.IsMatch(ConfigPanelImageWidth.text))  //宽
                currentDataItem.size = new Vector2(Convert.ToInt32(ConfigPanelImageWidth.text), currentDataItem.size.y);
            else
                ConfigPanelImageWidth.text = currentDataItem.size.x.ToString();

            if (intReg.IsMatch(ConfigPanelImageHeight.text))  //高
                currentDataItem.size = new Vector2(currentDataItem.size.x, Convert.ToInt32(ConfigPanelImageHeight.text));
            else
                ConfigPanelImageHeight.text = currentDataItem.size.y.ToString();

            if (intReg.IsMatch(ConfigPanelImageX.text) || ConfigPanelImageX.text == "0")  //X
                currentDataItem.position = new Vector2(Convert.ToInt32(ConfigPanelImageX.text), currentDataItem.position.y);
            else
                ConfigPanelImageX.text = currentDataItem.position.x.ToString();

            if (intReg.IsMatch(ConfigPanelImageY.text) || ConfigPanelImageX.text == "0")  //Y
                currentDataItem.position = new Vector2(currentDataItem.position.x, Convert.ToInt32(ConfigPanelImageY.text));
            else
                ConfigPanelImageY.text = currentDataItem.position.y.ToString();

            if (!string.IsNullOrEmpty(ConfigPanelImageTexture.text))//示例图片
            {
                ((DataTemplateItemImage)currentDataItem).texturePath = ConfigPanelImageTexture.text;
            }

                for (int i=0;i< currentTemplate.cardItems.Count; i++)
            {
                if (currentTemplate.cardItems[i].index == currentDataItem.index)
                    currentTemplate.cardItems[i] = currentDataItem;
            }
            CreateNewTemplatePreView(currentTemplate);
            LoadSavedListItems(currentTemplate);
        }

        public void SaveTemplate()
        {
            SaveJsonManager.instance.SaveTemplate(currentTemplate);
        }
    }
}