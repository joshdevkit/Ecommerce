using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces.Auth
{
    public interface ITokenGenerator
    {
        string GenerateToken(User user);
    }
}
