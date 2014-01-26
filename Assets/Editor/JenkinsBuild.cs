using UnityEditor;
using System.Collections.Generic;
using System;

class JenkinsBuild {	
	static string APP_NAME = "Dead End Alley";
	static string TARGET_DIR = "targets";
	enum BuildType {Win32, MacOSX, iOS, Android, Linux, Web};
	
	[MenuItem ("Custom/CI/Build Mac OS X")]
	static void PerformMacOSX () {
		BuildTarget(BuildType.MacOSX);
	}

	[MenuItem ("Custom/CI/Build Win32")]
	static void PerformWin32 () {
		BuildTarget(BuildType.Win32);
	}

	[MenuItem ("Custom/CI/Build iOS")]
	static void PerformiOS () {
		BuildTarget(BuildType.iOS);
	}

	[MenuItem ("Custom/CI/Build Android")]
	static void PerformAndroid () {
		BuildTarget(BuildType.Android);
	}
	
	[MenuItem ("Custom/CI/Build WebPlayer")]
	static void PerformWeb () {
		BuildTarget(BuildType.Web);
	}

	private static string[] FindEnabledEditorScenes()
	{
		List<string> EditorScenes = new List<string>();
		foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
			if (!scene.enabled) continue;
			EditorScenes.Add(scene.path);
		}
		return EditorScenes.ToArray();
	}

	private static void ParseCommandLine() {
		string[] args = System.Environment.GetCommandLineArgs();
		APP_NAME = args[args.Length - 1];
	}

	private static void BuildLightmaps() {
		Lightmapping.Bake();
	}

	static void BuildTarget(BuildType target) {
		string[] scenes = FindEnabledEditorScenes();
		ParseCommandLine();
		if (!System.IO.Directory.Exists(TARGET_DIR)) {
			System.IO.Directory.CreateDirectory(TARGET_DIR);
		}
		string target_dir = null;
		string output = null;
		string extension = null;
		BuildTarget unity_target = 0;
		BuildOptions unity_options = BuildOptions.None;
		switch (target) {
			case BuildType.MacOSX:
				target_dir = "MacOSX";
				unity_target = UnityEditor.BuildTarget.StandaloneOSXIntel;
				extension = ".app";
				break;
			case BuildType.Win32:
				target_dir = "Win32";
				unity_target = UnityEditor.BuildTarget.StandaloneWindows;
				extension = ".exe";
				break;
			case BuildType.Android:
				target_dir = "Android";
				unity_target = UnityEditor.BuildTarget.Android;
				extension = ".apk";
				break;
			case BuildType.Linux:
				target_dir = "Linux";
				break;
			case BuildType.iOS:
				target_dir = "iOS";
				unity_target = UnityEditor.BuildTarget.iPhone;
				break;
			case BuildType.Web:
				target_dir = "WebPlayer";
				unity_target = UnityEditor.BuildTarget.WebPlayer;
				break;
		}
		if (System.IO.Directory.Exists(TARGET_DIR + "/" + target_dir)) {
			System.IO.Directory.Delete(TARGET_DIR + "/" + target_dir, true);
		}
		System.IO.Directory.CreateDirectory(TARGET_DIR + "/" + target_dir);

		output = TARGET_DIR + "/" + target_dir + "/" + APP_NAME;
		if (extension != null) {
			output += extension;
		}
		EditorUserBuildSettings.SwitchActiveBuildTarget(unity_target);
		string res = BuildPipeline.BuildPlayer(scenes, output, unity_target, unity_options);
		if (res.Length > 0) {
			throw new Exception("Build Failure: " + res);
		}
	}
}
