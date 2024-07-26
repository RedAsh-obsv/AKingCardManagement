using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AKingCard
{
    [Serializable]
    public class DataCard
    {
        public string index;
        public string name;
        public DataTemplate thisTemplate;
        public List<DataTemplateItem> items;
        public DataCard(string name, DataTemplate template)
        {
            this.index = template + DateTime.Now.Ticks.ToString();
            this.name = name;
            thisTemplate = template;
        }
        public string toString()
        {
            return $"index:{this.index}, name:{this.name}, items:{items.Count}";
        }
        public bool Equals(DataCard otherTemplate)
        {
            if (otherTemplate == null) return false;
            if (!otherTemplate.index.Equals(index)) return false;
            if (!otherTemplate.name.Equals(name)) return false;
            if (!otherTemplate.thisTemplate.Equals(thisTemplate)) return false;
            if (!otherTemplate.items.Count.Equals(items.Count)) return false;
            else
            {
                for (int i = 0; i < otherTemplate.items.Count; i++)
                {
                    if (!otherTemplate.items[i].Equals(items[i].index)) return false;
                }
            }
            return true;
        }
    }
}