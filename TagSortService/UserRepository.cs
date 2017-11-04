using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Security;
using Bookmarks.Common;
using Bookmarks.Mongo.Data;
using AutoMapper;

namespace TagSortService
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(MongoUserContext context)
        {
            this.Context = context;
        }

        public MongoUserContext Context { get; private set; }
        IMapper MapperObj { get; set; }

        public void ConfigureMappings()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Bookmarks.Mongo.Data.User, TagSortService.User>();
            });

            this.MapperObj = config.CreateMapper();
        }

        public User GetUserByUserName(string user)
        {
            var mongoUser = Context.GetUserByUserName(user);
            return Mapper.Map<TagSortService.User>(mongoUser);
        }

        public bool ValidateUser(string user, string password)
        {
            return Context.ValidateUser(user, password); 
        }
    }
}