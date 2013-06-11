using System;

namespace Benchy
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TeardownAttribute : Attribute
    {
        public object[] MethodParameters { get; set; }

        public TeardownAttribute(params object[] methodParameters)
        {
            MethodParameters = methodParameters;
        }

    }
}