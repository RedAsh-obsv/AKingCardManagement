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
        public TextMeshProUGUI textName;
        private DataTemplate thisData = new DataTemplate("-", Vector2.one);
        private UnityAction<DataTemplate> onClickAction;
        private Button thisButton;
        public void Init(DataTemplate data, UnityAction<DataTemplate> OnClickAction)
        {
            thisData = data;
            onClickAction = OnClickAction;
            textName.text = UnityWebRequest.UnEscapeURL(data.name);
            thisButton = GetComponent<Button>();
            thisButton.onClick.AddListener(OnClick);
        }
        private void OnClick()
        {
            onClickAction?.Invoke(thisData);
        }
    }
}