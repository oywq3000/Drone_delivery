using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FreeLookCustom
{
    public class MouseEvent_Camera : MonoBehaviour
    {
        public bool isBack;
        //射线
        private Ray ray;
        private RaycastHit hit;
        private CameraActiveCtrl CurrentCtrl;
        private GameObject m_TempGameObj;
        private Button backButton;
        private void Start()
        {
            StartCoroutine(Listening());//启动协程
            isBack = false;
        }
        //协程监听相机
        IEnumerator Listening()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);//发出射线
                    if (Physics.Raycast(ray, out hit))
                    {
                        m_TempGameObj = hit.collider.gameObject;
                        //获得当前点击到的物体
                        if (m_TempGameObj.tag == "MoveCamera")
                        {
                            if (m_TempGameObj.GetComponent<CameraActiveCtrl>().m_ParaentCamera ==
                                CurrentCtrl)
                            {
                                CurrentCtrl.OnDisablizing();
                                m_TempGameObj.GetComponent<CameraActiveCtrl>().OnEnablizing();
                            }

                        }
                    }
                }
                if (isBack)
                {
                    // 判断当前相机是否有父相机，如有则跳转到该父相机
                    if (CurrentCtrl.m_ParaentCamera != null)
                    {
                        CurrentCtrl.OnDisablizing();
                        CurrentCtrl.m_ParaentCamera.OnEnablizing();
                        isBack = false;
                    }
                }
                yield return null;
            }
        }

        public void OnEntryCamera()
        {
            //每激活一个相机是将当前相机的壳赋给CurrentCtrl
            CurrentCtrl = GetComponent<CinemachineBrain>()
                .ActiveVirtualCamera
                .VirtualCameraGameObject
                .GetComponentInParent<CameraActiveCtrl>();
        }
        //按钮控制跳转
        public void BackBotton()
        {
            isBack = true;
        }
    }
}

