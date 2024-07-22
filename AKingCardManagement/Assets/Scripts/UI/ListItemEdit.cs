using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace AKingCard
{
    public class ListItemEdit : MonoBehaviour
    {
        private const string LogTag = "ListItemEdit";
        public TextMeshProUGUI textName;
        public Button buttonUp;
        public Button buttonDown;
        public Button buttonDelete;
        public DataCardItem thisData;
        private UnityAction<Transform, DataCardItem> onClickAction;
        private UnityAction setSortAction;
        private Button thisButton;
        public void Init(DataCardItem data, UnityAction SetSortAction, UnityAction<Transform, DataCardItem> OnClickAction)
        {
            thisData = data;
            onClickAction = OnClickAction;
            setSortAction = SetSortAction;
            textName.text = UnityWebRequest.UnEscapeURL(data.name);
            thisButton = GetComponent<Button>();
            thisButton.onClick.AddListener(OnClick);
            buttonUp.onClick.AddListener(OnClickButtonUp);
            buttonDown.onClick.AddListener(OnClickButtonDown);
            buttonDelete.onClick.AddListener(OnClickButtonDelete);
        }
        private void OnClick()
        {
            LogManager.Log($"[{LogTag}] OnClick");
            onClickAction?.Invoke(this.transform, thisData);
        }
        private void OnClickButtonUp()
        {
            LogManager.Log($"[{LogTag}] OnClickButtonUp");
            int thisSiblingIndex = transform.GetSiblingIndex();
            if (thisSiblingIndex > 0)
                transform.SetSiblingIndex(thisSiblingIndex - 1);
            setSortAction.Invoke();
        }
        private void OnClickButtonDown()
        {
            LogManager.Log($"[{LogTag}] OnClick");
            int thisSiblingIndex = transform.GetSiblingIndex();
            int parentsChild = transform.parent.childCount;
            if (thisSiblingIndex < parentsChild - 1)
                transform.SetSiblingIndex(thisSiblingIndex + 1);
            setSortAction.Invoke();
        }
        private void OnClickButtonDelete()
        {
            LogManager.Log($"[{LogTag}] OnClickButtonDown");
        }
    }

}