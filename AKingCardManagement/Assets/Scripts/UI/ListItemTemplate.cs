using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace AKingCard
{
    public class ListItemTemplate : MonoBehaviour
    {
        private const string LogTag = "ListItemTemplate";
        public TextMeshProUGUI textName;
        public Button buttonDelete;
        [HideInInspector]
        public DataTemplate thisData = new DataTemplate("-", Vector2.one);
        private UnityAction<DataTemplate> onClickAction;
        private Button thisButton;
        private UnityAction<GameObject> deleteAction;
        public void Init(DataTemplate data, UnityAction<DataTemplate> OnClickAction, UnityAction<GameObject> DeleteAction)
        {
            thisData = data;
            onClickAction = OnClickAction;
            deleteAction = DeleteAction;
            textName.text = UnityWebRequest.UnEscapeURL(data.name);
            thisButton = GetComponent<Button>();
            thisButton.onClick.AddListener(OnClick);
            buttonDelete.onClick.AddListener(OnClickButtonDelete);
        }
        private void OnClick()
        {
            onClickAction?.Invoke(thisData);
        }
        private void OnClickButtonDelete()
        {
            LogManager.Log($"[{LogTag}] OnClickButtonDelete");
            deleteAction?.Invoke(this.gameObject);
        }
    }
}