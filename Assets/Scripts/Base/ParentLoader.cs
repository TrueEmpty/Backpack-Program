using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ParentLoader : MonoBehaviour
{
    Database db;

    public InputField searchBar;

    public GameObject content;
    public GameObject LoadParentPF;

    public GameObject ParentLoadForm;
    public GameObject ChildAddForm;


    List<GameObject> parentObj = new List<GameObject>();

    public float loadRate = 20;
    float nextLoad = 0;
    bool lpr = false;

    // Start is called before the first frame update
    void Start()
    {
        db = Database.instance;
    }

    // Update is called once per frame
    void Update()
    {

        if(SearchSelected())
        {
            ParentLoadForm.SetActive(true);
            ChildAddForm.SetActive(false);

            if(lpr)
            {
                LoadParentsInfo();
                lpr = false;
            }
        }
        else
        {
            if(!IsInvoking("Deactivate"))
            {
                Invoke("Deactivate", .15f);
            }
        }

    }

    public void LoadParentsInfo()
    {
        ClearParentObj();

        List<Parents> AllFoundParents = db.GetAllParents(searchBar.text);

        if(AllFoundParents.Count > 0)
        {
            for(int i = 0; i < AllFoundParents.Count; i++)
            {
                GameObject go = Instantiate(LoadParentPF, content.transform);

                go.GetComponent<LoadParent>().parent = AllFoundParents[i];

                parentObj.Add(go);
            }
        }

        nextLoad = Time.time + loadRate;
    }

    public void ClearParentObj()
    {
        if(parentObj.Count > 0)
        {
            for(int i = parentObj.Count - 1; i >= 0; i--)
            {
                Destroy(parentObj[i]);
            }

            parentObj.Clear();
        }
    }

    void Deactivate()
    {
        lpr = true;
        ParentLoadForm.SetActive(false);
        ChildAddForm.SetActive(true);
    }

    bool SearchSelected()
    {
        bool result = false;

        if (EventSystem.current.currentSelectedGameObject == searchBar.gameObject)
        {
            result = true;
        }

        Window wi = ParentLoadForm.GetComponent<Window>();

        if (wi != null)
        {
            if(wi.MouseClickInsideWindow())
            {
                result = true;
            }
        }

        return result;
    }
}
