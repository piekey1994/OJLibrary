using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace OJ
{
    public class PathConfig
    {
        public string IncludePath
        {
            get { return @"D:\Programs\VC6\VC98\Include"; }
        }
        public string LibPath
        {
            get { return @"D:\Programs\VC6\VC98\Lib"; }
        }
        public string CodePath
        {
            get { return @"code\"; }
        }
    }
    public enum ResultInfo
    {
        [Description("")]
        Empty,
        [Description("编译中")]
        Compiling,
        [Description("编译错误")]
        CompileError,
        [Description("运行中")]
        Testing,
        [Description("正确")]
        Accepted,
        [Description("格式错误")]
        PresentationError,
        [Description("答案错误")]
        WrongAnswer,
        [Description("时间超限")]
        TimeLimitExceeded,
        [Description("内存超限")]
        MemoryLimitExceeded,
        [Description("运行错误")]
        RuntimeError,
    }
    public class Tools
    {
        public static string GetDescription(this Enum value, Boolean nameInstead = true)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }

            FieldInfo field = type.GetField(name);
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            if (attribute == null && nameInstead == true)
            {
                return name;
            }
            return attribute == null ? null : attribute.Description;
        }
        public static void Runcmd(string cmd)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(cmd);
            p.StandardInput.WriteLine("exit");
            p.WaitForExit();
            p.Close();
        }
    }
}
