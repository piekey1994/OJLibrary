using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
namespace OJ
{
    public class _SubmittedCode
    {
        public _SubmittedCode(int cid, int qid, string c)
        {
            CodeID = cid;
            QuestionID = qid;
            Code = c;
        }
        public _SubmittedCode()
        {
        }
        public int CodeID
        {
            get;
            private set;
        }
        public int UserID
        {
            get;
            private set;
        }
        public int QuestionID
        {
            get;
            private set;
        }
        public string Code
        {
            get;
            private set;
        }
        public int RunTime
        {
            get;
            private set;
        }
        public int Memory
        {
            get;
            private set;
        }
        public string Result
        {
            get;
            private set;
        }
        public string Describe
        {
            get;
            private set;
        }
        public string Time
        {
            get;
            private set;
        }
        public int State
        {
            get;
            private set;
        }
    }
    public class _Question
    {
        public _Question() { }
        public _Question(int qid, int mt, int mm)
        {
            QuestionID = qid;
            MaxTime = mt;
            MaxMemory = mm;
        }
        public int QuestionID
        {
            get;
            private set;
        }
        public int MaxTime
        {
            get;
            private set;
        }
        public int MaxMemory
        {
            get;
            private set;
        }
    }
    // private int maxTime;
    //  private int maxMemory;
    /*public Program(int aabb)
    {
        AABB = aabb;
    }
    public int program
    {
        get  { return programID; }
    }
    public int AABB
    {
        get;
        private set;
    }
        
    public int getProgramID()
    {
        return this.programID;
    }
    public int getMaxTime()
    {
        return this.maxTime;
    }
    public int getMaxMemory()
    {
        return this.maxMemory;
    }
    public int getUserID()
    {
        return this.userID;
    }
    public int getProblemID()
    {
        return this.problemID;
    }
    public string getProgramCode()
    {
        return this.programCode;
    }
    public float getRunTime()
    {
        return this.runTime;
    }
    public float getMemory()
    {
        return this.memory;
    }
    public string getResult()
    {
        return this.result;
    }
    public string getDescribe()
    {
        return this.describe;
    }
    public string getTime()
    {
        return this.time;
    }
    public int getState()
    {
        return this.state;
    }
    public void setProgramID(int programID)
    {
        this.programID = programID;
    }
    public void setUserID(int userID)
    {
        this.userID = userID;
    }
    public void setProblemID(int problemID)
    {
        this.problemID = problemID;
    }
    public void setProgramCode(string programCode)
    {
        this.programCode = programCode;
    }
    public void setRunTime(int runTime)
    {
        this.runTime = runTime;
    }
    public void setMemory(int memory)
    {
        this.memory = memory;
    }
    public void setResult(string result)
    {
        this.result = result;
    }
    public void setDescribe(string describe)
    {
        this.describe = describe;
    }
    public void setTime()
    {
        this.time = DateTime.Now.ToString();
    }
    public void setState(int state)
    {
        this.state = state;
    }
    public void setMaxTime(int timeLimit)
    {
        this.maxTime = timeLimit;
    }
    public void setMaxMemory(int memory)
    {
        this.maxMemory = memory;
    }*/


    public class _TestCase
    {
        public string input
        {
            get;
            private set;
        }
        public string output
        {
            get;
            private set;
        }
        public _TestCase() { }
        public _TestCase(string input, string output)
        {
            this.input = input;
            this.output = output;
        }
    }
    class JudgeDao
    {
        public JudgeDao() { }
        public string connectString;
        private void ConnectDb()
        {
            //string connectString = "Database=OJ_DB;Server=192.168.109.230;Integrated Security=True;uid=sa;pwd=sa";
            connectString = "Data Source=192.168.109.230;Initial Catalog=OJ_DB;User ID=sa;Password=sa";
            //sqlCnt = new SqlConnection(connectString);
        }
        /*private void DisConnectDb()
        {
            sqlCnt.Close();
            sqlCnt.Dispose();
        }*/
        public void UpdateSubmittedCode(Result r)//  int type,string describe,string time
        {
            //string connectString = "Data Source=192.168.109.230;Initial Catalog=OJ_DB;User ID=sa;Password=sa";
            ConnectDb();
            using (var conn = new SqlConnection(connectString))
            {
                conn.Open();
                //update program database
                string MyCommand = "update SubmittedCode set result='" + r.result + "',describe='" + r.describe + "',runTime=" + r.time + ",memory=" + r.memory + "where codeID=" + r.codeID;

                using (SqlCommand command = new SqlCommand(MyCommand, conn))
                {
                    command.ExecuteNonQuery();
                }
            }

        }
        public List<_SubmittedCode> QuerySubmittedCodeList()
        {
            List<_SubmittedCode> sc = new List<_SubmittedCode>();
            //string connectString = "Data Source=192.168.109.230;Initial Catalog=OJ_DB;User ID=sa;Password=sa";
            ConnectDb();
            bool temp = false;
            using (var conn = new SqlConnection(connectString))
            {
                string MyCommand = "select top 10 p.*,q.* from SubmittedCode p,Question q where state=0";
                using (SqlCommand command = new SqlCommand(MyCommand, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())//return a 'stream'
                    {
                        while (reader.Read())
                        {
                            temp = true;
                            _SubmittedCode smc = new _SubmittedCode((int)reader["CodeID"], (int)reader["QuestionID"], (string)reader["Code"]);
                            string sql = "update SubmittedCode set state=1 where codeID=" + smc.CodeID;
                            using (SqlCommand comm = new SqlCommand(sql, conn))
                            {
                                comm.ExecuteNonQuery();
                            }
                            sc.Add(smc);
                            //pg.setProgramID(reader.GetInt32(reader.GetOrdinal("programID")));
                            /*pg.setProgramID((int)reader["programID"]);
                            pg.setProblemID((int)reader["problemID"]);
                            pg.setProgramCode((string)reader["programCode"]);
                            pg.setMaxTime((int)reader["timeLimit"]);
                            pg.setMaxMemory((int)reader["memoryLimit"]);*/
                            //pg.setTime();
                            //pg.setState(1);
                        }
                        if (!temp)
                        {
                            return null;
                        }
                    }
                }
                /*MyCommand = "update program set STATE=1 WHERE PROGRAMID=" + pg.getProgramID();
                using (SqlCommand command = new SqlCommand(MyCommand, conn))
                {
                    command.ExecuteNonQuery();
                }*/
            }
            return sc;
        }
        public List<_TestCase> QueryTestCaseList(int qid)
        {
            //string connectString = "Data Source=192.168.109.230;Initial Catalog=OJ_DB;User ID=sa;Password=sa";
            ConnectDb();
            List<_TestCase> tc = new List<_TestCase>();
            using (var conn = new SqlConnection(connectString))
            {
                string MyCommand = "select *from TestCase where questionID=" + qid;

                using (SqlCommand command = new SqlCommand(MyCommand, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())//return a 'stream'
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                _TestCase io = new _TestCase((string)reader["input"], (string)reader["output"]);
                                tc.Add(io);
                            }
                        }
                    }
                }
            }
            return tc;
        }
        public _Question FindQuestionById(int qid)
        {
            ConnectDb();
            using (var conn = new SqlConnection(connectString))
            {
                string MyCommand = "select * from Question where questionID=" + qid;
                using (SqlCommand command = new SqlCommand(MyCommand, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())//return a 'stream'
                    {
                        if (reader.Read())
                        {
                            _Question q = new _Question((int)reader["questionID"], (int)reader["maxTime"], (int)reader["maxMemory"]);
                            return q;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
    }
}
