using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGameManagementHandler : MonoBehaviour
{
    [Header("Main")]
    public List<ManagementPage> managementPages;

    [Header("Build")]
    public GameObject buildItemCanvasPrefab;
    public List<BuildItemTypesTabs> buildItemTypesTabs;

    private void Awake()
    {
        //Default
        AddPageTabListeners();

        //Build
        AddBuildTabListeners();
        SpawnBuildItems();
        buildItemTypesTabs[0].Button.onClick.Invoke();
    }
    private void OnEnable()
    {
        ChangePageTab(managementPages[0]);
    }

    //Default
    private void AddPageTabListeners()
    {
        foreach (ManagementPage page in managementPages)
        {
            if (page.Button == null || page.Page == null) continue;
            page.Button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ChangePageTab(page));
        }
    }
    private void ChangePageTab(ManagementPage page = null)
    {
        foreach (ManagementPage p in managementPages)
        {
            if (page == null) { p.Page.SetActive(false); continue; }

            if (p.Page != page.Page)
            {
                p.Page.SetActive(false);
                p.Button.GetComponent<RawImage>().color = Color.white;
            }
            else
            {
                page.Page.SetActive(!page.Page.activeSelf);
                p.Button.GetComponent<RawImage>().color = Color.green;
            }
        }


    }

    //Build
    private void AddBuildTabListeners()
    {
        foreach (BuildItemTypesTabs page in buildItemTypesTabs)
        {
            if (page.Button == null || page.Page == null) continue;
            page.Button.GetComponent<Button>().onClick.AddListener(() => ChangeBuildTab(page));
        }
    }
    private void SpawnBuildItems()
    {
        foreach (BuildItemTypesTabs page in buildItemTypesTabs)
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
    private void ChangeBuildTab(BuildItemTypesTabs page = null)
    {
        foreach (BuildItemTypesTabs p in buildItemTypesTabs)
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
}

[System.Serializable] public class ManagementPage
{
    public GameObject Button;
    public GameObject Page;
}
[System.Serializable] public class BuildItemTypesTabs
{
    public Button Button;
    public GameObject Page;
    public List<BuildItem_S> BuildItems;
}