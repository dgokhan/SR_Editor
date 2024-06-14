// Decompiled with JetBrains decompiler
// Type: BansheeGz.BGDatabase.UnityWebRequest
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

using System;

#nullable enable
namespace BansheeGz.BGDatabase
{
  public class UnityWebRequest
  {
    public int timeout;
    public bool isNetworkError;
    public bool isHttpError;
    public string responseCode;
    public string error;
    public DownloadHandler downloadHandler;

    public static UnityWebRequest Get(string contextUrl) => throw new NotImplementedException();

    public static string EscapeURL(string httpParameterItem2)
    {
      throw new NotImplementedException();
    }

    public static UnityWebRequest Post(string contextUrl, WWWForm form)
    {
      throw new NotImplementedException();
    }

    public object SendWebRequest() => throw new NotImplementedException();

    public void SetRequestHeader(string httpHeaderItem1, string httpHeaderItem2)
    {
      throw new NotImplementedException();
    }
  }
}
