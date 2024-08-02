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
    public class DataTemplateItemText : DataTemplateItem
    {
        public string textContent;
        public int textSize;
        public Color textColor;
        public DataTemplateItemAlign align;
        public string fontPath;
        public DataTemplateItemText(string index, string name, Vector2 size, Vector2 position)
        {
            this.index = index;
            this.name = name;
            this.size = size;
            this.position = position;
            this.type = DataTemplateItemType.Text;
            this.textSize = 25;
            this.textColor = new Color(0,0,0,1);
            this.align = DataTemplateItemAlign.Left;
            this.fontPath =$"{Application.streamingAssetsPath}\\Fonts\\黑体-常规.ttf";

        }
        public bool Equals(DataTemplateItemText otherTemplate)
        {
            if (!otherTemplate.index.Equals(index)) return false;
            if (!otherTemplate.name.Equals(name)) return false;
            if (!otherTemplate.size.Equals(size)) return false;
            if (!otherTemplate.position.Equals(position)) return false;
            if (!otherTemplate.textContent.Equals(textContent)) return false;
            if (!otherTemplate.textSize.Equals(textSize)) return false;
            if (!otherTemplate.textColor.Equals(textColor)) return false;
            if (!otherTemplate.align.Equals(align)) return false;
            return true;
        }
        public static DataTemplateItemText New()
        {
            return new DataTemplateItemText(DateTime.Now.Ticks.ToString() + "000", "", Vector2.one, Vector2.zero);
        }
    }
}

