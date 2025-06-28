using System;

namespace IBID.WebService.Domain.Uteis
{
    public class ExcecaoCustomizada : Exception
    {
        public ExcecaoCustomizada(string message) : base(message)
        {

        }

    }
}
