using _Core.Drove.Event;
using Drove.Command.Command;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using _Core.Scripts;

namespace QFramework.Example
{
    public class SetingPanelData : UIPanelData
    {
    }

    public partial class SetingPanel : UIPanel, IController
    {
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as SetingPanelData ?? new SetingPanelData();
            // please add init code here
            ExitBtn.onClick.AddListener(() => { UIKit.HidePanel<SetingPanel>(); });
            ExitBtn1.onClick.AddListener(() => { UIKit.HidePanel<SetingPanel>(); });
            AudioKit.Settings.MusicVolume.RegisterWithInitValue(v => BGMusicSeting.value = v / 2);
            BGMusicSeting.onValueChanged.AddListener(v =>  AudioKit.Settings.MusicVolume.Value = v*2);
            
            
            SoundEffectSeting.value = 0.25f;
            SoundEffectSeting.onValueChanged.AddListener(v => this.SendCommand(new SoundChanged(v)));
            InfSwitch.onValueChanged.AddListener(enabled => { this.SendCommand(new DroneInfSwitchChanged(enabled)); });
        }

        protected override void OnOpen(IUIData uiData = null)
        {
           
        }
        protected override void OnShow()
        {
        }
        protected override void OnHide()
        {
        }
        protected override void OnClose()
        {
        }
        public IArchitecture GetArchitecture()
        {
            return DroneArchitecture.Interface;
        }
    }
}