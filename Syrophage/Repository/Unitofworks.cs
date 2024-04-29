﻿using Syrophage.Data;
using Syrophage.Repository.IRepository;

namespace Syrophage.Repository
{
    public class Unitofworks : IUnitofWorks
    {
        private readonly ApplicationDbContext _db;
        public IContactRepository Contact { get; set; }

        public INewsletterRepository Newsletter { get; set; }

        public IUserRepository User { get; set; }

        public Unitofworks(ApplicationDbContext _db)
        {
            this._db = _db;
            Contact = new ContactRepository(_db);
            Newsletter = new NewsletterRepository(_db);
            User = new UserRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }

}
