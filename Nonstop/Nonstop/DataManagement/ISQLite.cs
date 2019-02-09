using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Nonstop.Forms.DataManagement
{
    public interface ISQLite
    {
        SQLiteConnection getConnection(string path, string db);
    }
}
