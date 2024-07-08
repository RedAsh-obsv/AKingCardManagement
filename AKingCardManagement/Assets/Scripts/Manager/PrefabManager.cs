using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager instance { private set; get; }
    PrefabManager()
    {
        instance = this;
    }
    public GameObject ListItemCard;
    public GameObject ListItemTemplate;
    public GameObject PreviewItem;
    public GameObject PreviewRoot;
}
