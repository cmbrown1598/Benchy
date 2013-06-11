using System;

namespace Benchy
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SetupAttribute : Attribute
    {
        public object[] MethodParameters { get; set; }

        public SetupAttribute(params object[] methodParameters)
        {
            MethodParameters = methodParameters;
        }
    }
}