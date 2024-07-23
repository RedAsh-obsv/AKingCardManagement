using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AKingCard
{
    [Serializable]
    public class DataTemplate
    {
        public long index;
        public string name;
        public Vector2 size;
        public List<DataCardItem> cardItems;
        public DataTemplate(string name, Vector2 size)
        {
            this.index = DateTime.Now.Ticks;
            this.name = name;
            this.size = size;
            cardItems = new List<DataCardItem>();
            cardItems.Add(new CardItemImage(0, "Background", size, Vector2.zero));
        }
        public string toString()
        {
            return $"index:{this.index}, name:{this.name}, size:{size}, cardItems:{cardItems.Count}";
        }
        public bool Equals(DataTemplate otherTemplate)
        {
            if (otherTemplate == null) return false;
            if (otherTemplate.index != index) return false;
            if (otherTemplate.name != name) return false;
            if (otherTemplate.size != size) return false;
            if (otherTemplate.index != index) return false;
            if (otherTemplate.cardItems.Count != cardItems.Count) return false;
            else
            {
                for (int i = 0; i < otherTemplate.cardItems.Count; i++)
                {
                    if(!otherTemplate.cardItems[i].Equals(cardItems[i].index)) return false;
                }
            }
            return true;
        }
    }
}