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
    public class PageTemplateEdit : MonoBehaviour
    {
        private const string LogTag = "PageTemplateEdit";
        public PageManager pageManager;
        public Transform alertBack;
        public RectTransform previewParent;
        public TextMeshProUGUI previewTextTitle;
        public TextMeshProUGUI previewTextSize;
        public GameObject ConfigPanel;

        private DataTemplate currentTemplate;
        private bool changeHasSaved = false;
        private DateTime changeTimer;
        void Start()
        {
            Debug.Log($"[{LogTag}] Start");
            alertBack.gameObject.SetActive(false);
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
            previewTextTitle.text = UnityWebRequest.UnEscapeURL(data.name);
            previewTextSize.text = $"{data.size.x} x {data.size.y}";
            ConfigPanel.SetActive(false);
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
            GameObject PreviewRoot = Instantiate(PrefabManager.instance.PreviewRoot, previewParent);

            RectTransform PreviewRootRect = PreviewRoot.GetComponent<RectTransform>();
            PreviewRootRect.sizeDelta = data.cardItems[0].size;

            for (int i = 0; i < data.cardItems.Count; i++)
            {
                GameObject newItem = Instantiate(PrefabManager.instance.PreviewItem, PreviewRootRect);
                RectTransform newItemRect = newItem.GetComponent<RectTransform>();
                newItemRect.localPosition = data.cardItems[i].position;
                PreviewItem newItemPreview = newItem.GetComponent<PreviewItem>();
                newItemPreview.Init(data.cardItems[i].size, data.cardItems[i].position);
            }

            Vector2 contentSize = data.cardItems[0].size;
            Vector2 containerSize = previewParent.sizeDelta;
            if (contentSize.x / contentSize.y > containerSize.x / containerSize.y)  //¸ü¿í
            {
                Vector2 targetSize = Vector2.one;
                targetSize.x = containerSize.x / contentSize.x;
                targetSize.y = (contentSize.y * (containerSize.x / contentSize.x)) / contentSize.y;
                PreviewRootRect.localScale = targetSize;
            }
            else //¸ü¸ß
            {
                Vector2 targetSize = Vector2.one;
                targetSize.y = containerSize.y / contentSize.y;
                targetSize.x = (contentSize.x * (containerSize.y / contentSize.y)) / contentSize.x;
                PreviewRootRect.localScale = targetSize;
            }
        }
    }
}