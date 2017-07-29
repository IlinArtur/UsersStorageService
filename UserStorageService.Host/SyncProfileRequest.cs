using System;
using System.ComponentModel.DataAnnotations;
using UserStorageService.Host;

namespace UserStoreageService.Host
{
    public class SyncProfileRequest : MyAccountRequestBase
    {
        public bool? AdvertisingOptIn { get; set; }

        [Required]
        [RegularExpression("[A-z]{2}")]
        public string CountryIsoCode { get; set; }

        public DateTime DateModified { get; set; }

        [Required]
        [RegularExpression("[A-z]{2}(-[A-z]{2})?")]
        public string Locale { get; set; }
    }
}