// Filters/IgnoreAuthAttribute.cs
// Login/Logout/Error jaisi public actions ko is attribute se mark karo — global filter unhe skip kar dega.
using System;

namespace Regis.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class IgnoreAuthAttribute : Attribute
    {
    }
}