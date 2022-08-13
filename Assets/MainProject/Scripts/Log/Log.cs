using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using BlackTree.Common;

public class Log : MonoSingleton<Log>
{
    static FileStream _fileLog;
    static StreamWriter _writer;
    string _logPath;
    const string _logFilename = "Log";

    public void Setup()
    {
        _logPath = Application.dataPath + "/Log";

        if (!System.IO.Directory.Exists(_logPath))
            System.IO.Directory.CreateDirectory(_logPath);

        string fullpath = MakeFullPath(_logFilename, DateTime.Now.ToString("yyyyMMdd_HHmmss"));
        _fileLog = new FileStream(fullpath, FileMode.Append);
        _writer = new StreamWriter(_fileLog);
    }

    protected override void Release()
    {
        base.Release();
        if (_writer != null)
            _writer.Close();

        if (_fileLog != null)
            _fileLog.Close();
    }

    public void log(string logmsg)
    {
        if (_writer != null)
            _writer.WriteLine(logmsg);
    }


    string MakeFullPath(string fileName, string dateTimeString)
    {
        return string.Format("{0}/{1}_{2}.txt", _logPath, fileName, dateTimeString);
    }

}


