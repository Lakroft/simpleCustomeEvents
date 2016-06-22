using UnityEngine;
using System.Collections;

public static class MProjectInfo {

    public static string[] availableLanguages;

    public static string[] resolutions;

    public static string bannerMethod;

    public struct banPos
    {
        public string x;
        public string y;
    }

    public static banPos bannerPosition;

    public static string orientation;

    public static bool _loaded;

}