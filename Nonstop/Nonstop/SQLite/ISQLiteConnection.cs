using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Nonstop.Forms.SQLite
{
    public interface ISQLiteConnection
    {
        SQLiteConnection getConnection(string path, string db);
    }
}
