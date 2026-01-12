using HalcyonHomeManager.Entities;
using System.Text.RegularExpressions;

namespace HalcyonHomeManager
{
    public static class Helpers
    {


        public static string ReturnDeviceFontSize()
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                return "Small";
            }
            else
            {
               return "Medium";
            }
        }

        public static double ReturnDeviceButtonWidth()
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                return 200;
            }
            else
            {
                return 130;
            }
        }

        public static bool ReturnDeviceNavigationDesktop()
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ReturnDeviceNavigationMobile()
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static ErrorLog ReturnErrorMessage(Exception ex, string className, string nethodName)
        {
            ErrorLog error = new ErrorLog();
            error.Message = ex.Message;
            error.ClassName = className;
            error.MethodName = nethodName;
            error.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
            return error;
        }

        public static ErrorLog ReturnLogMessage(string message, string className, string nethodName)
        {
            ErrorLog error = new ErrorLog();
            error.Message = message;
            error.ClassName = className;
            error.MethodName = nethodName;
            error.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
            return error;
        }

        public static bool IsPhoneValid(HouseHoldMember houseHoldMemberViewModel)
        {
            if (houseHoldMemberViewModel.PhoneNumber != null)
            {
                var test = RemoveSpecialCharacters(houseHoldMemberViewModel.PhoneNumber);
                if (test.Length == 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            try
            {
                var stripped = Regex.Replace(str, "[^a-zA-Z0-9% ._]", string.Empty);
                var strippedUnderscores = stripped.Replace("_", "");
                return strippedUnderscores;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static Microsoft.Maui.Graphics.Color SetStateColor(string state)
        {
            return state switch
            {
                "New" => Microsoft.Maui.Graphics.Color.FromRgb(255, 204, 0),
                "In Progress" => Microsoft.Maui.Graphics.Color.FromRgb(0, 72, 255),
                "Done" => Microsoft.Maui.Graphics.Color.FromRgb(0, 102, 0),
                _ => Microsoft.Maui.Graphics.Color.FromRgb(0, 72, 255),
            };
        }
    }


}

