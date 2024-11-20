using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleKhatabook.Models.DTOs
{
    public class MasterDataJsonLanguageDTO
    {
        public int LanguageTypeId { get; set; }
        public string LanguageName { get; set; }
        [JsonProperty("TranslatedLanguage")]
        public string TranslatedFieldValue { get; set; }
    }
}
