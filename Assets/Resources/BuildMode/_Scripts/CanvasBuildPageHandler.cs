using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBuildPageHandler : MonoBehaviour
{
    [Header("*** General ***")]
    public GameObject buildItemCanvasPrefab;
    public List<BuildItemTypesTabs> buildItemTypesPages;

    

    private void Awake()
    {
        AddButtonListeners();
        CreateItems();

        buildItemTypesPages[0].Button.onClick.Invoke();
    }
    private void ChangePage(BuildItemTypesTabs page = null)
    {
        foreach (BuildItemTypesTabs p in buildItemTypesPages)
        {
            if (page == null) { p.Page.SetActive(false); continue; }

            if (p.Page != page.Page)
            {
                p.Page.SetActive(false);
                p.Button.GetComponent<RawImage>().color = Color.white;
            }
            else
            {
                page.Page.SetActive(true);
                p.Button.GetComponent<RawImage>().color = Color.green;
            }
        }


    }
    private void AddButtonListeners()
    {
        foreach (BuildItemTypesTabs page in buildItemTypesPages)
        {
            if (page.Button == null || page.Page == null) continue;
            page.Button.GetComponent<Button>().onClick.AddListener(() => ChangePage(page));
        }
    }
    private void CreateItems()
    {
        foreach (BuildItemTypesTabs page in buildItemTypesPages)
        {
            if (page.BuildItems == null || page.BuildItems.Count == 0) continue;

            foreach (BuildItem_S item in page.BuildItems)
            {
                GameObject itemObj = Instantiate(buildItemCanvasPrefab, page.Page.transform);
                itemObj.GetComponent<CanvasBuildItemHandler>().SetItem(item);

                itemObj.GetComponent<Button>().onClick.AddListener(() => BuildModeHandler.Instance.SetActiveBuildItem(item));

            }


        }
    }
}


