using System;

namespace DocumentManagement.Commands.Common.Compensations
{
    /// <summary>
    /// Marks operation as a compensation operation for the main operation.
    /// Main operation name specified by <see cref="Name"/> property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CompensationActionAttribute : Attribute
    {
        /// <summary>
        /// Name of the action (aka. command) which can be potentially compensated.
        /// </summary>
        public string Name { get; set; }
    }
}