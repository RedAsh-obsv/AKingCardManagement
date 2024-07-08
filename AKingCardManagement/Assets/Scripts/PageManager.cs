using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AKingCard
{
    enum PageType
    {
        StartPage,
        TemplateManagePage,
        TemplateEditPage,
        CardManagePage,
        CardEditPage,
    }
    public class PageManager : MonoBehaviour
    {
        public Transform pageStart;
        public Transform pageTemplateManage;
        public Transform pageCardManage;
        public Transform pageTemplateEdit;
        public Transform pageCardEdit;
        private Dictionary<PageType,Transform> pages = new Dictionary<PageType, Transform>();

        void Start()
        {
            pages.Add(PageType.StartPage, pageStart);
            pages.Add(PageType.TemplateManagePage, pageTemplateManage);
            pages.Add(PageType.CardManagePage, pageCardManage);
            pages.Add(PageType.TemplateEditPage, pageTemplateEdit);
            pages.Add(PageType.CardEditPage, pageCardEdit);
            foreach(var page in pages)
            {
                page.Value.gameObject.SetActive(true);
            }
            OpenStartPage();
        }

        void Update() { }

        public void OpenStartPage()
        {
            OnSwitchPage(PageType.StartPage);
        }
        public void OpenTemplateManagePage()
        {
            OnSwitchPage(PageType.TemplateManagePage);
        }
        public void OpenCardManagePage()
        {
            OnSwitchPage(PageType.CardManagePage);
        }
        public void OpenTemplateEditPage()
        {
            OnSwitchPage(PageType.TemplateEditPage);
        }
        public void OpenCardEditPage()
        {
            OnSwitchPage(PageType.CardEditPage);
        }
        private void OnSwitchPage(PageType pageType)
        {
            foreach (var page in pages)
            {
                if (page.Key == pageType)
                    SetVisable(true, page.Value);
                else
                    SetVisable(false, page.Value);
            }
        }

        public void SetVisable(bool visiable, Transform transform)
        {
            float scale = visiable ? 1 : 0.001f;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}