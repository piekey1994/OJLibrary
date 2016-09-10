using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace OJ
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void CppMain(string a, string b, string c);

    public class Tester
    {
        private List<string> input;
        private List<string> output;
        private int maxTime;
        private int maxMemory;
        private string dllPath;
        private string guid;

        public Tester(_Question q,List<_TestCase> t,string dllPath)
        {
            maxTime = q.MaxTime;
            maxMemory = q.MaxMemory;
            input=new List<string>();
            output=new List<string>();
            foreach(var it in t)
            {
                input.Add(it.input);
                output.Add(it.output);
            }
            this.dllPath = dllPath;
            guid=Guid.NewGuid().ToString();
        }

        public Result Test()
        {
            StreamWriter oldOut = new StreamWriter(Console.OpenStandardOutput(), System.Text.Encoding.Default);
            StreamReader oldIn = new StreamReader(Console.OpenStandardInput(), System.Text.Encoding.Default);
            oldOut.AutoFlush = true;

            int waCount = 0;
            int useTimeMax = -1;
            for (int i =0 ;i<input.Count;i++)
            {
                string nowInput = input[i];
                string nowOutput = output[i];
                Task<string> msgTask=null;
                Task<string> outTask = null;
                string errorMsg = null;
                using (NamedPipeServerStream inPipe = new NamedPipeServerStream("inpipe" + guid, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous),
                    outPipe = new NamedPipeServerStream("outpipe" + guid, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous),
                    msgPipe = new NamedPipeServerStream("msgpipe" + guid, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous))
                {
                        
                    StreamWriter inWriter = new StreamWriter(inPipe);
                    inWriter.Write(nowInput);
                    StreamReader outReader = new StreamReader(outPipe);
                    StreamReader msgReader = new StreamReader(msgPipe);
                    inPipe.BeginWaitForConnection(
                        delegate
                        {
                            inPipe.WaitForConnection(); 
                            inWriter.Flush();
                            inPipe.Close();
                        },
                        null);
                    outPipe.BeginWaitForConnection(
                        delegate
                        {
                            outPipe.WaitForConnection();
                            outTask = outReader.ReadToEndAsync();
                        },
                        null);
                    msgPipe.BeginWaitForConnection(
                        delegate
                        {
                            msgPipe.WaitForConnection();
                            msgTask = msgReader.ReadToEndAsync();
                        },
                        null);
                    Task task = new Task(()=>runCpp(ref errorMsg));
                    task.Start();
                    if (task.Wait(maxTime + 5000))
                    {
                        if (errorMsg != null)
                        {
                            Console.SetIn(oldIn);
                            Console.SetOut(oldOut);
                            return new RuntimeErrorResult(errorMsg);
                        }
                        while (outTask == null || msgTask == null) ;
                        outTask.Wait();
                        msgTask.Wait();
                        //string result = msgReader.ReadToEnd();
                        string result = msgTask.Result;
                        Debug.Assert(!result.Equals(""), "管道不通");
                        int useTime = Int32.Parse(result);
                        if (useTime > maxTime)
                        {
                            return new TimeLimitExceededResult(useTime);
                        }
                        else
                        {
                            string answer = outTask.Result;
                            //Console.WriteLine("C#1:" + answer + " " + nowOutput);
                            if (!answer.Equals(nowOutput))
                            {
                                string aStr = answer.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                                string bStr = nowOutput.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                                //Console.WriteLine("C#2:" + aStr + " " + bStr);
                                if (aStr.Equals(bStr))
                                {
                                    return new PresentationErrorResult();
                                }
                                else
                                {
                                    waCount++;
                                    if (useTimeMax < useTime) useTimeMax = useTime;
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.SetIn(oldIn);
                        Console.SetOut(oldOut);
                        return new TimeLimitExceededResult(-1);
                    }                          
                }           
            }
            if (waCount == 0)
            {
                return new AcceptedResult(useTimeMax);
            }
            else
            {
                return new WrongAnswerResult((double)waCount / input.Count);
            }
        }//Test Over
        private void runCpp(ref string msg)
        {
            using (DllInvoke dll = new DllInvoke(dllPath))
            {
                CppMain test = (CppMain)dll.Invoke("MyMain", typeof(CppMain));
                try
                {
                    test(@"\\.\pipe\inpipe" + guid, @"\\.\pipe\outpipe" + guid, @"\\.\pipe\msgpipe" + guid);
                }
                catch (Exception e)
                {
                    msg = e.Message;
                }
            }
        }
    }
}
