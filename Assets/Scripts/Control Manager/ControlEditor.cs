using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(Control))]
public class ControlEditor //: Editor
{
    /*bool showControl = false;

    public override void OnInspectorGUI()
    {
        Control mS = (Control)target;

        #region Used Script Display

        GUI.enabled = false;
        MonoScript script = MonoScript.FromMonoBehaviour((MonoBehaviour)mS);
        script = EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false) as MonoScript;
        GUI.enabled = true;

        #endregion

        //DrawDefaultInspector();

        object ct = GetControlType(mS);

        var style = new GUIStyle(GUI.skin.label);
        style.fontStyle = FontStyle.Bold;
        style.fontSize = 13;
        style.normal.textColor = Color.Lerp(Color.black, Color.cyan, .6f);
        style.alignment = TextAnchor.MiddleCenter;

        //EditorGUILayout.LabelField(ct.ToString().Substring(0, ct.ToString().IndexOf('(')), style);
        EditorGUILayout.LabelField(ct.ToString(), style);

        EditorGUILayout.Space();

        if(mS.controlled_by_window == null)
        {
            mS.group = EditorGUILayout.TextField("Group", mS.group);
            mS.order = EditorGUILayout.IntField("Order",mS.order);
        }
        else
        {
            if(mS.controlled_by_window.auto_group)
            {
                EditorGUILayout.LabelField("Group", mS.group);
            }
            else
            {
                mS.group = EditorGUILayout.TextField("Group", mS.group);
            }

            mS.order = EditorGUILayout.IntField("Order", mS.order);
        }

        mS.skip = EditorGUILayout.Toggle("Skip",mS.skip);
        mS.active = EditorGUILayout.Toggle("Active", mS.active);

        EditorGUILayout.Space();

        /* #region Show Control Type
         showControl = EditorGUILayout.Foldout(showControl, ct.ToString().Substring(0, ct.ToString().IndexOf('(')));

         if (showControl)
         {
             EditorGUILayout.Space();

             DisplayControlType((Object)ct);
         }
         #endregion*/
/*
        EditorGUILayout.Space();
    }

    object GetControlType(Control mS)
    {
        object result = null;

        switch (mS.control_Type)
        {
            case Control.ControlType.Text:
                result = mS.text_Script;
                break;
            case Control.ControlType.Image:
                result = mS.image_Script;
                break;
            case Control.ControlType.RawImage:
                result = mS.rawImage_Script;
                break;
            case Control.ControlType.Button:
                result = mS.button_Script;
                break;
            case Control.ControlType.Toggle:
                result = mS.toggle_Script;
                break;
            case Control.ControlType.InputField:
                result = mS.inputField_Script;
                break;
            case Control.ControlType.Dropdown:
                result = mS.dropdown_Script;
                break;
            case Control.ControlType.Window:
                result = mS.Window_Script;
                break;
        }

        return result;
    }

    void DisplayControlType(Object cS)
    {
        Editor e = Editor.CreateEditor(cS, null);
        e.DrawDefaultInspector();
    }*/
}
