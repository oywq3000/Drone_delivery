using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FreeLookCustom
{

    public class CameraActiveCtrl : MonoBehaviour
    {
        private GameObject m_PointCamera;

        private CinemachineFreeLook m_PointFreeLook;
        private FreeLookAbstrctController _freeLookAbstrctController;
        private SphereCollider m_SphereCollider;
        [HideInInspector]
        public int Priority;

        private float DelayTime;

        //用来记录父物体的Ctrl组件，方便返回上一阶层摄像机
        public CameraActiveCtrl m_ParaentCamera = null;

        //判断是否是根相机，即开始相机
        public bool isRootCammera = false;
        struct InitCameraPos
        {
            public float x_Value;
            public float y_Value;
        }

        InitCameraPos m_InitCameraPos;

        private void Start()
        {
            //将初始状态下的相机位置赋格m_InitCameraPos
            m_InitCameraPos.x_Value = GetComponentInChildren<CinemachineFreeLook>().m_XAxis.Value;
            m_InitCameraPos.y_Value = GetComponentInChildren<CinemachineFreeLook>().m_YAxis.Value;
            Priority = GetComponentInChildren<CinemachineFreeLook>().Priority;
            DelayTime = Camera.main.gameObject.GetComponent<CinemachineBrain>().m_DefaultBlend.BlendTime;
            m_PointCamera = transform.Find("PointCamera").gameObject;


            m_PointFreeLook = m_PointCamera.GetComponent<CinemachineFreeLook>();
            _freeLookAbstrctController = m_PointCamera.GetComponent<FreeLookAbstrctController>();
            m_SphereCollider = GetComponent<SphereCollider>();

            if (!isRootCammera)
            {
                //如果不是根相机则付完值后将其隐藏,最初
                m_PointCamera.SetActive(false);
            }
            else
            {
                //如果是根相机这将其碰撞体关闭
                m_SphereCollider.enabled = false;
                //开启监听鼠标
                _freeLookAbstrctController.enabled = true;

            }

        }

        public void OnEnablizing()
        {
            GetComponent<SphereCollider>().enabled = false;
            m_PointCamera.SetActive(true);
        }

        public void OnDisablizing()
        {
            m_PointCamera.GetComponent<CinemachineFreeLook>().Priority = 0;
            StopAllCoroutines();
            StartCoroutine(RecoveryingToHint_Instant());
        }
        IEnumerator RecoveryingToHint_Instant()
        {
            float timer = 0;
            while (timer < DelayTime - 0.05f)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            m_PointFreeLook.m_XAxis.Value = m_InitCameraPos.x_Value;
            m_PointFreeLook.m_YAxis.Value = m_InitCameraPos.y_Value;
            m_SphereCollider.enabled = true;
            m_PointFreeLook.Priority = Priority;
            m_PointCamera.SetActive(false);
        }


     

    }
}



