using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Nonstop.Forms.DataManagement
{
    public interface ISQLite
    {
        SQLiteAsyncConnection GetConnection();
    }
}
