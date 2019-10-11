using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.XamarinForms.Controls
{
    public class Controls
    {
        public static bool IsInitialized
        {
            get;
            private set;
        }

        public static void Init()
        {
            if (!IsInitialized)
                IsInitialized = true;
        }
    }
}
