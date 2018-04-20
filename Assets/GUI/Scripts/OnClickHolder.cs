using UnityEngine;
using UnityEngine.UI;


public abstract class OnClickHolder<T> : MonoBehaviour
{
    public T holdedObject;
    protected Button button;

    protected abstract void OnClickEvent();

    protected virtual void Start()
    {
        button = GetComponentInChildren<Button>();

        if(button == null)      
            button = gameObject.AddComponent<Button>();
        
        button.onClick.AddListener(OnClickEvent);
    }
}
