using UnityEngine;
using System.IO;
using System;

/// <summary>
/// 파일 로그 시스템 - 유니티 실행할 때마다 새로 작성됨
/// </summary>
public static class DebugLogger
{
        private static string _logFilePath;
        private static bool _initialized = false;

        static DebugLogger()
        {
            Initialize();
        }

        private static void Initialize()
        {
            if (_initialized) return;

            // 프로젝트 폴더에 로그 파일 생성
            _logFilePath = Path.Combine(Application.dataPath, "..", "debug_log.txt");

            // 새 파일로 시작 (기존 내용 삭제)
            File.WriteAllText(_logFilePath, $"=== 디버그 로그 시작: {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===\n\n");

            _initialized = true;
            Log("DebugLogger 초기화 완료");
        }

        /// <summary>
        /// 로그 기록 (콘솔 + 파일)
        /// </summary>
        public static void Log(string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            string logMessage = $"[{timestamp}] {message}";

            Debug.Log(logMessage);
            WriteToFile(logMessage);
        }

        /// <summary>
        /// 경고 로그
        /// </summary>
        public static void LogWarning(string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            string logMessage = $"[{timestamp}] [WARNING] {message}";

            Debug.LogWarning(logMessage);
            WriteToFile(logMessage);
        }

        /// <summary>
        /// 에러 로그
        /// </summary>
        public static void LogError(string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            string logMessage = $"[{timestamp}] [ERROR] {message}";

            Debug.LogError(logMessage);
            WriteToFile(logMessage);
        }

        /// <summary>
        /// 스택트레이스 포함 로그
        /// </summary>
        public static void LogWithStackTrace(string message)
        {
            string stackTrace = StackTraceUtility.ExtractStackTrace();
            Log($"{message}\n스택트레이스:\n{stackTrace}");
        }

        private static void WriteToFile(string message)
        {
            try
            {
                File.AppendAllText(_logFilePath, message + "\n");
            }
            catch (Exception e)
            {
                Debug.LogError($"로그 파일 쓰기 실패: {e.Message}");
            }
        }

    /// <summary>
    /// 로그 파일 경로 반환
    /// </summary>
    public static string GetLogFilePath()
    {
        return _logFilePath;
    }
}
