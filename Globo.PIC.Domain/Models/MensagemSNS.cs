using Newtonsoft.Json;

namespace Globo.PIC.Domain.Models
{

    /// <summary>
    /// Empacotamento de mensagem para SNS
    /// </summary>
    public class MensagemSNS<T>
    {

        string _default;

        /// <summary>
        /// 
        /// </summary>
        public string @default
        {
            get
            {
                return _default;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_payload"></param>
        public MensagemSNS(T _payload)
        {
            _default = JsonConvert.SerializeObject(_payload, new JsonSerializerSettings(){
				ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Serialize() => JsonConvert.SerializeObject(this, new JsonSerializerSettings(){
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        });
    }
}
