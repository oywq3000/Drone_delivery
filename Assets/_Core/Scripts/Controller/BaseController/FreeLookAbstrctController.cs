
using UnityEngine;
using Cinemachine;
namespace FreeLookCustom
{
    [RequireComponent(typeof(CinemachineFreeLook))]
    public  class FreeLookAbstrctController : MonoBehaviour
    {
       
        public bool isStartCamera = false;
        protected CinemachineFreeLook m_FreeLook;
        protected Ray ray;
        protected RaycastHit hit;
        //用来显示和控制该相机的scale(即等比缩放相机的轨道、视角)
        #region 在界面中更改的参数

        //视野和轨道的规格大小，可等比放大缩小范围
        [Range(0, 5000)] public float m_CameraScale;
        [Range(0, 100)] public int m_View;
        [Range(0, 10)] public float m_TopRadius;
        [Range(0, 10)] public float m_MidRadius;
        [Range(0, 10)] public float m_BottomRadius;
        [Range(0, 10)] public float m_TopHeight;
        [Range(0, 10)] public float m_BottomHeight;
        [Range(0, 10)] public float m_XValue;
        [Range(0, 10)] public float m_YValue;
        #endregion
        //拿到壳，即摄像机的触发碰撞体
        private GameObject m_Shell;
        protected virtual void Start()
        {
            m_FreeLook = GetComponent<CinemachineFreeLook>();
            m_FreeLook.m_YAxis.m_MaxSpeed = 0;
            m_FreeLook.m_XAxis.m_MaxSpeed = 0;
            //此函数为监听鼠标
            if (isStartCamera)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
            
        }

        
        protected virtual void Update()
        {
            m_FreeLook.m_YAxis.m_MaxSpeed = 0;
            m_FreeLook.m_XAxis.m_MaxSpeed = 0;
            if (Input.GetMouseButton(1))
            {
                m_FreeLook.m_YAxis.m_MaxSpeed = 3;
                m_FreeLook.m_XAxis.m_MaxSpeed = 360;
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                m_CameraScale -= Time.deltaTime * 5000;
                m_FreeLook.m_Orbits[1].m_Radius = m_CameraScale * (m_MidRadius / 50f);
                m_FreeLook.m_Orbits[0].m_Radius = m_CameraScale * (m_TopRadius / 50f);
                m_FreeLook.m_Orbits[2].m_Radius = m_CameraScale * (m_BottomRadius / 50f);
                m_FreeLook.m_Orbits[0].m_Height = m_CameraScale * (m_TopHeight / 50f);
                m_FreeLook.m_Orbits[2].m_Height = m_CameraScale * (-m_BottomHeight / 50f);
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                m_CameraScale += Time.deltaTime * 5000;
                m_FreeLook.m_Orbits[1].m_Radius = m_CameraScale * (m_MidRadius / 50f);
                m_FreeLook.m_Orbits[0].m_Radius = m_CameraScale * (m_TopRadius / 50f);
                m_FreeLook.m_Orbits[2].m_Radius = m_CameraScale * (m_BottomRadius / 50f);
                m_FreeLook.m_Orbits[0].m_Height = m_CameraScale * (m_TopHeight / 50f);
                m_FreeLook.m_Orbits[2].m_Height = m_CameraScale * (-m_BottomHeight / 50f);
            }
        }

#if UNITY_EDITOR
        private CinemachineFreeLook m_FreeLook1;

        private void OnValidate()
        {
            m_FreeLook1 = this.gameObject.GetComponent<CinemachineFreeLook>();
            //将相机的壳，也就是相机外碰撞体置于相机跟随的物
            //m_Shell.transform.position = m_FreeLook1.Follow.transform.position;
            m_FreeLook1.m_Lens.FieldOfView = (int) m_View;
            m_FreeLook1.m_Orbits[1].m_Radius = m_CameraScale * (m_MidRadius / 50f);
            m_FreeLook1.m_Orbits[0].m_Radius = m_CameraScale * (m_TopRadius / 50f);
            m_FreeLook1.m_Orbits[2].m_Radius = m_CameraScale * (m_BottomRadius / 50f);
            m_FreeLook1.m_Orbits[0].m_Height = m_CameraScale * (m_TopHeight / 50f);
            m_FreeLook1.m_Orbits[2].m_Height = m_CameraScale * (-m_BottomHeight / 50f);
        }
        private void Reset()
        {
            m_FreeLook1 = GetComponent<CinemachineFreeLook>();
            //进入Editor时调用，或进入点击Reset
            m_Shell = transform.parent.gameObject;
            m_CameraScale = 200;
            m_View = 40;
            m_TopRadius = 1.5f;
            m_MidRadius = 3;
            m_BottomRadius = 1.5f;
            m_TopHeight = 2;
            m_BottomHeight = 2;
            m_FreeLook1.m_Lens.FieldOfView = (int) m_View;
            m_FreeLook1.m_Orbits[1].m_Radius = m_CameraScale * (m_MidRadius / 50f);
            m_FreeLook1.m_Orbits[0].m_Radius = m_CameraScale * (m_TopRadius / 50f);
            m_FreeLook1.m_Orbits[2].m_Radius = m_CameraScale * (m_BottomRadius / 50f);
            m_FreeLook1.m_Orbits[0].m_Height = m_CameraScale * (m_TopHeight / 50f);
            m_FreeLook1.m_Orbits[2].m_Height = m_CameraScale * (-m_BottomHeight / 50f);
            m_FreeLook1.m_XAxis.Value += m_XValue;
            m_FreeLook1.m_YAxis.Value += m_YValue;
        }

#endif
    }
}