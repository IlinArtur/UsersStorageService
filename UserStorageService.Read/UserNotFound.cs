using System;
using System.Runtime.Serialization;

namespace UserStorageService.Read
{
    [DataContract]
    public class UserNotFound
    {
        [DataMember]
        public Guid Id { get; set; }
    }
}
