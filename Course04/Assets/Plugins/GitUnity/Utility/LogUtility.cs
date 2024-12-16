using System;
using System.IO;
using UnityEngine;

namespace GitUnity.Utility
{
    public static class LogUtility
    {
        private static string logFilePath = Path.Combine(Application.persistentDataPath, "GitUnityLogs.txt");

        /// <summary>
        /// ログを追記
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            string logMessage = $"[{DateTime.Now}] {message}";
            Debug.Log(logMessage);

            // ファイルに追記
            try
            {
                File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to write log to file: {ex.Message}");
            }
        }

        /// <summary>
        /// エラーログを追記
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(string message)
        {
            string logMessage = $"[{DateTime.Now}] ERROR: {message}";
            Debug.LogError(logMessage);

            // ファイルに追記
            try
            {
                File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to write error log to file: {ex.Message}");
            }
        }

        /// <summary>
        /// ログが保存されているファイルパスを返す
        /// </summary>
        /// <returns></returns>
        public static string GetLogFilePath()
        {
            return logFilePath;
        }
    }
}
