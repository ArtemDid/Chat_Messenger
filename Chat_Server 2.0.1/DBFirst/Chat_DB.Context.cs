﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Chat_Server.DBFirst
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ChatMessengerEntities : DbContext
    {
        public ChatMessengerEntities()
            : base("name=ChatMessengerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual int stp_UserAdd(string firstName, string login_, string password_, Nullable<int> role_, Nullable<int> ava, ObjectParameter userID)
        {
            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));
    
            var login_Parameter = login_ != null ?
                new ObjectParameter("Login_", login_) :
                new ObjectParameter("Login_", typeof(string));
    
            var password_Parameter = password_ != null ?
                new ObjectParameter("Password_", password_) :
                new ObjectParameter("Password_", typeof(string));
    
            var role_Parameter = role_.HasValue ?
                new ObjectParameter("Role_", role_) :
                new ObjectParameter("Role_", typeof(int));
    
            var avaParameter = ava.HasValue ?
                new ObjectParameter("Ava", ava) :
                new ObjectParameter("Ava", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("stp_UserAdd", firstNameParameter, login_Parameter, password_Parameter, role_Parameter, avaParameter, userID);
        }
    
        public virtual ObjectResult<stp_UserById_Result> stp_UserById(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<stp_UserById_Result>("stp_UserById", userIdParameter);
        }
    
        public virtual ObjectResult<stp_UserByLogin_Result> stp_UserByLogin(string userLogin, string userPassword)
        {
            var userLoginParameter = userLogin != null ?
                new ObjectParameter("UserLogin", userLogin) :
                new ObjectParameter("UserLogin", typeof(string));
    
            var userPasswordParameter = userPassword != null ?
                new ObjectParameter("UserPassword", userPassword) :
                new ObjectParameter("UserPassword", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<stp_UserByLogin_Result>("stp_UserByLogin", userLoginParameter, userPasswordParameter);
        }
    }
}
