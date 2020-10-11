using Chat_Server.DBFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity.Core.Objects;

namespace Chat_Server.DataLayer
{
    public static class _DL
    {
        public static int Add(stp_UserById_Result newUser)
        {
            using (var db = new ChatMessengerEntities())
            {
                ObjectParameter newUserIDParam = new ObjectParameter("UserID", 0);

                db.stp_UserAdd(
                    firstName: newUser.FirstName,
                    login_: newUser.Login_,
                    password_: newUser.Password_,
                    role_: newUser.Role_,
                    ava: newUser.Ava,
                    userID: newUserIDParam
                    );

                return (int)newUserIDParam.Value;
            }



        }



    }
}
