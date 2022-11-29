using BackEnd.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Core.Services
{
    public interface IUserService
    {
        User CurrentUset { get; }
    }
}
