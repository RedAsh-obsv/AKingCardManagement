using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AKingCard
{
    [Serializable]
    public class DataCardItem
    {
        public string index;
        public string name;
        public Vector2 size = Vector2.one;
        public Vector2 position = Vector2.zero;
        public string content;
        public bool Equals(DataCardItem otherTemplate)
        {
            if (otherTemplate.index.Equals(index)) return false;
            if (otherTemplate.name.Equals(name)) return false;
            if (otherTemplate.size.Equals(size)) return false;
            if (otherTemplate.position.Equals(position)) return false;
            return true;
        }
    }
}

