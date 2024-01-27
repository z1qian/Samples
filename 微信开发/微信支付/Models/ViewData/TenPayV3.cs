using Senparc.Weixin.MP.Helpers;

namespace 微信支付.Models.ViewData
{

    public class Base_TenPayV3VD
    {
        public JsSdkUiPackage JsSdkUiPackage { get; set; }

        public string Msg { get; set; }
    }

    public class TenPayV3_Index : Base_TenPayV3VD
    {
        public Dictionary<string, int> ProductList { get; set; }
    }

    public class TenPayV3_Odrer : Base_TenPayV3VD
    {
        public string Product { get; set; }
        public string Package { get; set; }
        public string PaySign { get; set; }
    }
}
