// Decompiled with JetBrains decompiler
// Type: D_Test
// Assembly: BGDatabaseStandalone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 045236C4-9128-4CB6-BAAB-C4AA7F0800A4
// Assembly location: C:\Users\alper\Documents\GitHub\SR_Editor\SR_Editor\BGDatabaseStandalone.dll

using BansheeGz.BGDatabase;
using System;
using System.Collections.Generic;

#nullable enable
public class D_Test : BGEntity
{
  private static BGMetaRow _metaDefault;
  private static BGFieldEntityName _ufle12jhs77_name;
  private static BGFieldInt _ufle12jhs77_intValue;
  private static readonly D_Test.Factory _factory0_PFS = new D_Test.Factory();

  public static BGMetaRow MetaDefault
  {
    get
    {
      BGMetaRow metaDefault = D_Test._metaDefault;
      if (metaDefault != null)
        return metaDefault;
      BGId metaId = new BGId(5460658520083705143UL, 15934140631239260591UL);
      return D_Test._metaDefault = BGCodeGenUtils.GetMeta<BGMetaRow>(metaId, (Action) (() => D_Test._metaDefault = (BGMetaRow) null));
    }
  }

  public static BGRepoEvents Events => BGRepo.I.Events;

  public static int CountEntities => D_Test.MetaDefault.CountEntities;

  public string name
  {
    get => D_Test._name[this.Index];
    set => D_Test._name[this.Index] = value;
  }

  public int intValue
  {
    get => D_Test._intValue[this.Index];
    set => D_Test._intValue[this.Index] = value;
  }

  public static BGFieldEntityName _name
  {
    get
    {
      BGFieldEntityName ufle12jhs77Name = D_Test._ufle12jhs77_name;
      if (ufle12jhs77Name != null)
        return ufle12jhs77Name;
      BGMetaRow metaDefault = D_Test.MetaDefault;
      BGId fieldId = new BGId(5664344857275741339UL, 9785100395792815008UL);
      return D_Test._ufle12jhs77_name = BGCodeGenUtils.GetField<BGFieldEntityName>((BGMetaEntity) metaDefault, fieldId, (Action) (() => D_Test._ufle12jhs77_name = (BGFieldEntityName) null));
    }
  }

  public static BGFieldInt _intValue
  {
    get
    {
      BGFieldInt ufle12jhs77IntValue = D_Test._ufle12jhs77_intValue;
      if (ufle12jhs77IntValue != null)
        return ufle12jhs77IntValue;
      BGMetaRow metaDefault = D_Test.MetaDefault;
      BGId fieldId = new BGId(4779073853211366417UL, 13529265378025683886UL);
      return D_Test._ufle12jhs77_intValue = BGCodeGenUtils.GetField<BGFieldInt>((BGMetaEntity) metaDefault, fieldId, (Action) (() => D_Test._ufle12jhs77_intValue = (BGFieldInt) null));
    }
  }

  private D_Test()
    : base((BGMetaEntity) D_Test.MetaDefault)
  {
  }

  private D_Test(BGId id)
    : base((BGMetaEntity) D_Test.MetaDefault, id)
  {
  }

  private D_Test(BGMetaEntity meta)
    : base(meta)
  {
  }

  private D_Test(BGMetaEntity meta, BGId id)
    : base(meta, id)
  {
  }

  public static D_Test FindEntity(Predicate<D_Test> filter)
  {
    return BGCodeGenUtils.FindEntity<D_Test>((BGMetaEntity) D_Test.MetaDefault, filter);
  }

  public static List<D_Test> FindEntities(
    Predicate<D_Test> filter,
    List<D_Test> result = null,
    Comparison<D_Test> sort = null)
  {
    return BGCodeGenUtils.FindEntities<D_Test>((BGMetaEntity) D_Test.MetaDefault, filter, result, sort);
  }

  public static void ForEachEntity(
    Action<D_Test> action,
    Predicate<D_Test> filter = null,
    Comparison<D_Test> sort = null)
  {
    BGCodeGenUtils.ForEachEntity<D_Test>((BGMetaEntity) D_Test.MetaDefault, action, filter, sort);
  }

  public static D_Test GetEntity(BGId entityId) => (D_Test) D_Test.MetaDefault.GetEntity(entityId);

  public static D_Test GetEntity(int index) => (D_Test) D_Test.MetaDefault[index];

  public static D_Test GetEntity(string entityName)
  {
    return (D_Test) D_Test.MetaDefault.GetEntity(entityName);
  }

  public static D_Test NewEntity() => (D_Test) D_Test.MetaDefault.NewEntity();

  public static D_Test NewEntity(BGId entityId) => (D_Test) D_Test.MetaDefault.NewEntity(entityId);

  public static D_Test NewEntity(Action<D_Test> callback)
  {
    return (D_Test) D_Test.MetaDefault.NewEntity(new BGMetaEntity.NewEntityContext((Action<BGEntity>) (entity => callback((D_Test) entity))));
  }

  public class Factory : BGEntity.EntityFactory
  {
    public BGEntity NewEntity(BGMetaEntity meta) => (BGEntity) new D_Test(meta);

    public BGEntity NewEntity(BGMetaEntity meta, BGId id) => (BGEntity) new D_Test(meta, id);
  }
}
