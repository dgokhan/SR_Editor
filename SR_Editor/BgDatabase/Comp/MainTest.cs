// Decompiled with JetBrains decompiler
// Type: MainTest
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

using BansheeGz.BGDatabase;
using System;
using System.IO;

#nullable enable
public class MainTest
{
  public static void Main()
  {
    BGRepo.SetDefaultRepoContent(File.ReadAllBytes("../../../../../Unity/Assets/Resources/bansheegz_database.bytes "));
    BGRepo.I.ForEachMeta((Action<BGMetaEntity>) (meta =>
    {
      Console.WriteLine("Table " + meta.Name);
      meta.ForEachField((Action<BGField>) (field =>
      {
        Console.Write(field.Name);
        Console.Write("\t");
      }));
      Console.WriteLine("\t");
      meta.ForEachEntity((Action<BGEntity>) (entity =>
      {
        meta.ForEachField((Action<BGField>) (field =>
        {
          if (field is BGAssetLoaderA.WithLoaderI && field is BGStorableString bgStorableString2)
            Console.Write(bgStorableString2.GetStoredValue(entity.Index));
          else
            Console.Write(field.GetValue(entity.Index));
          Console.Write("\t");
        }));
        Console.WriteLine("\t");
      }));
    }));
  }
}
