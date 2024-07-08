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
        private CardTemplate thisData = new CardTemplate("-", Vector2.one);
        private UnityAction<CardTemplate> onClickAction;
        private Button thisButton;
        public void Init(CardTemplate data, UnityAction<CardTemplate> OnClickAction)
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