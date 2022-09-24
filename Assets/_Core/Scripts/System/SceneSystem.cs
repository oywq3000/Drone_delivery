using _Core.Drove.Event;
using QFramework;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace _Core.Drove.Script.System
{
    public interface ISceneSystem : ISystem
    {
        void SwitchScene(string targetSenceName);
    }

    public class SceneSystem : AbstractSystem, ISceneSystem
    {
        protected override void OnInit()
        {
        }

        public void SwitchScene(string targetSenceName)
        {
            SceneManager.LoadScene(targetSenceName);
        }
    }
}