using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddNewSchool : MonoBehaviour
{
    Database db;
    [SerializeField]
    Schools newSchool = null;

    public InputField schoolName;

    [Space()]
    public Toggle g_preK;
    public Toggle g_K;
    public Toggle g_1st;
    public Toggle g_2nd;
    public Toggle g_3rd;
    public Toggle g_4th;
    public Toggle g_5th;
    public Toggle g_6th;
    public Toggle g_7th;
    public Toggle g_8th;
    public Toggle g_9th;
    public Toggle g_10th;
    public Toggle g_11th;
    public Toggle g_12th;

    [Space()]
    public Text message;

    // Start is called before the first frame update
    void Start()
    {
        db = GameObject.FindGameObjectWithTag("Manager").GetComponent<Database>();

        newSchool = new Schools();
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

        if (CheckIfSchoolExist())
        {
            //Show that the School Name already exist
            mesText = "This School Name already Exist";
        }

        if (newSchool.SchoolName.Trim() == "")
        {
            if (mesText != "")
            {
                mesText += ", ";
            }

            //Show that the School Name is null
            mesText += "School Name is null";
        }

        if (newSchool.Grades.Count <= 0)
        {
            if (mesText != "")
            {
                mesText += ", ";
            }

            //Show that the School Name is null
            mesText += "No Grades Selected";
        }

        message.text = mesText;
    }

    public void AddSchool()
    {
        //Check if School Name already exist
        if(!CheckIfSchoolExist() && newSchool.SchoolName.Trim() != "" && newSchool.Grades.Count > 0)
        {
            //Add New School
            db.AddNew(newSchool);
        }
    }

    void UpdateInfo()
    {
        if(schoolName != null)
        {
            newSchool.SchoolName = schoolName.text.Trim();
        }

        newSchool.Grades = GetAListOfGrades();

        newSchool.Computer = db.computer.text;
    }

    public bool CheckIfSchoolExist()
    {
        bool result = db.schools.Exists(x => x.SchoolName.Trim().ToUpper() == newSchool.SchoolName.Trim().ToUpper());

        return result;
    }

    public List<string> GetAListOfGrades()
    {
        List<string> result = new List<string>();

        if (g_preK != null)
        {
            if(g_preK.isOn)
            {
                result.Add("Pre-K");
            }
        }

        if (g_K != null)
        {
            if (g_K.isOn)
            {
                result.Add("K");
            }
        }

        if (g_1st != null)
        {
            if (g_1st.isOn)
            {
                result.Add("1st");
            }
        }

        if (g_2nd != null)
        {
            if (g_2nd.isOn)
            {
                result.Add("2nd");
            }
        }

        if (g_3rd != null)
        {
            if (g_3rd.isOn)
            {
                result.Add("3rd");
            }
        }

        if (g_4th != null)
        {
            if (g_4th.isOn)
            {
                result.Add("4th");
            }
        }

        if (g_5th != null)
        {
            if (g_5th.isOn)
            {
                result.Add("5th");
            }
        }

        if (g_6th != null)
        {
            if (g_6th.isOn)
            {
                result.Add("6th");
            }
        }

        if (g_7th != null)
        {
            if (g_7th.isOn)
            {
                result.Add("7th");
            }
        }

        if (g_8th != null)
        {
            if (g_8th.isOn)
            {
                result.Add("8th");
            }
        }

        if (g_9th != null)
        {
            if (g_9th.isOn)
            {
                result.Add("9th");
            }
        }

        if (g_10th != null)
        {
            if (g_10th.isOn)
            {
                result.Add("10th");
            }
        }

        if (g_11th != null)
        {
            if (g_11th.isOn)
            {
                result.Add("11th");
            }
        }

        if (g_12th != null)
        {
            if (g_12th.isOn)
            {
                result.Add("12th");
            }
        }

        return result;
    }
}
