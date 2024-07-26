using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AKingCard
{
    public enum DataTemplateItemAlign
    {
        Left = 1,
        Center = 2,
        Right = 3,
    }
    [Serializable]
    public class DataTemplateItem
    {
        public string index;
        public DataTemplateItemAlign align = DataTemplateItemAlign.Left;
        public string name;
        public Vector2 size = Vector2.one;
        public Vector2 position = Vector2.zero;
        public string texturePath;
        public string textContent;
        public int textSize = 25;
        public Color textColor = Color.black;
        public DataTemplateItem(string index, string name, Vector2 size, Vector2 position)
        {
            this.index = index;
            this.name = name;
            this.size = size;
            this.position = position;
        }
        public bool Equals(DataTemplateItem otherTemplate)
        {
            if (!otherTemplate.index.Equals(index)) return false;
            if (!otherTemplate.align.Equals(align)) return false;
            if (!otherTemplate.name.Equals(name)) return false;
            if (!otherTemplate.size.Equals(size)) return false;
            if (!otherTemplate.position.Equals(position)) return false;
            if (!otherTemplate.texturePath.Equals(texturePath)) return false;
            if (!otherTemplate.textContent.Equals(textContent)) return false;
            if (!otherTemplate.textSize.Equals(textSize)) return false;
            if (!otherTemplate.textColor.Equals(textColor)) return false;
            return true;
        }
    }
}

