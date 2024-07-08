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
        public PageManager pageManager;
        public Transform alertBack;
        public RectTransform PreviewParent;
        public GameObject PrefabPreViewItem;

        public bool changeHasSaved = false;
        void Start()
        {
            alertBack.gameObject.SetActive(false);
        }

        void Update()
        {
        }
        public void ShowAlertBack()
        {
            alertBack.gameObject.SetActive(true);
        }
        public void HideAlertBack()
        {
            alertBack.gameObject.SetActive(false);
        }
        public void CheckBack()
        {
            if (changeHasSaved)
                pageManager.OpenTemplateManagePage();
            else
                ShowAlertBack();
        }
        public void SaveChange()
        {

        }
        private void CreateNewTemplatePreView(CardTemplate data)
        {
            if (PreviewParent.childCount != 0)
            {
                foreach (Transform child in PreviewParent)
                {
                    Destroy(child.gameObject);
                }
            }
            GameObject Background = Instantiate(PrefabPreViewItem, PreviewParent);
            Background.AddComponent<Mask>();
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
            if (contentSize.x / contentSize.y > containerSize.x / containerSize.y)  //¸ü¿í
            {
                Vector2 targetSize = Vector2.one;
                targetSize.x = containerSize.x / contentSize.x;
                targetSize.y = (contentSize.y * (containerSize.x / contentSize.x)) / contentSize.y;
                BackgroundRect.localScale = targetSize;
            }
            else //¸ü¸ß
            {
                Vector2 targetSize = Vector2.one;
                targetSize.y = containerSize.y / contentSize.y;
                targetSize.x = (contentSize.x * (containerSize.y / contentSize.y)) / contentSize.x;
                BackgroundRect.localScale = targetSize;
            }
        }
    }
}