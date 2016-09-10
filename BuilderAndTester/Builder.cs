using System;
using System.IO;

using File = System.IO.File;

namespace OJ
{
    public class Builder
    {
        private string code;
        private string includePath;
        private string libPath;
        private string guid;
        private string prefix;
        public Builder(_SubmittedCode sc)
        {
            code = sc.Code;
            PathConfig config=new PathConfig();
            includePath = config.IncludePath;
            libPath = config.LibPath;
            guid = Guid.NewGuid().ToString();
            prefix = config.CodePath + guid;
            using (StreamWriter sw = new StreamWriter(File.OpenWrite(prefix + ".cpp")))
            {
                sw.Write(code);
            }
        }
        public Result Build()
        {
            //编译
            string cmd = "@cl.exe /I " + "\"" + includePath + "\"" + " /nologo /MT /O2 /GX /Fp\"MyDLL.pch\" " + prefix + ".cpp /Fo\"" + prefix + ".obj\" " + "/c /D WIN32 /D NDEBUG /D _WINDOWS /D _MBCS /D _USRDLL > \"" + prefix + ".cpl\"";
            Tools.Runcmd(cmd);
            using (StreamReader sr = new StreamReader(File.OpenRead(prefix + ".cpl")))
            {
                string cplResult = sr.ReadToEnd();
                if (cplResult.Contains("error"))
                {
                    return new CompilationErrorResult(cplResult.Replace(guid, "main"));
                }
            }
            //连接
            cmd = "@link " + prefix + ".obj pipe.obj /out:" + prefix + ".dll" + " /nologo /dll /incremental:no /LIBPATH:\"" + libPath + "\" > " + prefix + ".link";
            Tools.Runcmd(cmd);
            using (StreamReader sr = new StreamReader(System.IO.File.Open(prefix + ".link", FileMode.Open)))
            {
                string linkResult = sr.ReadToEnd();
                if (linkResult.Contains("error"))
                {
                    return new CompilationErrorResult(linkResult.Replace(guid, "main"));
                }
                else
                {
                    return new CompilationPassResult();
                }
            }   
        }
        public string DllPath
        {
            get { return prefix+".dll"; }
        }
    }
}
