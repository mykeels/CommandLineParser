using CommandLineParser.Helpers;
using System;
using System.Reflection;

namespace CommandLineParser.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class TransformAttribute : Attribute
    {
        private TransformDelegate delegateInstance;
        private Type targetType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType">Parent Type of delegate function</param>
        /// <param name="delegateName">Name of delegate function</param>
        public TransformAttribute(Type targetType, string delegateName)
        {
            this.targetType = targetType;

            this.delegateInstance = (string data) => targetType.GetMethod(delegateName).Invoke(System.Activator.CreateInstance(targetType), new[] { data });
        }

        public Delegate Execute
        {
            get
            {
                return this.delegateInstance;
            }
        }

        public Type TargetType
        {
            get
            {
                return this.targetType;
            }
        }
    }
}
