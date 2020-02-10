namespace DocumentManagement.Commands.Results
{
    public enum CommandResultStatus
    {
        Unknown = 0,
        Success = 1,
        Fail = 2
    }

    public class CommandResult
    {
        public CommandResultStatus Status { get; }
        public string Message { get; }

        public CommandResult(CommandResultStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public static CommandResult GetFailed(string message) => 
            new CommandResult(CommandResultStatus.Fail, message);

        public static CommandResult GetSuccess() => 
            new CommandResult(CommandResultStatus.Success, string.Empty);
    }

    public class CommandResult<T> : CommandResult
    {
        public T Result { get; }

        public CommandResult(CommandResultStatus status, string message) : base(status, message)
        {
        }

        public CommandResult(T result) : base(CommandResultStatus.Success, string.Empty)
        {
            Result = result;
        }

        public static CommandResult<T> GetSuccess(T result) => new CommandResult<T>(result);
    }
}
