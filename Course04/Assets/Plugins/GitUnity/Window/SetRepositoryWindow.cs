using GitUnity.Repository;
using GitUnity.Utility;
using UnityEditor;
using UnityEngine;

namespace GitUnity.Editor
{
    public class SetRepositoryWindow : EditorWindow
    {
        [MenuItem("Tools/GitUnity/Git Repository Manager")]
        public static void ShowWindow()
        {
            // ツールウィンドウの作成と表示
            var window = GetWindow<SetRepositoryWindow>("Git Repository Manager");
            var newPosition = new Rect(Screen.width / 2, Screen.height / 2, 500, 250);
            window.position = newPosition;
        }

        private GitRepositoryManager gitRepoManager;
        private RepositorySettings repositorySettings;
        private string repoName = "New_Repository"; // 作成するリポジトリ名
        private string repoDescription = "Description"; // 作成するレポジトリの説明
        private bool isPublic; // レポジトリの公開設定
        private const string REPO_SETTINGS_PATH = "Assets/Plugins/GitUnity/RepositorySettings.asset";


        private void OnEnable()
        {
            gitRepoManager = new GitRepositoryManager(new CommandRunner());

            repositorySettings = AssetDatabase.LoadAssetAtPath<RepositorySettings>(REPO_SETTINGS_PATH);
            if (repositorySettings == null)
            {
                repositorySettings = ScriptableObject.CreateInstance<RepositorySettings>();
                AssetDatabase.CreateAsset(repositorySettings, REPO_SETTINGS_PATH);
            }
            else
            {
                repoName = repositorySettings.name;
                repoDescription = repositorySettings.description;
                isPublic = repositorySettings.@private;
            }
        }

        private void OnGUI()
        {
            GUILayout.Label("Git Repository Manager", EditorStyles.boldLabel);

            // リモートレポジトリの設定画面に行く
            if (GUILayout.Button("User Settings"))
            {
                UserSettingsWindow.ShowWindow();
            }

            // ユーザー入力フィールド
            repoName = EditorGUILayout.TextField("Repository Name", repoName);
            repoDescription = EditorGUILayout.TextField("Description", repoDescription);
            isPublic = EditorGUILayout.Toggle("Publish", isPublic);

            // ボタン押下時にリポジトリを作成
            if (GUILayout.Button("Create Repository"))
            {
                // レポジトリ設定を作成
                repositorySettings.name = repoName;
                repositorySettings.description = repoDescription;
                repositorySettings.@private = isPublic;

                AssetDatabase.SaveAssets();

                CreateGitRepo();
            }

            //ローカルレポジトリのディレクトリ選択
            if (GUILayout.Button("Select Local Repository Path"))
            {
                // フォルダ選択ダイアログを表示
                string selectedPath = EditorUtility.OpenFolderPanel("Select Local Repository Folder", "", "");
                // フォルダが選択された場合
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    gitRepoManager.LocalRepoPath = selectedPath;
                }
            }

            // 選択したローカルリポジトリのパスを表示
            EditorGUILayout.LabelField("Local Repository Path:", gitRepoManager.LocalRepoPath);

            // ローカルレポジトリの初期化
            if (GUILayout.Button("Initialize Local Repository"))
            {
                bool success = gitRepoManager.InitLocalRepo(repoName);
                if (success)
                {
                    EditorUtility.DisplayDialog("レポジトリの初期化", "レポジトリの初期化に成功しました。", "OK");
                }
                else
                {
                    EditorUtility.DisplayDialog("エラー", $"レポジトリの初期化に失敗しました。詳細はログファイル({LogUtility.GetLogFilePath()})を確認してください。", "OK");
                }
            }

            using (new GUILayout.HorizontalScope())
            {

                if (GUILayout.Button("Open GitHub Page"))
                {
                    gitRepoManager.OpenGithubPage(repoName);
                }

                if (GUILayout.Button("Open your cmd or GitHubDesktop"))
                {
                    int dialogIndex = EditorUtility.DisplayDialogComplex("Git操作へ", "コマンドプロンプトかGitHubDesktopを開きます", "コマンドプロンプトへ", "キャンセル", "GitHubDesktopへ");
                    switch (dialogIndex)
                    {
                        case 0:
                            gitRepoManager.OpenCommandPrompt();
                            break;

                        case 2:
                            gitRepoManager.OpenGitHubDesktop();
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// リモートレポジトリの作成をしたあとそのページに行く
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="userName"></param>
        /// <param name="repoName"></param>
        private async void CreateGitRepo()
        {
            bool success = await gitRepoManager.CreateRemoteRepo(REPO_SETTINGS_PATH);

            // 作成したレポジトリのページに飛ぶ
            if (success)
            {
                if (EditorUtility.DisplayDialog("レポジトリの作成", "レポジトリの作成に成功しました！\nGitHubのレポジトリページを開きますか？", "はい", "いいえ"))
                {
                    gitRepoManager.OpenGithubPage(repoName);
                }
            }
            else
            {
                EditorUtility.DisplayDialog("エラー", $"リポジトリの作成に失敗しました。詳細はログファイル({LogUtility.GetLogFilePath()})を確認してください。", "OK");
            }
        }
    }
}
