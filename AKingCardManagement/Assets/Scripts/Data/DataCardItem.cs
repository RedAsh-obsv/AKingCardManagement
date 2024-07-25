using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AKingCard
{
    public enum DataCardItemType
    {
        Image = 1,
        Text = 2,
    }
    public enum DataCardItemAlign
    {
        Left = 1,
        Center = 2,
        Right = 3,
    }
    [Serializable]
    public class DataCardItem
    {
        public string index;
        public DataCardItemType type;
        public DataCardItemAlign align = DataCardItemAlign.Left;
        public string name;
        public Vector2 size = Vector2.one;
        public Vector2 position = Vector2.zero;
        public string texturePath;
        public string textContent;
        public int textSize = 25;
        public Color textColor = Color.black;
        public DataCardItem(string index, DataCardItemType type, string name, Vector2 size, Vector2 position)
        {
            this.index = index;
            this.type = type;
            this.name = name;
            this.size = size;
            this.position = position;
        }
        public bool Equals(DataCardItem otherTemplate)
        {
            if (otherTemplate.index.Equals(index)) return false;
            if (otherTemplate.type.Equals(type)) return false;
            if (otherTemplate.align.Equals(align)) return false;
            if (otherTemplate.name.Equals(name)) return false;
            if (otherTemplate.size.Equals(size)) return false;
            if (otherTemplate.position.Equals(position)) return false;
            if (otherTemplate.texturePath.Equals(texturePath)) return false;
            if (otherTemplate.textContent.Equals(textContent)) return false;
            if (otherTemplate.textSize.Equals(textSize)) return false;
            if (otherTemplate.textColor.Equals(textColor)) return false;
            return true;
        }
    }
}

