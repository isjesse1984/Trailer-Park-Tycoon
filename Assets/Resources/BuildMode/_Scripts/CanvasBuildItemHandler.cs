using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBuildItemHandler : MonoBehaviour
{
    [SerializeField] private BuildItem_S item;

    public BuildItem_S GetItem => item;
    public void SetItem(BuildItem_S _item)
    {
        if (item == null) return;

        item = _item;

        GameObject iconChild = transform.Find("Icon").gameObject;
        if( iconChild != null ) iconChild.GetComponent<RawImage>().texture = item.icon;

        GameObject nameChild = transform.Find("Name").gameObject;
        if( nameChild != null ) nameChild.GetComponent<TextMeshProUGUI>().text = item.itemName;
        
        GameObject costChild = transform.Find("Cost").gameObject;
        if (costChild != null) costChild.GetComponent<TextMeshProUGUI>().text = $" ${item.cost.ToString("F2")}";
    }
}
