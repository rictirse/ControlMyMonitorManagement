using System;
using System.Security.Principal;

namespace CMM.Library.Helpers
{
    public class UAC
    {
        public static void Check()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
                throw new Exception($"Cannot delete task with your current identity '{identity.Name}' permissions level." +
                "You likely need to run this application 'as administrator' even if you are using an administrator account.");

        }
    }
}
