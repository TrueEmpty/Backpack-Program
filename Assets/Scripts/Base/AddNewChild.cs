using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddNewChild : MonoBehaviour
{
    Database db;
    ParentInfo pI;

    public WindowLoad wl;

    #region Children Info
    public GameObject childInfoResource;
    public GameObject newChildInfoResource;
    public GameObject childListContainer;

    public string lastParentUID;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        db = Database.instance;
        pI = ParentInfo.instance;
    }

    // Update is called once per frame
    void Update()
    { 
        if(pI.parent.UniqueId != "" && pI.parent.UniqueId != null && pI.parent.UniqueId != lastParentUID)
        {
            UpdateChildList();
            lastParentUID = pI.parent.UniqueId;
        }
    }

    public void AddNewParent()
    {
        wl.OpenWindow("addparent");
    }

    public void ResetChildList()
    {
        int cC = childListContainer.transform.childCount;

        if (cC > 0)
        {
            for(int i = cC - 1; i >= 0; i--)
            {
                Destroy(childListContainer.transform.GetChild(i));
            }
        }

        UpdateChildList();
    }

    void UpdateChildList()
    {
        //Get Parent's Children
        List<Children> parentsChildren = db.children.FindAll(x => x.ParentUID == pI.parent.UniqueId);

        if (parentsChildren.Count >= 0 && (pI.parent.UniqueId != "" && pI.parent.UniqueId != null))
        {
            for(int i = 0; i <= parentsChildren.Count; i++)
            {
                if(i < parentsChildren.Count)
                {
                    //Create Child Info
                    GameObject go = Instantiate(childInfoResource, new Vector3(0, 60 * i, 0), Quaternion.identity, childListContainer.transform) as GameObject;

                    ChildInfo cI = go.GetComponent<ChildInfo>();

                    if (cI != null)
                    {
                        cI.SetChildInfo(parentsChildren[i]);
                    }
                }
                else
                {
                    //Create New Child Info
                    GameObject ngo = Instantiate(newChildInfoResource, new Vector3(0, 60 * i, 0), Quaternion.identity, childListContainer.transform) as GameObject;

                    NewChildInfo ncI = ngo.GetComponent<NewChildInfo>();

                    if (ncI != null)
                    {
                        ncI.child.ParentUID = pI.parent.UniqueId;
                        ncI.anc = this;
                    }
                }
            }
        }
    }
}
