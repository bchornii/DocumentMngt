using System;

namespace DocumentManagement.Commands.Handlers.Compensations.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CompensationActionAttribute : Attribute
    {
        public string Name { get; set; }
    }
}