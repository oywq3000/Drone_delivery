using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace TrikCamera
{
    public class TrickController : MonoBehaviour
    {
        // Start is called before the first frame update
        public Transform mTargetTRF;
        public float mSpeed = 2;
        public Vector3 Offset = new Vector3(0.1f,1.5f,-6.5f);
        private Vector3 pos;
        void Start()
        {
            transform.position = mTargetTRF.position + Offset;
            StartCoroutine(Tracking());
        }


        // Update is called once per frame
        IEnumerator Tracking()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                Vector3 affset = this.Offset; 
                Quaternion rot = Quaternion.AngleAxis(mTargetTRF.eulerAngles.y, Vector3.up); 
                Vector3 dir = rot * Offset; //旋转后的向量 
                pos = dir + mTargetTRF.position; 
                transform.position = Vector3.Lerp(this.transform.position, pos, mSpeed * Time.deltaTime); 
                transform.LookAt(mTargetTRF); 
                ZoomView(); 
            }
           
        }

        void ZoomView()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                transform.GetComponent<Camera>().fieldOfView += Time.deltaTime*100;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                transform.GetComponent<Camera>().fieldOfView -= Time.deltaTime*100;
            }
        }
    }
}