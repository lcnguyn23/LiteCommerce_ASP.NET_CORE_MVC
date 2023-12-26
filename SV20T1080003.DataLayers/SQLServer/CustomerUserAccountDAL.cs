using SV20T1080003.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080003.DataLayers.SQLServer
{
    public class CustomerUserAccountDAL : _BaseDAL, IUserAccountDAL
    {
        public CustomerUserAccountDAL(string connectionString) : base(connectionString)
        {
        }

        public UserAccount? Authorize(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public bool ChangePassword(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
