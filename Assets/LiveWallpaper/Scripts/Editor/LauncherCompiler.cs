#if UNITY_EDITOR_WIN
using System;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace LiveWallpaperCore.Launcher {
    public static class LauncherCompiler {

        public static void CompileLauncher(string pathToBuiltProject) {
            try {
                var search = AssetDatabase.FindAssets("LauncherSource");

                if(search.Length <= 0) {
                    Debug.LogError("Could not find LauncherSource.cs.");
                    return;
                }
                else if(search.Length > 1)
                    Debug.LogWarning("Duplicate LauncherSource.cs, make sure there's only one in the project.");

                var fileName = Path.GetFileName(pathToBuiltProject);
                var fileNameNoExt = Path.GetFileNameWithoutExtension(pathToBuiltProject);
                var folder = Path.GetDirectoryName(pathToBuiltProject);
                var tempPath = FileUtil.GetUniqueTempPathInProject();
                var source = AssetDatabase.GUIDToAssetPath(search[0]);
                var code = File.ReadAllText(source).Replace("GAME_NAME_HERE", fileName);

                File.WriteAllText(tempPath, code);

                CompileFile(Path.Combine(folder, fileNameNoExt + "_wallpaper.exe"), tempPath);
            }
            catch(Exception e) {
                Debug.LogWarning("Failed to compile wallpaper laucher.");
                Debug.LogException(e);
            }
        }

        [PostProcessBuild]
        private static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
            switch(target) {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    CompileLauncher(pathToBuiltProject);
                    break;
            }
        }

        private static void CompileFile(string outputPath, string sourceFile) {
            try {
                if(File.Exists(outputPath)) {
                    File.Delete(outputPath);
                    Debug.LogWarning("Deleted previous file in same path.");
                }

                sourceFile = Path.GetFullPath(sourceFile);

                using(var provider = new CSharpCodeProvider()) {
                    var option = new CompilerParameters() {
                        GenerateExecutable = true,
                        TempFiles = new TempFileCollection(Path.GetTempPath(), false),
                    };

                    var result = provider.CompileAssemblyFromFile(option, sourceFile);

#pragma warning disable IDE0007 // Use implicit type
                    foreach(CompilerError error in result.Errors)
                        if(error.IsWarning)
                            Debug.LogWarning(error);
                        else if(!error.ToString().Contains("(Location of the symbol related to previous warning)"))
                            Debug.LogError(error);
#pragma warning restore IDE0007 // Use implicit type

                    if(!File.Exists(result.PathToAssembly))
                        Debug.LogError("Failed to compile wallpaper launcher.");
                    else {
                        File.Move(result.PathToAssembly, outputPath);
                        Debug.Log("Wallpaper launcher compiled.");
                    }
                }
            }
            catch(Exception e) {
                Debug.LogException(e);
            }
        }

    }
}
#endif