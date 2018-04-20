using UnityEngine;
using UnityEngine.UI;

namespace Portfolio
{
    public abstract class GUIIconBarGenerator : MonoBehaviour, IGUIGeneratable
    {


        protected RectTransform bar;

        [SerializeField]
        protected Image barEnd;

        [SerializeField]
        protected bool GenerateOnStart;

        [SerializeField]
        protected Image barConnector;

        [SerializeField]
        protected Image iconPrefab;

        protected IIconBarGeneratable[] objsToGenerate;
        protected abstract IIconBarGeneratable[] GetObjsToGenerate();
      

        protected virtual void Start()
        {
            bar = GetComponent<RectTransform>();
            objsToGenerate = GetObjsToGenerate();
            if(GenerateOnStart)
                Generate();
        }

        protected virtual void OnIconInstantiate(GameObject createdIcon, int i)
        {
             
        }

        protected virtual bool IconGenerationStatement(int i)
        {
            return true;
        }

        public void Generate()
        {
            //Utility.DestoryChildrens(transform);
            int vertical = GetComponent<VerticalLayoutGroup>() ? 1 : 0;

            Image _temp;

            for (int i = 0; i < objsToGenerate.Length; i++)
            {
                if (i == 0)
                {
                    _temp = Instantiate(barEnd, bar);
                    _temp.transform.Rotate(new Vector3(0.0f, 0.0f, -90 * vertical));
                }

                if(IconGenerationStatement(i))
                {
                    _temp = Instantiate(iconPrefab, bar);
                    Utility.GetComponentInChildrenWithoutParent<Image>(_temp.transform).sprite = objsToGenerate[i].getIcon();
                    OnIconInstantiate(_temp.gameObject, i);
                }


                if (i != objsToGenerate.Length - 1)
                {
                    _temp = Instantiate(barConnector, bar);
                    _temp.transform.Rotate(new Vector3(0.0f, 0.0f, -90 * vertical));

                }
                else
                {
                    _temp = Instantiate(barEnd, bar);
                    float z = vertical == 1 ? 90 : 180;
                    _temp.transform.Rotate(new Vector3(0.0f, 0.0f, z));
                }

            }

        }
    }
}