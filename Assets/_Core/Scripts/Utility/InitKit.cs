using QFramework;

namespace _Core.Scripts.Utility
{
  

    public static class InitKit
    {
        public static string GenerateId(int serialNum)
        {
            string id;
            if (serialNum < 10)
            {
                id = "000" + serialNum;
            }
            else if (serialNum < 100)
            {
                id = "00" + serialNum;
            }
            else if (serialNum < 1000)
            {
                id = "0" + serialNum;
            }
            else if (serialNum < 10000)
            {
                id = serialNum.ToString();
            }
            else
            {
                id = null;
            }
            return id;
        }
    }
}