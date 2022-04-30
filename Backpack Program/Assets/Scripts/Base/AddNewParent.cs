using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddNewParent : MonoBehaviour
{
    Database db;
    ParentInfo pI;

    [SerializeField]
    Parents newParent = null;

    public InputField firstName;
    public InputField lastName;

    [Space()]
    public InputField address;
    public InputField city;
    public InputField contact;

    [Space()]
    public Dropdown pickup;

    [Space()]
    public Text message;

    public AddNewChild anc = null;

    // Start is called before the first frame update
    void Start()
    {
        db =Database.instance;
        pI = ParentInfo.instance;

        newParent = new Parents();

        pickup.options = new List<Dropdown.OptionData>();

        if(db.pickuptimes.Count > 0)
        {
            for (int i = 0; i < db.pickuptimes.Count; i++)
            {
                pickup.options.Add(new Dropdown.OptionData(db.pickuptimes[i].Times));
            }

            pickup.value = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInfo();
        CheckValidity();
    }

    void CheckValidity()
    {
        string mesText = "";

        if (CheckIfParentExist())
        {
            //Show that the Parent already exist
            mesText = "This Parent already Exist";
        }

        if (newParent.FirstName.Trim() == "" && newParent.LastName.Trim() == "")
        {
            if (mesText != "")
            {
                mesText += ", ";
            }

            //Show that the Name is null
            mesText += "Parent's Name is null";
        }

        if (newParent.Address.Trim() == "")
        {
            if (mesText != "")
            {
                mesText += ", ";
            }

            //Show that the Address is null
            mesText += "Parent's Address is null";
        }

        if (newParent.City.Trim() == "")
        {
            if (mesText != "")
            {
                mesText += ", ";
            }

            //Show that the City is null
            mesText += "Parent's City is null";
        }

        if (newParent.Contact.Trim() == "")
        {
            if (mesText != "")
            {
                mesText += ", ";
            }

            //Show that the Contact is null
            mesText += "Parent's Contact is null";
        }

        if (newParent.PickupTime.Trim() == "")
        {
            if (mesText != "")
            {
                mesText += ", ";
            }

            //Show that the PickupTime is null
            mesText += "Parent's PickupTime is null";
        }

        message.text = mesText;
    }

    public void AddParent()
    {
        //Check if Parent already exist
        if (!CheckIfParentExist() && newParent.FirstName.Trim() != "" && newParent.LastName.Trim() != "")
        {
            //Add New Parent
            db.AddNew(newParent);
            pI.SelectParent(newParent);
        }
    }

    void UpdateInfo()
    {
        if (firstName != null)
        {
            newParent.FirstName = firstName.text.Trim();
        }

        if (lastName != null)
        {
            newParent.LastName = lastName.text.Trim();
        }

        if (address != null)
        {
            newParent.Address = address.text.Trim();
        }

        if (city != null)
        {
            newParent.City = city.text.Trim();
        }

        if (contact != null)
        {
            newParent.Contact = contact.text.Trim();
        }

        if (pickup != null && db.pickuptimes.Count > 0)
        {
            if(pickup.value >= 0)
            {
                newParent.PickupTime = pickup.options[pickup.value].text;
            }
        }

        newParent.Computer = db.computer.text;
    }

    public bool CheckIfParentExist()
    {
        bool result = db.parents.Exists(x => x.FirstName.Trim().ToUpper() == newParent.FirstName.Trim().ToUpper() && x.LastName.Trim().ToUpper() == newParent.LastName.Trim().ToUpper() && x.Address.Trim().ToUpper() == newParent.Address.Trim().ToUpper() && x.City.Trim().ToUpper() == newParent.City.Trim().ToUpper());

        return result;
    }
}
