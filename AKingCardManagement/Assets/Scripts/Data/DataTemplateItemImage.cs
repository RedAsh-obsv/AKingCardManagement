using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AKingCard
{
    [Serializable]
    public class DataTemplateItemImage : DataTemplateItem
    {
        public string texturePath;
        public DataTemplateItemImage(string index, string name, Vector2 size, Vector2 position)
        {
            this.index = index;
            this.name = name;
            this.size = size;
            this.position = position;
            this.type = DataTemplateItemType.Image;
        }
        public bool Equals(DataTemplateItemImage otherTemplate)
        {
            if (!otherTemplate.index.Equals(index)) return false;
            if (!otherTemplate.name.Equals(name)) return false;
            if (!otherTemplate.size.Equals(size)) return false;
            if (!otherTemplate.position.Equals(position)) return false;
            if (!otherTemplate.texturePath.Equals(texturePath)) return false;
            return true;
        }
        public static DataTemplateItemImage New()
        {
            return new DataTemplateItemImage(DateTime.Now.Ticks.ToString() + "000", "", Vector2.one, Vector2.zero);
        }
    }
}

