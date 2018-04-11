﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace Traicy.GUI.Logic
{
    public class PythonConnector
    {
        [Obsolete("IronPython isn't compatible with version 3.6 of Python and can't be used with TensorFlow either.")]
        public void ExecutePythonScript()
        {
            try
            {
                var engine = Python.CreateEngine(); 
                var paths = engine.GetSearchPaths();
                //add packages to import all needed modules for IronPython
                paths.Add(@"C:\Users\Eva\Anaconda3\envs\customTFLearn\Lib"); //add python script files to the ironPython paths
                paths.Add(@"C:\Users\Eva\Anaconda3\envs\customTFLearn\Lib\site-packages"); //add all side packages and modules e.g. numpy, skimage to ironPython paths
                engine.SetSearchPaths(paths);
                var scope = engine.CreateScope();

                var source = engine.CreateScriptSourceFromFile(@"../Debug/filters/Test.py");
                source.Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        [Obsolete("IronPython isn't compatible with version 3.6 of Python and can't be used with TensorFlow either.")]
        public void ExecutePythonScript2()
        {
            var ironPythonRuntime = Python.CreateRuntime();
            //string directory = System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString();
            dynamic loadIPython = ironPythonRuntime.UseFile(@"../filters/Test.py");
            var prediction = loadIPython.test(); //call specific function
        }

        public string GetPrediction(string absoluteImagePath)
        {
            //TODO: als Ressourcen-String verwenden
            //string pythonScriptFilePath = @"C:\Users\Eva\Documents\GitHub\TensorFlow\traicy\gui\Traicy.GUI\bin\Debug\filters\Test2.py";
            string pythonScriptFilePath = @"py_scripts\predict_number.py"; //TODO: importierte Python-Module werden nicht erkannt
            
            var result = StartPythonProcess(pythonScriptFilePath, absoluteImagePath);
            if (!string.IsNullOrEmpty(result))
            {
                var parsedString = PythonOutputParser.Parse(result);
                var letter = parsedString[0];
                var probability = parsedString[1];
                var prediction = $"The letter is {letter} with a probability of {probability}";
                return prediction;
            }
            return "prediction";
        }

        private string StartPythonProcess(string command, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = @"C:\Users\Eva\Anaconda3\envs\customEnv\python.exe",
                Arguments = $"\"{command}\" \"{args}\"",
                UseShellExecute = false, // don't use windows cmd
                CreateNoWindow = true,
                RedirectStandardOutput = true, // Any output, generated by application will be redirected back
                RedirectStandardError = true // Any error in standard output will be redirected back (for example exceptions)
            };

            var test = start.WorkingDirectory;

            using (Process process = Process.Start(start))
            {
                try
                {
                    if (process != null)
                    {
                        process.WaitForExit();
                        using (StreamReader reader = process.StandardOutput)
                        {
                            string stderr =
                                process.StandardError.ReadToEnd(); // Here are the exceptions from our Python script
                            string result = reader.ReadToEnd(); // Here is the result of StdOut(for example: print "test")
                            return result;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Log(e.Message);
                }
            }
            return string.Empty;
        }
    }
}