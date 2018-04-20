using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class GUIManager : MonoBehaviour {

    public static GUIManager GUI;

    private void Awake()
    {
        GUI = this;
    }
    Image _activeToolbar;

    public Image activeToolbar
    {
        get { return _activeToolbar; }

        set
        {
            if (_activeToolbar != null) activeToolbar.color = Color.white;
            _activeToolbar = value;
            _activeToolbar.color = Color.black;
        }
    }
}
