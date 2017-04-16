using System;

namespace BarsGroup.CodeGuard
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GuardRedirectAttibute: Attribute
    {
        public GuardRedirectAttibute(string name)
        {
            
        }
    }
}