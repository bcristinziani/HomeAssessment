using HomeAsessment.Business;
using System;

namespace HomeAsessment
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var versionManagement = new VersionManagement();
            versionManagement.Start(args);
        }
    }
}
