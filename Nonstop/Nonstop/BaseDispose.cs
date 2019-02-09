using System;
using System.Collections.Generic;
using System.Text;

namespace Nonstop.Forms
{
    public class BaseDispose : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
