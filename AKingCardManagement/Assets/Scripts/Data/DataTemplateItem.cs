using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AKingCard
{
    public enum DataTemplateItemType
    {
        Image = 1,
        Text = 2,
    }
    public class DataTemplateItem
    {
        public string index;
        public string name;
        public Vector2 size;
        public Vector2 position;
        public DataTemplateItemType type;
    }
}

