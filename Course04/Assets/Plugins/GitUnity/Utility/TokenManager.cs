using UnityEditor;

namespace GitUnity.Utility
{
    public class TokenManager
    {
        private const string TOKEN_KEY = "ACCESS_TOKEN_KEY";

        /// <summary>
        /// トークンを保存
        /// </summary>
        /// <param name="token"></param>
        public static void SaveToken(string token)
        {
            EditorPrefs.SetString(TOKEN_KEY, token);
        }

        /// <summary>
        /// トークンを取得
        /// </summary>
        /// <returns></returns>
        public static string GetToken()
        {
            return EditorPrefs.HasKey(TOKEN_KEY) ? EditorPrefs.GetString(TOKEN_KEY) : null;
        }

        /// <summary>
        /// トークンを削除
        /// </summary>
        public static void DeleteToken()
        {
            if (EditorPrefs.HasKey(TOKEN_KEY))
            {
                EditorPrefs.DeleteKey(TOKEN_KEY);
            }
        }
    }
}
