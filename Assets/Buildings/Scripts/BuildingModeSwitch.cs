using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public class BuildingModeSwitch : MonoBehaviour
    {

        [SerializeField]
        GameObject buildingCameraController;

        bool _isOnBuildingMode = false;

        public bool isOnBuildingMode
        {
            get
            {
                return _isOnBuildingMode;
            }

            set
            {
                _isOnBuildingMode = value;

                if (value) GUIManager.manager.GenerateTrapbar();
                else Utility.DestoryChildrens(GUIManager.manager.Trapbar.transform);

                DisablePlayerControl(!isOnBuildingMode);
                CameraModeSwitch(isOnBuildingMode);

                if (!value) BuildManager.instance.FreeCurrentBuilding();

                buildingCameraController.SetActive(isOnBuildingMode);
            }
        }

        public void switchMode()
        {
            isOnBuildingMode = !isOnBuildingMode;
        }

        void DisablePlayerControl(bool b)
        {
            GameManager.manager.player.GetComponent<CharacterInput>().enabled = b;
        }

        void CameraModeSwitch(bool b)
        {
            if (b)
                Camera.main.GetComponent<SimpleFollowCamera>().FollowTarget = buildingCameraController.transform;
            else
                Camera.main.GetComponent<SimpleFollowCamera>().FollowTarget = GameManager.manager.player.transform;
        }
    }
}