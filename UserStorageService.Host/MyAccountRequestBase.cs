using System;

namespace UserStorageService.Host
{
    public abstract class MyAccountRequestBase
    {
        public Guid UserId { get; set; }
        public Guid RequestId { get; set; }
    }
}
