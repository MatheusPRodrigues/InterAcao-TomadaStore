using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomadaStore.Utils.Factories.Interfaces;

namespace TomadaStore.Utils.Factories
{
    public class SQLDBConnection : DBConnectionFactory
    {
        public override IDBConnection CreateDBConnection()
        {
            return new SQLDBConnectionImpl();
        }
    }
}
