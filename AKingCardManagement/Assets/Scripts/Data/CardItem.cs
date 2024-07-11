using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AKingCard
{
    public class CardItem
    {
        public DataTemplate template;
        public long index;
        public string name;
        public Vector2 size = Vector2.one;
        public Vector2 position = Vector2.zero;
        public string content;
        public bool Equals(DataTemplate template, CardItem otherTemplate)
        {
            if (otherTemplate.template.index != template.index) return false;
            if (otherTemplate.index != index) return false;
            if (otherTemplate.name != name) return false;
            if (otherTemplate.size != size) return false;
            if (otherTemplate.position != position) return false;
            return true;
        }
    }
}

