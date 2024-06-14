/*
<copyright file="BGMetaView.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// view implementation.
    /// View is collection of rows from different tables
    /// </summary>
    public class BGMetaView : BGMetaObject
    {
        private readonly BGMetaViewMappings mappings;

        /// <summary>
        /// the database, this meta belongs to
        /// </summary>
        public BGRepo Repo { get; private set; }

        /// <inheritdoc/>
        public override int Index => Repo.GetViewIndex(Id);

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
                base.Comment = value;
                FireViewChanged();
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
                base.ControllerType = newControllerNull ? null : value;
                FireViewChanged();
            }
        }
        
        /// <summary>
        /// View's name (should be unique across database)
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

                Repo.ViewNameWasChanged(oldName, Name);
            }
        }

        public BGMetaViewMappings Mappings => mappings;

        private BGMetaRow delegateMeta;

        /// <summary>
        /// table with views fields. This meta does not belong to the same BGRepo as view does
        /// </summary>
        public BGMetaRow DelegateMeta
        {
            get => delegateMeta;
            internal set
            {
                if (delegateMeta == value) return;
                if (delegateMeta != null) delegateMeta.Repo.Events.OnAnyChange -= DelegateMetaChanged;
                delegateMeta = value;
                delegateMeta.Repo.Events.On = true;
                delegateMeta.Repo.Events.OnAnyChange -= DelegateMetaChanged;
                delegateMeta.Repo.Events.OnAnyChange += DelegateMetaChanged;
            }
        }

        /// <summary>
        /// the list of inbound relations
        /// </summary>
        public List<BGAbstractRelationI> RelationsInbound
        {
            get
            {
                var relationsInbound = new List<BGAbstractRelationI>();

                Repo.ForEachMeta(meta =>
                {
                    var fields = meta.FindFields();
                    foreach (var field in fields)
                    {
                        if (!(field is BGAbstractRelationI relation)) continue;
                        switch (relation)
                        {
                            case BGFieldViewRelationSingle relationI:
                            {
                                var relatedView = relationI.View;
                                if (relatedView == null || !Equals(relatedView, this)) continue;
                                relationsInbound.Add(relation);
                                break;
                            }
                            case BGFieldViewRelationMultiple manyTablesRelationI:
                            {
                                var relatedView = manyTablesRelationI.View;
                                if (relatedView == null || !Equals(relatedView, this)) continue;
                                relationsInbound.Add(relation);
                                break;
                            }
                        }
                    }
                });
                return relationsInbound;
            }
        }

        /// <summary>
        /// included metas
        /// </summary>
        public List<BGMetaEntity> Metas
        {
            get
            {
                var result = new List<BGMetaEntity>();
                foreach (var metaId in mappings.IncludedMetas)
                {
                    var meta = Repo.GetMeta(metaId);
                    if (meta == null) continue;
                    result.Add(meta);
                }

                return result;
            }
        }

        //================================================================================================
        //                                              Constructors
        //================================================================================================

        // For new models only!
        public BGMetaView(BGRepo repo, string name) : this(repo, repo.NewViewId, name)
        {
            DelegateMeta = new BGMetaRow(new BGRepo(), Id, name);
            new BGFieldEntityName(delegateMeta, null) { System = true };
        }

        // For existing models
        private BGMetaView(BGRepo repo, BGId id, string name) : base(id, name)
        {
            Repo = repo ?? throw new BGException("Repo can not be null");
            Repo.Register(this);
            mappings = new BGMetaViewMappings(this);
        }

        internal static BGMetaView Create(BGRepo repo, BGId id, string name) => new BGMetaView(repo, id, name);

        //================================================================================================
        //                                              Methods
        //================================================================================================
        /// <summary>
        /// Check the current status and throws exception is mapping have any error
        /// </summary>
        public void CheckStatus() => Repo.ForEachMeta(meta => Mappings.CheckStatus(meta));

        /// <inheritdoc/>
        public override void Delete()
        {
            if (IsDeleted) return;
            base.Delete();

            Repo.Events.Batch(() =>
            {
                var reverseRelations = RelationsInbound;
                if (!BGUtil.IsEmpty(reverseRelations))
                {
                    foreach (var reverseRelation in reverseRelations) ((BGField)reverseRelation).Delete();
                }

                Unregister();
            });
            Unload();
            Repo = null;
        }

        //remove table from repo register
        protected void Unregister() => Repo?.Unregister(this);

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString() => null;

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes() => null;

        /// <inheritdoc />
        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
        }

        internal static BGMetaView FromBinary(BGBinaryReader binder, BGRepo repo)
        {
            var version = binder.ReadInt();
            switch (version)
            {
                case 1:
                case 2:
                {
                    //params
                    var id = binder.ReadId();
                    var name = binder.ReadString();
                    var view = Create(repo, id, name);

                    //parameters
                    view.ConfigFromBytes(binder.ReadByteArray());
                    view.Comment = binder.ReadString();
                    view.Addon = binder.ReadString();
                    view.System = binder.ReadBool();

                    //repo
                    var viewRepo = new BGRepo();
                    viewRepo.Load(BGUtil.ToArray(binder.ReadByteArray()));
                    view.DelegateMeta = (BGMetaRow)viewRepo.GetMeta(id);

                    //mappings
                    binder.ReadArray(() =>
                    {
                        view.Mappings.Add(binder.ReadId());
                    });

                    if (version >= 2) view.ControllerType = binder.ReadString();

                    return view;
                }
                default:
                {
                    throw new BGException("Can not read view from binary array: unsupported version $", version);
                }
            }
        }

        // flush table to binary stream
        internal static void ToBinary(BGBinaryWriter builder, BGMetaView view)
        {
            const int version = 2;

            //version
            builder.AddInt(version);

            //params
            builder.AddId(view.Id);
            builder.AddString(view.Name);
            builder.AddByteArray(view.ConfigToBytes());
            builder.AddString(view.Comment);
            builder.AddString(view.Addon);
            builder.AddBool(view.System);


            //repo
            var delegateMetaRepo = view.delegateMeta.Repo;
            for (var i = delegateMetaRepo.CountViews - 1; i >= 0; i--) delegateMetaRepo.GetView(i).Delete();
            builder.AddByteArray(delegateMetaRepo.Save());

            //mappings
            view.Mappings.Trim();
            builder.AddArray(() =>
            {
                foreach (var metaId in view.Mappings.IncludedMetas) builder.AddId(metaId);
            }, view.Mappings.MappingsCount);
            
            //v2
            builder.AddString(view.ControllerType);
        }


        //================================================================================================
        //                                              Misc
        //================================================================================================
        /// <summary>
        /// if provided field type is supported as view field
        /// </summary>
        public static bool IsFieldTypeSupported(Type fieldType)
        {
            if (BGLocalizationUglyHacks.IsLocaleField(fieldType)) return false;
            if (typeof(BGAbstractRelationI).IsAssignableFrom(fieldType)) return false;
            /*
            if (fieldType == typeof(BGFieldNested)) return false;
            if (fieldType == typeof(BGFieldViewRelationSingle)) return false;
            if (fieldType == typeof(BGFieldViewRelationMultiple)) return false;
            */

            return true;
        }

        /// <summary>
        /// clone view to provided repo
        /// </summary>
        public BGMetaView CloneTo(BGRepo repo)
        {
            // if (metaFilter != null && !metaFilter(Id)) return null;
            var clone = BGMetaView.Create(repo, Id, Name);
            clone.Addon = Addon;
            clone.Comment = Comment;
            clone.ControllerType = ControllerType;

            var cloneDelegateRepo = new BGRepo();
            delegateMeta.Repo.CloneTo(cloneDelegateRepo, null, null, false);
            clone.DelegateMeta = cloneDelegateRepo.GetMeta<BGMetaRow>(delegateMeta.Id);

            Mappings.CloneTo(clone.Mappings);
            return clone;
        }

        /// <summary>
        /// fire view changed event
        /// </summary>
        public void FireViewChanged() => Repo.Events.ViewWasChanged(this);

        private void DelegateMetaChanged(object sender, BGEventArgsAnyChange e) => Repo.Events.ViewWasChanged(this);

        /// <summary>
        /// sync fields with provided view
        /// </summary>
        public void ComplyFields(BGMetaView view2)
        {
            if (view2.Id != Id) throw new Exception($"View IDs mismatch, {view2.Id}!={Id}");
            var repo = new BGRepo();
            view2.DelegateMeta.Repo.CloneTo(repo, null, null, false);
            DelegateMeta = repo.GetMeta<BGMetaRow>(Id);
        }

        //do not remove it's used by reflection
        internal void SwitchTo(BGRepo repo)
        {
            Repo = repo;
            Repo.Register(this);
        }

        /// <summary>
        /// compare views parameters
        /// </summary>
        public bool DeepEqual(BGMetaView t2)
        {
            if (!string.Equals(Name, t2.Name)) return false;
            if (!string.Equals(Comment, t2.Comment)) return false;
            if (!string.Equals(ControllerType, t2.ControllerType)) return false;
            if (!Mappings.DeepEqual(t2.Mappings)) return false;
            if (DelegateMeta.CountFields != t2.DelegateMeta.CountFields) return false;

            for (var i = 0; i < DelegateMeta.CountFields; i++)
            {
                var field = DelegateMeta.GetField(i);
                var field2 = t2.DelegateMeta.GetField(i);
                if (field.Name != field2.Name) return false;
                if (field.GetType() != field2.GetType()) return false;
            }

            return true;
        }

        /// <summary>
        /// count the number of rows, referencing this view with view relational fields
        /// </summary>
        public int CountRelatedEntities(BGId metaId)
        {
            var relations = RelationsInbound;
            var result = 0;
            foreach (var relation in relations)
            {
                var field = (BGField)relation;
                field.Meta.ForEachEntity(entity =>
                {
                    switch (relation)
                    {
                        case BGFieldViewRelationSingle relationSingle:
                        {
                            var rowRef = relationSingle.GetStoredValue(entity.Index);
                            if (rowRef == null || rowRef.MetaId != metaId) return;
                            result++;
                            break;
                        }
                        case BGFieldViewRelationMultiple relationMultiple:
                        {
                            var list = relationMultiple.GetStoredValue(entity.Index);
                            if (list == null || list.Count == 0) return;
                            if (list.Find(rowRef => rowRef?.MetaId == metaId) == null) return;
                            result++;
                            break;
                        }
                    }
                });
            }

            return result;
        }
    }
}