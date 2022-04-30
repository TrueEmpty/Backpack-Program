using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadParent : MonoBehaviour
{
    Database db;

    [SerializeField]
    public Parents parent;

    public Text fullName;
    public Text ChildrenCount;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(db == null)
        {
            db = Database.instance;
        }

        if(parent != null)
        {
            fullName.text = parent.GetFullName();

            List<Children> cl = db.GetParentsChildren(parent.UniqueId);
            ChildrenCount.text = cl.Count + " Children";
        }
    }

    public void LoadParentInfo()
    {
        if(parent != null)
        {
            ParentInfo.instance.SelectParent(parent);
        }
    }
}
