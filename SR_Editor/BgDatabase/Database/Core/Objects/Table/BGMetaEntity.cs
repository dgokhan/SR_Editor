/*
<copyright file="BGMetaEntity.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Abstract class for the table
    /// </summary>
    public abstract partial class BGMetaEntity : BGMetaObject, IEquatable<BGMetaEntity>
    {
        //================================================================================================
        //                                              Static
        //================================================================================================
        //microoptimization: dictionary to hold already created meta factories
        //safe-to-use in multi-threaded environment
        private static readonly Dictionary<string, Func<BGRepo, BGId, string, BGMetaEntity>> MetaTypeName2Factory = new Dictionary<string, Func<BGRepo, BGId, string, BGMetaEntity>>();

        /// <summary>
        /// Table's descriptor attribute 
        /// </summary>
        public class MetaDescriptor : BGAttributeWithManager
        {
            public bool SkipInList;
        }

        //safe-to-use in multi-threaded environment
        private static readonly List<Type> MetaTypesList = new List<Type>();

        /// <summary>
        /// all classes, that extends from BGMetaEntity
        /// </summary>
        public static List<Type> MetaTypes
        {
            get
            {
                if (MetaTypesList.Count != 0) return MetaTypesList;

                var allSubTypes = BGUtil.GetAllSubTypes(typeof(BGMetaEntity));
                foreach (var fieldType in allSubTypes) MetaTypesList.Add(fieldType);
                return MetaTypesList;
            }
        }

        //================================================================================================
        //                                              Misc
        //================================================================================================
        /// <summary>
        /// Meta's name (should be unique across database)
        /// </summary>
        public override string Name
        {
            set
            {
                if (string.Equals(Name, value)) return;
                Repo.ErrorIfMetaNameIsNotUnique(value);

                var oldName = Name;
                //name is checked in base class
                base.Name = value;

                Repo.MetaNameWasChanged(oldName, Name);
            }
        }

        /// <summary>
        /// Meta's name for GUI
        /// </summary>
        public string DisplayName => BGAttribute.GetName(GetType());

        /// <summary>
        /// the database, this meta belongs to
        /// </summary>
        public BGRepo Repo { get; private set; }

        protected abstract Func<BGRepo, BGId, string, BGMetaEntity> CreateMetaFactory();

        /// <summary>
        /// meta manages its own entities (create/delete)
        /// </summary>
        public virtual bool IsManagingItsOwnEntities => false;

        /// <summary>
        /// meta supports partition addon
        /// </summary>
        public virtual bool SupportPartitioningField => true;

        /// <summary>
        /// type code is used by internal database code. Do not override it
        /// </summary>
        public virtual ushort TypeCode => 0;

        private BGRepoEvents events;


        //================================================================================================
        //                                              Relations
        //================================================================================================

        // private List<BGRelationI> relationsInbound;

        //================================================================================================
        //                                              Parameters
        //================================================================================================

        private bool uniqueName;

        /// <summary>
        /// Is entity name unique? This is used by validation only (which works in Unity Editor)   
        /// </summary>
        public virtual bool UniqueName
        {
            get => uniqueName;
            set
            {
                if (uniqueName == value) return;
                uniqueName = value;
                if (events.On) events.MetaWasChanged(this);
            }
        }

        private bool singleton;

        /// <summary>
        /// Is only one entity supposed to exist? This is used by validation only (which works in Unity Editor)   
        /// </summary>
        public virtual bool Singleton
        {
            get => singleton;
            set
            {
                if (singleton == value) return;
                singleton = value;
                if (events.On) events.MetaWasChanged(this);
            }
        }

        private bool emptyName;

        /// <summary>
        /// Is entity name is empty?    
        /// </summary>
        public virtual bool EmptyName
        {
            get => emptyName;
            set
            {
                if (emptyName == value) return;
                if (LazyLoader != null) LazyLoad();
                if (fields.Count > 0)
                {
                    var nameField = NameField;
                    if (nameField != null) nameField.NameEmpty = value;
                }

                emptyName = value;
                if (events.On) events.MetaWasChanged(this);
            }
        }

        /// <inheritdoc/>
        public override int Index => Repo.GetMetaIndex(Id);

        /// <inheritdoc/>
        public override string Comment
        {
            set
            {
                var oldComment = base.Comment;
                if (string.Equals(value, oldComment)) return;
                var newCommentNull = string.IsNullOrEmpty(value);
                var commentNull = string.IsNullOrEmpty(oldComment);
                if (newCommentNull && commentNull) return;
                SetComment(value);
                if (events.On) events.MetaWasChanged(this);
            }
        }

        /// <inheritdoc/>
        public override string ControllerType
        {
            set
            {
                var oldControllerType = base.ControllerType;
                if (string.Equals(value, oldControllerType)) return;
                var newControllerNull = string.IsNullOrEmpty(value);
                var controllerNull = string.IsNullOrEmpty(oldControllerType);
                if (newControllerNull && controllerNull) return;
                base.ControllerType = string.IsNullOrEmpty(value) ? null : value;
                Repo.Events.MetaWasChanged(this);
            }
        }

        private bool userDefinedReadonly;

        /// <summary>
        /// if user marked the table as readonly
        /// </summary>
        public bool UserDefinedReadonly
        {
            get => userDefinedReadonly;
            set
            {
                if (userDefinedReadonly == value) return;
                userDefinedReadonly = value;
                Repo.Events.MetaWasChanged(this);
            }
        }

        //================================================================================================
        //                                              Constructors
        //================================================================================================

        // For new models only!
        protected BGMetaEntity(BGRepo repo, string name) : this(repo, repo.NewMetaId, name)
        {
            new BGFieldEntityName(this, null) { System = true };
            events = Repo.Events;
        }

        // For existing models
        protected BGMetaEntity(BGRepo repo, BGId id, string name) : base(id, name)
        {
            Repo = repo ?? throw new BGException("Repo can not be null");
            Repo.Register(this);
            events = Repo.Events;
        }


        //================================================================================================
        //                                              Registration
        //================================================================================================

        //remove table from repo register
        protected void Unregister() => Repo?.Unregister(this);


        //================================================================================================
        //                                              Configuration
        //================================================================================================
        /// <inheritdoc/>
        public override string ConfigToString() => null;

        /// <inheritdoc/>
        public override void ConfigFromString(string config)
        {
        }

        /// <inheritdoc/>
        public override byte[] ConfigToBytes() => null;

        /// <inheritdoc/>
        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
        }


        //================================================================================================
        //                                              Delete
        //================================================================================================
        /// <inheritdoc/>
        public override void Delete()
        {
            if (IsDeleted) return;
            if (LazyLoader != null) LazyLoad();
            base.Delete();
            Repo.Events.Batch(() =>
            {
                //fields
                var fieldList = new List<BGField>(fields);
                foreach (var field in fieldList) field.Delete();

                //entities
                if (CountEntities > 0) DeleteEntities(new HashSet<BGEntity>(Store.ToList()));

                //reverse relations
                var reverseRelations = RelationsInbound;
                if (!BGUtil.IsEmpty(reverseRelations))
                    foreach (var reverseRelation in reverseRelations)
                        switch (reverseRelation)
                        {
                            case BGRelationI _:
                                //single
                                ((BGField)reverseRelation).Delete();
                                break;
                            case BGManyTablesRelationI manyRelation:
                                //multiple
                                manyRelation.RemoveRelatedMeta(this);
                                break;
                        }

                //views
                Repo.ForEachView(view =>
                {
                    if (!view.Mappings.IsIncluded(Id)) return;
                    view.Mappings.Remove(Id);
                });

                //unregister
                Unregister();
            });
            Unload();
            Repo = null;
        }


        //========================================================================================
        //              Misc
        //========================================================================================
        public override string ToString()
        {
            return "Meta [id:" + Id + ", name:" + Name + ", type:" + GetType().FullName + "]";
        }

        private void SetComment(string comment) => base.Comment = comment;

        /// <summary>
        /// Clone this meta to new repo   
        /// </summary>
        public virtual BGMetaEntity CloneTo(BGRepo repo, Predicate<BGId> metaFilter, Predicate<BGField> fieldFilter, bool copyValues)
            => CloneTo(new BGCloneContextMeta(repo, metaFilter, fieldFilter, copyValues, null));

        /// <summary>
        /// Clone this meta to new repo   
        /// </summary>
        public virtual BGMetaEntity CloneTo(BGRepo repo, Predicate<BGId> metaFilter, Predicate<BGField> fieldFilter, bool copyValues, Predicate<BGEntity> entityFilter)
            => CloneTo(new BGCloneContextMeta(repo, metaFilter, fieldFilter, copyValues, entityFilter));

        /// <summary>
        /// Clone this meta to new repo   
        /// </summary>
        public virtual BGMetaEntity CloneTo(BGCloneContextMeta context)
        {
            if (context.metaFilter != null && !context.metaFilter(Id)) return null;

            var clone = CreateMetaFactory()(context.repo, Id, Name);
//            var clone = BGUtil.Create<BGMetaEntity>(GetType(), true, repo, Id, Name);
            clone.Addon = Addon;
            CopyAttributesTo(clone);

            if (context.copyValues)
            {
                if (context.entityFilter == null) ForEachEntity(entity => clone.NewEntity(entity.Id));
                else
                {
                    //entityFilter is rarely set, so we copy implementation to avoid check for entityFilter==null if entityFilter is null
                    ForEachEntity(entity => clone.NewEntity(entity.Id), context.entityFilter);
                }
            }

            var fieldCloneContext = new BGCloneContextField(clone, context.copyValues) { OnAfterFieldCreated = context.OnAfterFieldCreated };
            ForEachField(field => field.CloneTo(fieldCloneContext), context.fieldFilter);

            ForEachKey(key => key.CloneTo(clone));
            ForEachIndex(index => index.CloneTo(clone));
            return clone;
        }

        /// <summary>
        /// copy table attributes to the clone object
        /// </summary>
        public void CopyAttributesTo(BGMetaEntity clone)
        {
            clone.System = System;
            clone.uniqueName = UniqueName;
            clone.singleton = Singleton;
            clone.emptyName = EmptyName;
            clone.SetComment(Comment);
            clone.UserDefinedReadonly = UserDefinedReadonly;
            clone.ControllerType = ControllerType;
            var configToBytes = ConfigToBytes();
            clone.ConfigFromBytes(configToBytes == null ? new ArraySegment<byte>(Array.Empty<byte>()) : new ArraySegment<byte>(configToBytes));
        }

        //do not remove it's used by reflection
        internal void SwitchTo(BGRepo repo)
        {
            Repo = repo;
            Repo.Register(this);
            events = Repo.Events;
            // ForEachField(field => field.SwitchTo(repo));
        }

        public bool Equals(BGMetaEntity other) => other != null && Id == other.Id;

        //========================================================================================
        //              Factory
        //========================================================================================
        /// <summary>
        /// Create new meta using string config 
        /// </summary>
        public static BGMetaEntity Create(BGRepo repo, string type, BGId id, string name, string config, bool system, string addon, bool uniqueName, bool singleton, bool emptyName)
        {
            var meta = Create(repo, type, id, name, system, addon, uniqueName, singleton, emptyName);
            meta.ConfigFromString(config);
            return meta;
        }

        /// <summary>
        /// Create new meta using byte array config   
        /// </summary>
        public static BGMetaEntity Create(BGRepo repo, string type, BGId id, string name, ArraySegment<byte> config, bool system, string addon, bool uniqueName, bool singleton, bool emptyName)
        {
            var meta = Create(repo, type, id, name, system, addon, uniqueName, singleton, emptyName);
            meta.ConfigFromBytes(config);
            return meta;
        }

        //create table with provided attributes
        private static BGMetaEntity Create(BGRepo repo, string type, BGId id, string name, bool system, string addon, bool uniqueName, bool singleton, bool emptyName)
        {
            //-------------------- create
            BGMetaEntity meta;
            if (MetaTypeName2Factory.TryGetValue(type, out var factory)) meta = factory(repo, id, name);
            else
            {
                meta = BGUtil.Create<BGMetaEntity>(type, true, repo, id, name);
                MetaTypeName2Factory[type] = meta.CreateMetaFactory();
            }

            //-------------------- fill all fields
            meta.System = system;
            meta.UniqueName = uniqueName;
            meta.Singleton = singleton;
            meta.EmptyName = emptyName;
            meta.Addon = addon;
            return meta;
        }

        //========================================================================================
        //              Duplicate
        //========================================================================================
        /*
        public virtual BGMetaEntity Duplicate(string newMetaName)
        {
            throw new BGException("This meta does not support duplication");
        }
        */

        internal BGLazyLoadMetaLoader LazyLoader;
        public bool LazyLoadingEnabledAndNotLoadedYet => LazyLoader != null;

        internal void LazyLoad()
        {
            if (LazyLoader == null) return;
            var temp = LazyLoader;
            LazyLoader = null;
            try
            {
                temp.Load();
            }
            catch (Exception e)
            {
                //something broken- we can not ignore this error, cause it can lead to data loss
                LazyLoader = temp;
                throw;
            }
            
        }

        //========================================================================================
        //              Binary
        //========================================================================================
        // reconstruct table from binary stream 
        internal static BGMetaEntity FromBinary(BGBinaryReader binder, BGRepo repo)
        {
            var version = binder.ReadInt();
            switch (version)
            {
                case 1:
                {
                    var id = binder.ReadId();
                    var name = binder.ReadString();
                    var type = binder.ReadString();
                    var config = binder.ReadByteArray();
                    var system = binder.ReadBool();
                    var addon = binder.ReadString();
                    var nameUnique = binder.ReadBool();
                    var singleton = binder.ReadBool();
                    var nameEmpty = binder.ReadBool();
                    var meta = Create(repo, type, id, name, config, system, addon, nameUnique, singleton, nameEmpty);
                    meta.SetComment(binder.ReadString());
                    return meta;
                }
                case 2:
                case 3:
                case 4:
                {
                    var typeCode = binder.ReadUShort();
                    string type = null;
                    if (typeCode == 0) type = binder.ReadString();
                    var id = binder.ReadId();
                    var name = binder.ReadString();
                    // var type = binder.ReadString();
                    var config = binder.ReadByteArray();
                    var system = binder.ReadBool();
                    var addon = binder.ReadString();
                    var nameUnique = binder.ReadBool();
                    var singleton = binder.ReadBool();
                    var nameEmpty = binder.ReadBool();
                    var meta = typeCode == 0
                        ? Create(repo, type, id, name, config, system, addon, nameUnique, singleton, nameEmpty)
                        : BGMetaTypeCodeFactory.Instance.Create(repo, typeCode, id, name, config, system, addon, nameUnique, singleton, nameEmpty);
                    meta.SetComment(binder.ReadString());

                    //v.3
                    if (version >= 3) meta.UserDefinedReadonly = binder.ReadBool();

                    //v.4
                    if (version >= 4) meta.ControllerType = binder.ReadString();

                    return meta;
                }
                default:
                {
                    throw new BGException("Can not read meta from binary array: unsupported version $", version);
                }
            }
        }

        // flush table to binary stream
        internal static void ToBinary(BGBinaryWriter builder, BGMetaEntity meta)
        {
            //version
            builder.AddInt(4);

            //type code
            builder.AddUShort(meta.TypeCode);
            if (meta.TypeCode == 0) builder.AddString(meta.GetType().AssemblyQualifiedName);

            builder.AddId(meta.Id);
            builder.AddString(meta.Name);
            // builder.AddString(meta.GetType().AssemblyQualifiedName);
            builder.AddByteArray(meta.ConfigToBytes());
            builder.AddBool(meta.System);
            builder.AddString(meta.Addon);
            builder.AddBool(meta.UniqueName);
            builder.AddBool(meta.Singleton);
            builder.AddBool(meta.EmptyName);
            builder.AddString(meta.Comment);

            //v.3
            builder.AddBool(meta.UserDefinedReadonly);

            //v.4
            builder.AddString(meta.ControllerType);
        }
    }
}