using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
namespace AKingCard
{
    public class LogManager : MonoBehaviour
    {
        private static FileStream mFileStream;
        private static StreamWriter mStreamWriter;
        private static StringBuilder mStringBuilder;
        private static DateTime mLaunchTime;
        private static string filePath;
        private static string fileName;
        private static string fullName;

        private const string LogStart = "The Start of This Log";
        private const string LogEnd = "The End of This Log";

        public void Awake()
        {
            mLaunchTime = DateTime.Now;
            if(Application.platform == RuntimePlatform.WindowsEditor)
            {
                int folderIndex = Application.dataPath.LastIndexOf("/");
                string folderPath = Application.dataPath.Substring(0, folderIndex);
                filePath = folderPath + "/" + "LogFiles";
            }
            else
                filePath = Application.streamingAssetsPath + "/LogFiles";

            fileName = "AKingCardTemplate-" + mLaunchTime.ToString("MMddyyyyHHmm") + ".log";
            fullName = $"{filePath}/{fileName}";

            mFileStream = CreateNewLog();
            mStringBuilder = new StringBuilder();
            mStreamWriter = new StreamWriter(mFileStream);

            WriteLog($"{LogStart} -- {fileName}");
            WriteLog($"Application version -- {Application.version}");

            PrintDeviceInfo();

        }
        private static void PrintDeviceInfo()
        {
            WriteLog("OperatingSystem: " + SystemInfo.operatingSystem);
            WriteLog("ProcessorType: " + SystemInfo.processorType);
            WriteLog("GraphicsDeviceName: " + SystemInfo.graphicsDeviceName);
            WriteLog("SystemMemorySize: " + SystemInfo.systemMemorySize);
            WriteLog("GraphicsMemorySize: " + SystemInfo.graphicsMemorySize);
        }
        public static void Log(string log)
        {
            Debug.Log(log);
            WriteLog(log);
        }
        public static void Error(string log)
        {
            Debug.LogError(log);
            WriteLog(log);
        }
        public static void Warning(string log)
        {
            Debug.LogWarning(log);
            WriteLog(log);
        }
        private static void WriteLog(string debugStr)
        {
            WriteLog(debugStr, string.Empty, LogType.Log);
        }
        private static void WriteLog(string debugStr, string stackTrace, LogType type)
        {
            try
            {
                mStringBuilder.Clear();
                mStringBuilder.Append(DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss:fff]"));
                mStringBuilder.Append($"[{type}]");
                mStringBuilder.Append(": ");
                mStringBuilder.AppendLine(debugStr);
                if (type == LogType.Error || type == LogType.Exception)
                    mStringBuilder.AppendLine(stackTrace);
                mStreamWriter.Write(mStringBuilder.ToString());
            }
            catch
            {
                mFileStream = CreateNewLog();
                mStringBuilder = new StringBuilder();
                mStreamWriter = new StreamWriter(mFileStream);
            }
            mStreamWriter?.Flush();
        }
        private static FileStream CreateNewLog()
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            FileStream fs;

            if (File.Exists(fullName))
            {
                fs = new FileStream(fullName, FileMode.Append, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(fullName, FileMode.Create, FileAccess.Write);
            }

            return fs;
        }
        public static void Destroy()
        {
        }
        private void OnApplicationQuit()
        {
            WriteLog($"{LogEnd} -- {fileName}");
            mStreamWriter.Close();
            mFileStream.Close();
            mStringBuilder.Clear();
        }
    }
}


