using System;

namespace PB.ITOps.Messaging.PatSender.Extensions
{
    public static class TypeExtensions
    {
        public static string SimpleQualifiedName(this Type t)
        {
            return string.Concat(t.FullName, ", ", t.Assembly.GetName().Name);
        }
    }
}