using System;

namespace OJ
{
    public class Result
    {
        public bool isPass()
        {
            return true;
        }
        public ResultInfo getResult()
        {
            return ResultInfo.Empty;
        }
        public string getDetails()
        {
            return String.Empty;
        }
        public int getTime()
        {
            return -1;
        }
        public int getMemory()
        {
            return -1;
        }
    }
    public class CompilationPassResult : Result
    {
        public bool isPass()
        {
            return true;
        }
    }
    public class CompilationErrorResult : Result
    {
        string message;
        public CompilationErrorResult(string message)
        {
            this.message = message;
        }
        public bool isPass()
        {
            return false;
        }
        public ResultInfo getResult()
        {
            return ResultInfo.CompileError;
        }
        public string getDetails()
        {
            return message;
        }
    }
    public class AcceptedResult : Result
    {
        int useTime;
        public AcceptedResult(int useTime)
        {
            useTime = this.useTime;
        }
        public bool isPass()
        {
            return true;
        }
        public ResultInfo getResult()
        {
            return ResultInfo.Accepted;
        }
        public int getTime() { return useTime; }
    }
    public class PresentationErrorResult : Result
    {
        public bool isPass()
        {
            return false;
        }
        public ResultInfo getResult()
        {
            return ResultInfo.PresentationError;
        }
    }
    public class WrongAnswerResult : Result
    {
        public string percent;
        public WrongAnswerResult(double p)
        {
            if (p == 1) percent = "";
            else percent = ((int)(p * 100)).ToString() + "%";
        }
        public bool isPass()
        {
            return false;
        }
        public ResultInfo getResult()
        {
            return ResultInfo.WrongAnswer;
        }
        public string getDetails()
        {
            return percent;
        }
    }
    public class TimeLimitExceededResult : Result
    {
        int useTime;
        public TimeLimitExceededResult(int useTime)
        {
            useTime = this.useTime;
        }
        public bool isPass()
        {
            return false;
        }
        public ResultInfo getResult()
        {
            return ResultInfo.TimeLimitExceeded;
        }
        public int getTime() { return useTime; }
    }
    public class MemoryLimitExceededResult : Result
    {
        int useMemory;
        public MemoryLimitExceededResult(int useMemory)
        {
            this.useMemory = useMemory;
        }
        public bool isPass()
        {
            return false;
        }
        public ResultInfo getResult()
        {
            return ResultInfo.MemoryLimitExceeded;
        }
        public int getMemory() { return useMemory; }
    }
    public class RuntimeErrorResult : Result
    {
        string message;
        public RuntimeErrorResult(string message)
        {
            this.message = message;
        }
        public bool isPass()
        {
            return false;
        }
        public ResultInfo getResult()
        {
            return ResultInfo.RuntimeError;
        }
        public string getDetails()
        {
            return message;
        }
    }
}
