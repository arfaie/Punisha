using Ecommerce.Net.OptionEnums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static Ecommerce.Models.ViewModels.SMSViewModel;

namespace Ecommerce.Areas.Helpers
{
    public class SendSMS
    {
        string Baseurl = "https://api.kavenegar.com/";
        public async Task<bool> SendAsync( short Type, string Mobile, string token, string token2 = null)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = null;

                if (Type == (int)SMSTypes.Register)
                {
                    Res = await client.GetAsync("v1/6E65695778564344644231365673544D73794E324C7377634149324F7A5270324F6346774D6753666363633D/verify/lookup.json?receptor=" + Mobile + "&token=" + token + "&token2=" + "Carbiotic.ir" + "&template=CarbioticRegister");

                }
                if (Type == (int)SMSTypes.ResendUserPass)
                {
                    Res = await client.GetAsync("v1/6E65695778564344644231365673544D73794E324C7377634149324F7A5270324F6346774D6753666363633D/verify/lookup.json?receptor=" + Mobile + "&token=" + token + "&token2=" + token2 + "&template=CarbioticReset");

                }
                if (Res.IsSuccessStatusCode && Res != null)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;

                    try
                    {
                        RootObject datalist = JsonConvert.DeserializeObject<RootObject>(Response);
                    }
                    catch (Exception e)
                    {

                        throw;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}
