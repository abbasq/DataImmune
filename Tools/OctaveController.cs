using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Threading;

namespace Tools
{
    public class OctaveController
    {
        Process OctaveProcess { get; set; }
        private string OctaveEchoString { get; set; }
        public OctaveController()
        {

        }
        public OctaveController(string PathToOctaveBinaries, string filename)
        {
            string errorMessage = StartOctave(PathToOctaveBinaries, filename, false);
            if (errorMessage != "")
            {
                Console.WriteLine(errorMessage);
                Console.ReadLine();
            }
        }

        public OctaveController(string PathToOctaveBinaries, string filename, bool CreateWindow)
        {
            string errorMessage = StartOctave(PathToOctaveBinaries, filename, CreateWindow);
            if (errorMessage != "")
            {
                Console.WriteLine(errorMessage);
                Console.ReadLine();
            }
        }
        string ptob;
        bool cw;
        public string StartOctave(string PathToOctaveBinaries, string filename, bool CreateWindow)
        {
            string errorMessage = "";
            ErrorMessages em = new ErrorMessages();
            ptob = PathToOctaveBinaries;
            cw = CreateWindow;
            this.OctaveEchoString = Guid.NewGuid().ToString();
            OctaveProcess = new Process();
            ProcessStartInfo pi = new ProcessStartInfo();
            if (PathToOctaveBinaries[PathToOctaveBinaries.Length - 1] != '\\')
                PathToOctaveBinaries = PathToOctaveBinaries + "\\";
            pi.FileName = PathToOctaveBinaries + filename;
            pi.RedirectStandardInput = true;
            pi.RedirectStandardOutput = true;
            pi.RedirectStandardError = true;
            pi.UseShellExecute = false;
            pi.CreateNoWindow = !CreateWindow;
            pi.Verb = "open";
            //
            pi.WorkingDirectory = ".";
            OctaveProcess.StartInfo = pi;

            try
            {
                OctaveProcess.Start();
                OctaveProcess.OutputDataReceived += new DataReceivedEventHandler(OctaveProcess_OutputDataReceived);
                OctaveProcess.BeginOutputReadLine();
                OctaveEntryText = ExecuteCommand(null);
            }
            catch (DirectoryNotFoundException)
            {                
                errorMessage = em.UnableToConnectToOctave(pi.FileName);
                return errorMessage;
            }
            catch (FileNotFoundException)
            {
                errorMessage = em.UnableToConnectToOctave(pi.FileName);
                return errorMessage;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                errorMessage = em.UnableToConnectToOctave(pi.FileName);
            }
            catch (Exception e)
            {
                errorMessage = em.GenericError(e.Message);
                return errorMessage;
            }

            return errorMessage;            
        }

        public double GetScalar(string scalar)
        {
            string rasp = ExecuteCommand(scalar, 30000);
            string val = rasp.Substring(rasp.LastIndexOf("\\") + 1).Trim();
            return double.Parse(val);
        }

        public double[] GetVector(string vector)
        {
            string rasp = ExecuteCommand(vector, 30000);
            string[] lines = rasp.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int i = 0;
            //Catching the next entry
            List<double> data = new List<double>();
            while (i != lines.Length)
            {
                string line = lines[i];
                if (line.Contains("through") || line.Contains("and"))
                {
                    i++;
                    line = lines[i];
                    string[] dataS = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int k = 0; k < dataS.Length; k++)
                    {
                        data.Add(double.Parse(dataS[k]));
                    }
                }
                i++;
            }
            //Special case in which we put all the results on a single line
            if (data.Count == 0)
            {
                string[] dataS = lines[lines.Length - 1].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (dataS.Length != 0)
                    for (int k = 0; k < dataS.Length; k++)
                    {
                        data.Add(double.Parse(dataS[k]));
                    }
            }
            return data.ToArray();
        }

        public double[][] GetMatrix(string matrix)
        {
            //Find the number of rows
            string rasp = ExecuteCommand(matrix + "(:,1)", 30000);
            string[] lines = rasp.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            double[][] mat = new double[lines.Length - 1][];
            for (int i = 0; i < mat.Length; i++)
            {
                mat[i] = GetVector(matrix + "(" + (i + 1) + ",:)");
            }
            return mat;
        }

        StringBuilder SharedBuilder = new StringBuilder();
        ManualResetEvent OctaveDoneEvent = new ManualResetEvent(false);
        public string OctaveEntryText { get; internal set; }

        public void WorkThread(object o)
        {
            string command = (string)o;
            SharedBuilder.Clear();
            OctaveDoneEvent.Reset();
            if (command != null)
            {
                OctaveProcess.StandardInput.WriteLine(command);
            }
            //ca sa avem referinta pentru output
            OctaveProcess.StandardInput.WriteLine("\"" + OctaveEchoString + "\"");
            OctaveDoneEvent.WaitOne();
        }
        public string ExecuteCommand(string command, int timeout)
        {
            //TODO: reintroduce process exit handling
            //if (OctaveProcess.HasExited)
            //{
            //    StartOctave(ptob, cw);
            //    if (OctaveRestarted != null) OctaveRestarted(this, EventArgs.Empty);
            //}
            exitError = false;

            Thread tmp = new Thread(new ParameterizedThreadStart(WorkThread));
            tmp.Start(command);

            if (!tmp.Join(timeout))
            {
                tmp.Abort();
                throw new Exception("Octave timeout");
            }
            if (exitError)
            {
                throw new Exception(errorMessage);
            }
            return SharedBuilder.ToString();
        }
        public string ExecuteCommand(string command)
        {
            Thread tmp = new Thread(new ParameterizedThreadStart(WorkThread));
            tmp.Start(command);

            tmp.Join();

            return SharedBuilder.ToString();
        }
        bool exitError = false;
        string errorMessage = null;
        void OctaveProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)
            {
                SharedBuilder.Clear();
                errorMessage = OctaveProcess.StandardError.ReadToEnd();
                SharedBuilder.Append("Octave has exited with the following error message: \r\n" + errorMessage);
                exitError = true;
                OctaveDoneEvent.Set();
                return;
            }
            if (e.Data.Trim() == "ans = " + OctaveEchoString)
                OctaveDoneEvent.Set();
            else
                SharedBuilder.Append(e.Data + "\r\n");
        }
        public event OctaveRestartedEventHandler OctaveRestarted;
        public delegate void OctaveRestartedEventHandler(object sender, EventArgs e);

    }
}

