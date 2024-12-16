using System.Diagnostics;

namespace GitUnity.Utility
{
    public class CommandRunner
    {
        /// <summary>
        /// コマンドプロンプトでコマンドを実行
        /// </summary>
        /// <param name="command"></param>
        public void RunCommand(string dir, string command, bool isWindow)
        {
            // プロセス設定
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe", // コマンドプロンプトを使用
                WorkingDirectory = dir,
                Arguments = $"/k {command}", // コマンド実行
                RedirectStandardOutput = !isWindow, // 標準出力をリダイレクト
                RedirectStandardError = !isWindow, // 標準エラーもリダイレクト
                UseShellExecute = isWindow, // シェル実行を無効に
                CreateNoWindow = !isWindow // ウィンドウ非表示
            };

            Process process = new Process
            {
                StartInfo = processStartInfo
            };

            process.Start();

            // 標準出力と標準エラーの読み取り
            if (isWindow) return;
            string output = process.StandardOutput.ReadToEnd();
            string errorOutput = process.StandardError.ReadToEnd();

            process.WaitForExit();
            process.Close();

            if (!string.IsNullOrEmpty(errorOutput))
            {
                if (errorOutput.Contains("fatal") || errorOutput.Contains("error"))
                {
                    LogUtility.LogError(errorOutput);
                }
            }
        }
    }
}
