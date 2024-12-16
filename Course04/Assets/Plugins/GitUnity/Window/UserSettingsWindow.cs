using UnityEditor;
using UnityEngine;
using GitUnity.Utility;

namespace GitUnity.Editor
{
    public class UserSettingsWindow : EditorWindow
    {
        public static void ShowWindow()
        {
            // ツールウィンドウの作成と表示
            var window = CreateInstance<UserSettingsWindow>();
            window.ShowModal();
            window.titleContent = new GUIContent("User Settings");
            var windowPos = window.position;
            var newPosition = new Rect(windowPos.x, windowPos.y, 450, 200);
            window.position = newPosition;
        }


        private string userName = "User Name";
        private string accessToken = "";

        private void OnEnable()
        {
            // 起動時にすでにトークンが保存してあれば表示する
            string tokenValue = TokenManager.GetToken();
            if (tokenValue != null && tokenValue.Contains('%'))
            {
                var tokens = tokenValue.Split('%');
                userName = tokens[0];
                accessToken = tokens[1];
            }
        }

        private void OnGUI()
        {
            GUILayout.Label("UserSettings", EditorStyles.boldLabel);

            // GithubのAPIを入手できるサイトに飛ばす
            if (GUILayout.Button("Get Your GitHub API"))
            {
                Application.OpenURL("https://github.com/settings/tokens");
            }

            // ユーザー入力フィールド
            userName = EditorGUILayout.TextField("User Name", userName);
            accessToken = EditorGUILayout.PasswordField("Access Token", accessToken);

            using (new GUILayout.HorizontalScope())
            {
                // アクセストークンの保存
                if (GUILayout.Button("Save"))
                {
                    bool isUserNameValid = !string.IsNullOrEmpty(userName);
                    bool isAccessTokenValid = !string.IsNullOrEmpty(accessToken) && accessToken.Contains("ghp_");

                    if (isUserNameValid && isAccessTokenValid)
                    {
                        TokenManager.SaveToken($"{userName}%{accessToken}");
                        EditorUtility.DisplayDialog("設定の保存", "ユーザ名、アクセストークンが正しく保存されました。", "閉じる");
                        this.Close();
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("エラー", "無効なユーザ名、アクセストークンです。\nもう一度確認してください。", "OK");
                        LogUtility.LogError("Both User Name and a valid Access Token are required.");
                    }
                }

                if (GUILayout.Button("Close"))
                {
                    if (EditorUtility.DisplayDialog("確認", "保存せずに閉じますか？", "はい", "いいえ"))
                    {
                        this.Close();
                    }
                }
            }
        }

        private void OnDestroy()
        {
            var returnWindow = GetWindow<SetRepositoryWindow>();
            returnWindow.Focus();
        }
    }
}
