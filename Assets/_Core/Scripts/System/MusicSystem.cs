
using _Core.Drove.Event;
using Cinemachine;
using Drove;
using QFramework;
using _Core.Scripts.Controller;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;


namespace _Core.Drove.Script.System
{
    public interface IMusicSystem:ISystem
    {
        
    }
    public class MusicSystem: AbstractSystem,IMusicSystem
    {
        private AudioSource _audioSource;
        protected override void OnInit()
        {
            _audioSource = Camera.main.GetComponent<AudioSource>();
            
            // 项目启动只调用一次即可
            ResKit.Init();

            // 通过资源名 + 类型搜索并加载资源（更方便）
            var prefab = mResLoader.LoadSync<GameObject>("AssetObj");
           
            // 通过 AssetBundleName 和 资源名搜索并加载资源（更精确）
            prefab = mResLoader.LoadSync<GameObject>("assetobj_prefab", "AssetObj");
        }

        // 每个脚本都需要
        private ResLoader mResLoader = ResLoader.Allocate();

      
        private void OnDestroy()
        {
            //Release Resource
            mResLoader.Recycle2Cache();
            mResLoader = null;
        }
        
        public void PlayBackGroundMusic()
        {
            
        }
        



    }
}