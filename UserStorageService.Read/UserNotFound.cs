using System;

namespace UserStorageService.Read
{
    public class UserNotFound
    {
        public Guid Id { get; }

        public UserNotFound(Guid id)
        {
            Id = id;
        }
    }
}
