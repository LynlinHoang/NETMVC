﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020056.BusinessLayers
{
    public static class Configuration
    {
        public static string ConnectionString { get; private set; } = "";

        public static void Initialize(string connectionString)
        {
            Configuration.ConnectionString = connectionString;
        }

    }
}
