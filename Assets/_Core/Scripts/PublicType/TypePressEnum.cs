namespace Drove
{
    public enum PressedKeyCode
    {
        SpeedUpPressed,
        SpeedDownPressed,
        ForwardPressed,
        BackPressed,
        LeftPressed,
        RightPressed,
        TurnLeftPressed,
        TurnRightPressed
    }
    public class TargetPosition
    {
        //x has double meanings when x is not zero this command represent plain position;
        //when x = 0 represent heigh position
        public float x;
        public float y;
        public float z;
        public float FlightHeight;

        public TargetPosition(float x, float y, float z, float flightHeight)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            FlightHeight = flightHeight;
        }
    }

}