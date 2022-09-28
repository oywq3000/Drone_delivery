using System.Linq;
using _Core.Drove.Event;
using Drove;
using QFramework;
using _Core.Scripts.Controller;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;


namespace _Core.Drove.Script.System
{
    public interface INavigationSystem : ISystem
    {
        TargetPosition GetCargoPos(int index = 0);
        TargetPosition GetRestPos(int index = 0);
        TargetPosition GetCargoDes(CargoController cargo);
        void ReleaseChannel(int channel);
        void ReleaseSYstemModel();
    }

    public class NavigationSystem : AbstractSystem, INavigationSystem
    {
        private const float HeightOffset = 0.5f;

        public TargetPosition GetCargoPos(int index = 0)
        {
            var cargoPosLists = this.GetModel<ICargoDesMap>().CargoList;
            if (cargoPosLists.Count != 0)
            {
                //this.GetModel<ICargoDesMap>().CargoList.RemoveAt(index);
                var temp = new Vector3();
                temp = cargoPosLists[index].GetPos();
                cargoPosLists.RemoveAt(index);
                return new TargetPosition(temp.x, temp.y + HeightOffset,
                    temp.z,
                    GetFlightHeigh());
            }

            return null;
        }

        public TargetPosition GetCargoDes(CargoController cargo)
        {
            var desList = this.GetModel<ICargoDesMap>().DesList;
            if (desList.Count != 0)
            {
                foreach (var desController in desList)
                {
                    if (desController.Id == cargo.Id)
                    {
                        return new TargetPosition(desController.GetPos().x, desController.GetPos().y + HeightOffset + 1,
                            desController.GetPos().z,
                            GetFlightHeigh());
                    }
                }
            }

            return null;
        }

        public TargetPosition GetRestPos(int index = 0)
        {
            //get the model's reference
            var model = this.GetModel<IDroneRestPos>().RestList;
            if (model.Count != 0)
            {
                //this.GetModel<ICargoDesMap>().CargoList.RemoveAt(index);
                var temp = new Vector3();
                temp = model[index].GetPos();
                model.RemoveAt(index);
                return new TargetPosition(temp.x, temp.y + HeightOffset,
                    temp.z,
                    GetFlightHeigh());
            }

            return null;
        }

        private float GetFlightHeigh(int index = 0)
        {
            var ChannelModel = this.GetModel<IAerialDroneCount>();
            var key = ChannelModel.Channel.Keys.First();
            ChannelModel.Channel[key]--;
            if (ChannelModel.Channel.TryGetValue(key, out int value))
            {
                if (value == 0)
                {
                    //if this channel is full than remove this channel key
                    ChannelModel.Channel.Remove(key);
                }
            }
            return ChannelModel.OriginalHeight + key * ChannelModel.ChannelSpan;
        }

        public void ReleaseChannel(int channel)
        {
            var channelDic = this.GetModel<IAerialDroneCount>().Channel;
            if (channelDic.Keys.Contains(channel))
            {
                channelDic[channel]++;
            }
            else
            {
                //if this channel is full before this drone release they own channel
                //add back to the dictionary
                channelDic.Add(channel,1);
            }
           
        }

        public void ReleaseSYstemModel()
        {
            this.GetModel<IAerialDroneCount>().Channel.Clear();
            this.GetModel<IAerialDroneCount>().Channel.Clear();
            this.GetModel<IDroneRestPos>().RestList.Clear();
            this.GetModel<ICargoDesMap>().DesList.Clear();
            this.GetModel<ICargoDesMap>().CargoList.Clear();
        }

        protected override void OnInit()
        {
        }
    }
}