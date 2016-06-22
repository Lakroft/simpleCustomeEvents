using UnityEngine;
using System.Collections;
using System;

public enum TypeJson
{
	ProjectInfo,
	BuildParams
}

public class MParseJsonConfig : MonoBehaviour
{
	const string VERSION = "1.0.3";
    public bool printDebug = false;
	public static bool projectInfoError;
	public static bool buildParamsError;


	public static bool parsingBuildParams = false;
	public static bool parsingProjectInfo = false;

	static public MParseJsonConfig instance = null;	

    //public void Awake()
    //{
    //    if (!MBuildParams._loaded)
    //    {
    //        if (printDebug) Debug.Log("am_builds.txt coroutine started");
    //        StartCoroutine("CoroutineOpenBuildParams");
    //    }
    //    if (!MProjectInfo._loaded)
    //    {
    //        if (printDebug) Debug.Log("am_project.txt coroutine started");
    //        StartCoroutine("CoroutineOpenProjectInfo");
    //    }
    //}

	void Start()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
			StartCoroutine (CoroutineOpenBuildParams ());
			StartCoroutine (CoroutineOpenProjectInfo ());
		}
		else
		{
		//	Destroy(gameObject);
		}
	}
	void Update()
	{
		//Debug.Log ("update");	
	}

    public  IEnumerator CoroutineOpenProjectInfo()
    {
		parsingProjectInfo = true;
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "am_project.txt");
        string text = "";
        if (filePath.Contains("://"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            text = www.text;
            parseProjectInfo(text);
        }
        else
        {
            #if !UNITY_METRO || UNITY_EDITOR
            using (System.IO.StreamReader reader = System.IO.File.OpenText(filePath))
            {
                text = reader.ReadToEnd();
            }
            parseProjectInfo(text);
            #else
            byte[] byteArray =  UnityEngine.Windows.File.ReadAllBytes(filePath);
            char[] charArray = System.Text.UTF8Encoding.UTF8.GetChars(byteArray);
            text = new string(charArray);
            parseProjectInfo(text);
            #endif
        }
    }

	static public void ParseConfigFile(TypeJson type,Action<bool> CallBack)
	{
		if (type == TypeJson.BuildParams) 
		{
			if(MBuildParams._loaded)
				CallBack(false);
			else
			{
				MParseJsonConfig.onBuildParamsParsed += CallBack;
				new GameObject ().AddComponent<MParseJsonConfig> ();
			}
		}
		if (type == TypeJson.ProjectInfo) 
		{
			if(MProjectInfo._loaded)
				CallBack(false);
			else
			{
				MParseJsonConfig.onProjectInfoParsed += CallBack;
				new GameObject ().AddComponent<MParseJsonConfig> ();
			}
		}
	
	
	}


    public IEnumerator CoroutineOpenBuildParams()
    {
		parsingBuildParams = true;
		
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "am_builds.txt");
        string text = "";

        if (filePath.Contains("://"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            text = www.text;
            parseBuildParams(text);
        }
        else
        {
            #if !UNITY_METRO || UNITY_EDITOR
            using (System.IO.StreamReader reader = System.IO.File.OpenText(filePath))
            {
                text = reader.ReadToEnd();
            }
            parseBuildParams(text);
            #else
            byte[] byteArray =  UnityEngine.Windows.File.ReadAllBytes(filePath);
            char[] charArray = System.Text.UTF8Encoding.UTF8.GetChars(byteArray);
            text = new string(charArray);
            parseBuildParams(text);
            #endif
        }
    }

	public delegate void ParserDelegate(bool parserError);

	public event ParserDelegate ProjectInfoParsed;
	public event ParserDelegate BuildParamsParsed;

	public static event Action<bool> onProjectInfoParsed;
	public static event Action<bool> onBuildParamsParsed;

    void parseProjectInfo(string text) {
        
        JSONObject o = new JSONObject(text);
		projectInfoError = false;
        if (o.HasField("project_info")) {
            JSONObject projectInfo = o.GetField("project_info");

            if (printDebug) Debug.Log("am_project.txt parse:");

            if (projectInfo.HasField("available_languages")) {
                JSONObject availableLanguages = projectInfo.GetField("available_languages");
				if (availableLanguages.IsArray) {
                	if (availableLanguages.Count == 0) {
						MProjectInfo.availableLanguages = new string[0];
						//Debug.LogError("MProjectInfo.availableLanguages = Empty field!");
						//projectInfoError = true;
						//Application.Quit();
					}
					else if (availableLanguages.Count < 0) {

                    		MProjectInfo.availableLanguages = new string[1];
						if (availableLanguages[0].IsString) {
                    		MProjectInfo.availableLanguages[0] = availableLanguages.str;
                    		if (printDebug) Debug.Log("MProjectInfo.availableLanguages = " + MProjectInfo.availableLanguages[0]);
						}
						else {
							Debug.LogError("MProjectInfo.availableLanguages = Not a string!");
							projectInfoError = true;
							Application.Quit();
						}
                	}
                	else {
                    	MProjectInfo.availableLanguages = new string[availableLanguages.Count];
                    	for (int i = 0; i < availableLanguages.Count; i++) {
                        	MProjectInfo.availableLanguages[i] = availableLanguages[i].str;
                        	if (printDebug) Debug.Log("MProjectInfo.availableLanguages["+i+"] = " + MProjectInfo.availableLanguages[i]);
                    	}
                	}
            	}
				else {
					Debug.LogError("MProjectInfo.availableLanguages = Incorrect input type!");
					projectInfoError = true;
					Application.Quit();
				}
			}
			else {
				Debug.LogError("MProjectInfo.availableLanguages = Not found!");
				projectInfoError = true;
				Application.Quit();
			}

            if (projectInfo.HasField("banner_method")) {
                JSONObject bannerMethod = projectInfo.GetField("banner_method");
				if (bannerMethod.IsString) {
                	MProjectInfo.bannerMethod = bannerMethod.str;
					if (MProjectInfo.bannerMethod != "") {
						if (printDebug) Debug.Log("MProjectInfo.bannerMethod = " + MProjectInfo.bannerMethod);
					}
					else {
						Debug.LogError("MProjectInfo.bannerMethod = Empty field!");
						projectInfoError = true;
						Application.Quit();
					}
				}
				else {
					Debug.LogError("MProjectInfo.bannerMethod = Incorrect input type!");
					projectInfoError = true;
					Application.Quit();
				}
            }
			else {
				Debug.LogError("MProjectInfo.bannerMethod = Not found!");
				projectInfoError = true;
				Application.Quit();
			}

            if (projectInfo.HasField("banner_position")) {
                JSONObject bannerPosition = projectInfo.GetField("banner_position");
                if (bannerPosition.HasField("x")) {
                    JSONObject bannerPositionX = bannerPosition.GetField("x");
					if (bannerPositionX.IsString) {
                    	MProjectInfo.bannerPosition.x = bannerPositionX.str;
						if (MProjectInfo.bannerPosition.x != "") {
                    		if (printDebug) Debug.Log("MProjectInfo.bannerPosition.x = " + MProjectInfo.bannerPosition.x);
						}
						else {
							Debug.LogError("MProjectInfo.bannerPosition.x = Empty field!");
							projectInfoError = true;
							Application.Quit();
						}
					}
					else {
						Debug.LogError("MProjectInfo.bannerPosition.x = Incorrect input type!");
						projectInfoError = true;
						Application.Quit();
					}
                }
				else {
					Debug.LogError("MProjectInfo.bannerPosition.x = Not found!");
					projectInfoError = true;
					Application.Quit();
				}
                if (bannerPosition.HasField("y")) {
                    JSONObject bannerPositionY = bannerPosition.GetField("y");
					if (bannerPositionY.IsString) {
                    	MProjectInfo.bannerPosition.y = bannerPositionY.str;
						if (MProjectInfo.bannerPosition.y != "") {
                    		if (printDebug) Debug.Log("MProjectInfo.bannerPosition.y = " + MProjectInfo.bannerPosition.y);
						}
						else {
							Debug.LogError("MProjectInfo.bannerPosition.y = Empty field!");
							projectInfoError = true;
							Application.Quit();
						}
					}
					else {
						Debug.LogError("MProjectInfo.bannerPosition.y = Incorrect input type!");
						projectInfoError = true;
						Application.Quit();
					}
                }
				else {
					Debug.LogError("MProjectInfo.bannerPosition.y = Not found!");
					projectInfoError = true;
					Application.Quit();
				}
            }
			else {
				Debug.LogError("MProjectInfo.bannerPosition = Not found!");
				projectInfoError = true;
				Application.Quit();
			}

            if (projectInfo.HasField("orientation")) {
                JSONObject orientation = projectInfo.GetField("orientation");
				if (orientation.IsString) {
                	MProjectInfo.orientation = orientation.str;
					if (MProjectInfo.orientation != "") {
                		if (printDebug) Debug.Log("MProjectInfo.orientation = " + MProjectInfo.orientation);
					}
					else {
						Debug.LogError("MProjectInfo.orientation = Empty field!");
						projectInfoError = true;
						Application.Quit();
					}
				}
				else {
					Debug.LogError("MProjectInfo.orientation = Incorrect input type!");
					projectInfoError = true;
					Application.Quit();
				}
            }
			else {
				Debug.LogError("MProjectInfo.orientation = Not found!");
				projectInfoError = true;
				Application.Quit();
			}

            if (projectInfo.HasField("resolutions")) {
                JSONObject resolutions = projectInfo.GetField("resolutions");
				if (resolutions.IsArray) {
					if (resolutions.Count == 0) {
                    	MProjectInfo.resolutions = new string[0];
						
						//Debug.LogError("MProjectInfo.resolutions = Empty field!");
						//projectInfoError = true;
						//Application.Quit();
					}
					else if (resolutions.Count < 0) {
                    	MProjectInfo.resolutions = new string[1];
                    	MProjectInfo.resolutions[0] = resolutions.str;
                    	if (printDebug)	Debug.Log("MProjectInfo.resolutions = " + MProjectInfo.resolutions[0]);
						Application.Quit();
                	}
                	else {
                    	MProjectInfo.resolutions = new string[resolutions.Count];
                    	for (int i = 0; i < resolutions.Count; i++) {
                        	MProjectInfo.resolutions[i] = resolutions[i].str;
                        	if (printDebug) Debug.Log("MProjectInfo.resolutions["+i+"] = " + MProjectInfo.resolutions[i]);
                    	}
                	}
				}
				else {
					Debug.LogError("MProjectInfo.resolutions = Incorrect input type!");
					projectInfoError = true;
					Application.Quit();
				}
            }
			else {
				Debug.LogError("MProjectInfo.resolutions = Not found!");
				projectInfoError = true;
				Application.Quit();
			}			            
        }
		else {
			Debug.LogError("project_info field not found!");
			projectInfoError = true;
			Application.Quit();
		}
		if (projectInfoError != true) {
			if (ProjectInfoParsed != null) {
				ProjectInfoParsed(projectInfoError);
				parsingProjectInfo = false;
			}

			if (onProjectInfoParsed != null) {
				onProjectInfoParsed(projectInfoError);
				parsingProjectInfo = false;
			}
			if (printDebug) Debug.Log("MProjectInfo was parsed successfully!");
			MProjectInfo._loaded = true;
		}
		else {
			Debug.LogError("MProjectInfo parsing was finished with error(s)!");
			MProjectInfo._loaded = false;
		}
    }

    void parseBuildParams(string text) {

        JSONObject o = new JSONObject(text);
		buildParamsError = false;
        if (o.HasField("build_params")) {
            JSONObject buildParams = o.GetField("build_params");
			if (printDebug) Debug.Log("am_builds.txt parse:");

            if (buildParams[0].HasField("language")) {
                JSONObject language = buildParams[0].GetField("language");
				if (language.IsArray) {
					if (language.Count == 0) {
                    	MBuildParams.language = new string[1];
						
						//Debug.LogError("MProjectInfo.language = Empty field!");
						//buildParamsError = true;
						//Application.Quit();
					}
					else if (language.Count < 0) {
                    	MBuildParams.language = new string[1];
                    	MBuildParams.language[0] = language.str;
                    	if (printDebug) Debug.LogWarning("MBuildParams.language = " + MBuildParams.language[0]);
						Application.Quit();
                	}
                	else {
                    	MBuildParams.language = new string[language.Count];
                    	for (int i = 0; i < language.Count; i++) {
                        	MBuildParams.language[i] = language[i].str;
                        	if (printDebug) Debug.Log("MBuildParams.language[" + i + "] = " + MBuildParams.language[i]);
                    	}
                	}
				}
				else {
					Debug.LogError("MProjectInfo.language = Incorrect input type!");
					buildParamsError = true;
					Application.Quit();
				}
            }
			else {
				Debug.LogError("MBuildParams.language = Not found!");
				buildParamsError = true;
				Application.Quit();
			}

            if (buildParams[0].HasField("bundle")) {
                JSONObject bundle = buildParams[0].GetField("bundle");
				if (bundle.IsString) {
                	MBuildParams.bundle = bundle.str;
					if (MBuildParams.bundle != "") {
                		if (printDebug) Debug.Log("MBuildParams.bundle = " + MBuildParams.bundle);
					}
					else {
						Debug.LogError("MBuildParams.bundle = Empty field!");
						buildParamsError = true;
						Application.Quit();
					}
				}
				else {
					Debug.LogError("MBuildParams.bundle = Incorrect input type!");
					buildParamsError = true;
					Application.Quit();
				}
            }
			else {
				Debug.LogError("MBuildParams.bundle = Not found!");
				buildParamsError = true;
				Application.Quit();
			}

            if (buildParams[0].HasField("innerID")) {
                JSONObject innerID = buildParams[0].GetField("innerID");
				if (innerID.IsString) {
                	MBuildParams.innerID = innerID.str;
					if (MBuildParams.innerID != "") {
                		if (printDebug) Debug.Log("MBuildParams.innerID = " + MBuildParams.innerID);
					}
					else {
						Debug.LogError("MBuildParams.innerID = Empty field!");
						buildParamsError = true;
						Application.Quit();
					}
				}
				else {
					Debug.LogError("MBuildParams.innerID = Incorrect input type!");
					buildParamsError = true;
					Application.Quit();
				}
            }
			else {
				Debug.LogError("MBuildParams.innerID = Not found!");
				buildParamsError = true;
				Application.Quit();
			}

            if (buildParams[0].HasField("platform")) {
                JSONObject platform = buildParams[0].GetField("platform");
				if (platform.IsString) {
                	MBuildParams.platform = platform.str;
					if (MBuildParams.platform != "") {
                		if (printDebug) Debug.Log("MBuildParams.platform = " + MBuildParams.platform);
					}
					else {
						Debug.LogError("MBuildParams.platform = Empty field!");
						buildParamsError = true;
						Application.Quit();
					}
				}
				else {
					Debug.LogError("MBuildParams.platform = Incorrect input type!");
					buildParamsError = true;
					Application.Quit();
				}
            }
			else {
				Debug.LogError("MBuildParams.platform = Not found!");
				buildParamsError = true;
				Application.Quit();
			}

            if (buildParams[0].HasField("payment")) {
                JSONObject payment = buildParams[0].GetField("payment");
				if (payment.IsString) {
                	MBuildParams.payment = payment.str;
					if (MBuildParams.payment != "") {
                		if (printDebug) Debug.Log("MBuildParams.payment = " + MBuildParams.payment);
					}
					else {
						Debug.LogError("MBuildParams.payment = Empty field!");
						buildParamsError = true;
						Application.Quit();
					}
				}
				else {
					Debug.LogError("MBuildParams.payment = Incorrect input type!");
					buildParamsError = true;
					Application.Quit();
				}
            }
			else {
				Debug.LogError("MBuildParams.payment = Not found!");
				buildParamsError = true;
				Application.Quit();
			}

			if (buildParams[0].HasField("unlock")) {
                JSONObject unlock = buildParams[0].GetField("unlock");
				if (unlock.IsBool) {
					MBuildParams.unlock = unlock.b;
					if (printDebug) Debug.Log("MBuildParams.unlock = " + MBuildParams.unlock);
				}			
				else {
					Debug.LogError("MBuildParams.unlock = Incorrect input type!");
					buildParamsError = true;
					Application.Quit ();
				}
			}
			else {
				Debug.LogError("MBuildParams.unlock = Not found!");
				buildParamsError = true;
				Application.Quit();
			}

            if (buildParams[0].HasField("has_banner")) {
                JSONObject hasBanner = buildParams[0].GetField("has_banner");
				if (hasBanner.IsBool) {
					MBuildParams.hasBanner = hasBanner.b;
					if (printDebug) Debug.Log("MBuildParams.hasBanner = " + MBuildParams.hasBanner);
				}
				else {
					Debug.LogError("MBuildParams.hasBanner = Incorrect input type!");
					buildParamsError = true;
					Application.Quit ();
				}
            }
			else {
				Debug.LogError("MBuildParams.hasBanner = Not found!");
				buildParamsError = true;
				Application.Quit();
			}
        }
		else {
			Debug.LogError("build_params field not found!");
			buildParamsError = true;
			Application.Quit();
		}
		if (buildParamsError != true) {
			if (printDebug) Debug.Log("MBuildParams was parsed successfully!");
			MBuildParams._loaded = true;
			if (BuildParamsParsed != null) {
				BuildParamsParsed(buildParamsError);
				parsingBuildParams = false;
			}
			if (onBuildParamsParsed != null) 
			{
				onBuildParamsParsed(buildParamsError);
				parsingBuildParams = false;
			}
		}
		else {
			Debug.LogError("MBuildParams parsing was finished with error(s)!");
			MBuildParams._loaded = false;
		}
    }
}