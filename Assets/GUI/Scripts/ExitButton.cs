using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class ExitButton : MonoBehaviour {

    [SerializeField]
    Transform transformToDisable;
    Button exitButton;

	void Start ()
    {
        if (transformToDisable == null) transformToDisable = transform.parent;
        exitButton = GetComponent<Button>();
        exitButton.onClick.AddListener(exitClick);
    }

    void exitClick()
    {
        transformToDisable.gameObject.SetActive(!transformToDisable.gameObject.activeSelf);
    }
	
	
}
