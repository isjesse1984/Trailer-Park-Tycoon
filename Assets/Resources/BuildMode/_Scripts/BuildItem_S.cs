using UnityEngine;

public class BuildItem_S : ScriptableObject
{
    public string itemName;
    public float cost;
    public Texture icon;
    public BuildItemType itemType;
}
[SerializeField]
public enum BuildItemType
{
    Road,
    TentSite,
    TrailerSite,
    CabinSite
}
