/*
 * Copyright (c) 2004, 2005 DotNetGuru and the individuals listed
 * on the ChangeLog entries.
 *
 * Authors :
 *   Jb Evain   (jbevain@gmail.com)
 *
 * This is a free software distributed under a MIT/X11 license
 * See LICENSE.MIT file for more details
 *
 * Generated by /CodeGen/cecil-gen.rb do not edit
 * Tue Aug 16 04:10:02 CEST 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Metadata {

	using System;
	using System.Collections;

	using Mono.Cecil.Binary;

	internal sealed class MetadataRowWriter : BaseMetadataRowVisitor {

		private MetadataRoot m_root;
		private MemoryBinaryWriter m_binaryWriter;
		private IDictionary m_ciCache;

		private int m_blobHeapIdxSz;
		private int m_stringsHeapIdxSz;
		private int m_guidHeapIdxSz;

		public MetadataRowWriter (MetadataTableWriter mtwv)
		{
			m_binaryWriter = mtwv.GetWriter ();
			m_root = mtwv.GetMetadataRoot ();
			m_ciCache = new Hashtable ();
		}

		private void WriteBlobPointer (uint pointer)
		{
			WriteByIndexSize (pointer, m_blobHeapIdxSz);
		}

		private void WriteStringPointer (uint pointer)
		{
			WriteByIndexSize (pointer, m_stringsHeapIdxSz);
		}

		private void WriteGuidPointer (uint pointer)
		{
			WriteByIndexSize (pointer, m_guidHeapIdxSz);
		}

		private void WriteTablePointer (uint pointer, Type table)
		{
			WriteByIndexSize (pointer, GetNumberOfRows (table) < (1 << 16) ? 2 : 4);
		}

		private void WriteMetadataToken (MetadataToken token, CodedIndex ci)
		{
			WriteByIndexSize (Utilities.CompressMetadataToken (ci, token),
				Utilities.GetCodedIndexSize (
					ci, new Utilities.TableRowCounter (GetNumberOfRows), m_ciCache));
		}

		private int GetNumberOfRows (Type table)
		{
			IMetadataTable t = m_root.Streams.TablesHeap [table];
			if (t == null || t.Rows == null)
				return 0;
			return t.Rows.Count;
		}

		private void WriteByIndexSize (uint value, int size)
		{
			if (size == 4)
				m_binaryWriter.Write (value);
			else if (size == 2)
				m_binaryWriter.Write ((ushort) value);
			else
				throw new MetadataFormatException ("Non valid size for indexing");
		}

		public AssemblyRow CreateAssemblyRow (AssemblyHashAlgorithm _hashAlgId, ushort _majorVersion, ushort _minorVersion, ushort _buildNumber, ushort _revisionNumber, AssemblyFlags _flags, uint _publicKey, uint _name, uint _culture)
		{
			AssemblyRow row = new AssemblyRow ();
			row.HashAlgId = _hashAlgId;
			row.MajorVersion = _majorVersion;
			row.MinorVersion = _minorVersion;
			row.BuildNumber = _buildNumber;
			row.RevisionNumber = _revisionNumber;
			row.Flags = _flags;
			row.PublicKey = _publicKey;
			row.Name = _name;
			row.Culture = _culture;
			return row;
		}

		public AssemblyOSRow CreateAssemblyOSRow (uint _oSPlatformID, uint _oSMajorVersion, uint _oSMinorVersion)
		{
			AssemblyOSRow row = new AssemblyOSRow ();
			row.OSPlatformID = _oSPlatformID;
			row.OSMajorVersion = _oSMajorVersion;
			row.OSMinorVersion = _oSMinorVersion;
			return row;
		}

		public AssemblyProcessorRow CreateAssemblyProcessorRow (uint _processor)
		{
			AssemblyProcessorRow row = new AssemblyProcessorRow ();
			row.Processor = _processor;
			return row;
		}

		public AssemblyRefRow CreateAssemblyRefRow (ushort _majorVersion, ushort _minorVersion, ushort _buildNumber, ushort _revisionNumber, AssemblyFlags _flags, uint _publicKeyOrToken, uint _name, uint _culture, uint _hashValue)
		{
			AssemblyRefRow row = new AssemblyRefRow ();
			row.MajorVersion = _majorVersion;
			row.MinorVersion = _minorVersion;
			row.BuildNumber = _buildNumber;
			row.RevisionNumber = _revisionNumber;
			row.Flags = _flags;
			row.PublicKeyOrToken = _publicKeyOrToken;
			row.Name = _name;
			row.Culture = _culture;
			row.HashValue = _hashValue;
			return row;
		}

		public AssemblyRefOSRow CreateAssemblyRefOSRow (uint _oSPlatformID, uint _oSMajorVersion, uint _oSMinorVersion, uint _assemblyRef)
		{
			AssemblyRefOSRow row = new AssemblyRefOSRow ();
			row.OSPlatformID = _oSPlatformID;
			row.OSMajorVersion = _oSMajorVersion;
			row.OSMinorVersion = _oSMinorVersion;
			row.AssemblyRef = _assemblyRef;
			return row;
		}

		public AssemblyRefProcessorRow CreateAssemblyRefProcessorRow (uint _processor, uint _assemblyRef)
		{
			AssemblyRefProcessorRow row = new AssemblyRefProcessorRow ();
			row.Processor = _processor;
			row.AssemblyRef = _assemblyRef;
			return row;
		}

		public ClassLayoutRow CreateClassLayoutRow (ushort _packingSize, uint _classSize, uint _parent)
		{
			ClassLayoutRow row = new ClassLayoutRow ();
			row.PackingSize = _packingSize;
			row.ClassSize = _classSize;
			row.Parent = _parent;
			return row;
		}

		public ConstantRow CreateConstantRow (ElementType _type, MetadataToken _parent, uint _value)
		{
			ConstantRow row = new ConstantRow ();
			row.Type = _type;
			row.Parent = _parent;
			row.Value = _value;
			return row;
		}

		public CustomAttributeRow CreateCustomAttributeRow (MetadataToken _parent, MetadataToken _type, uint _value)
		{
			CustomAttributeRow row = new CustomAttributeRow ();
			row.Parent = _parent;
			row.Type = _type;
			row.Value = _value;
			return row;
		}

		public DeclSecurityRow CreateDeclSecurityRow (SecurityAction _action, MetadataToken _parent, uint _permissionSet)
		{
			DeclSecurityRow row = new DeclSecurityRow ();
			row.Action = _action;
			row.Parent = _parent;
			row.PermissionSet = _permissionSet;
			return row;
		}

		public EventRow CreateEventRow (EventAttributes _eventFlags, uint _name, MetadataToken _eventType)
		{
			EventRow row = new EventRow ();
			row.EventFlags = _eventFlags;
			row.Name = _name;
			row.EventType = _eventType;
			return row;
		}

		public EventMapRow CreateEventMapRow (uint _parent, uint _eventList)
		{
			EventMapRow row = new EventMapRow ();
			row.Parent = _parent;
			row.EventList = _eventList;
			return row;
		}

		public ExportedTypeRow CreateExportedTypeRow (TypeAttributes _flags, uint _typeDefId, uint _typeName, uint _typeNamespace, MetadataToken _implementation)
		{
			ExportedTypeRow row = new ExportedTypeRow ();
			row.Flags = _flags;
			row.TypeDefId = _typeDefId;
			row.TypeName = _typeName;
			row.TypeNamespace = _typeNamespace;
			row.Implementation = _implementation;
			return row;
		}

		public FieldRow CreateFieldRow (FieldAttributes _flags, uint _name, uint _signature)
		{
			FieldRow row = new FieldRow ();
			row.Flags = _flags;
			row.Name = _name;
			row.Signature = _signature;
			return row;
		}

		public FieldLayoutRow CreateFieldLayoutRow (uint _offset, uint _field)
		{
			FieldLayoutRow row = new FieldLayoutRow ();
			row.Offset = _offset;
			row.Field = _field;
			return row;
		}

		public FieldMarshalRow CreateFieldMarshalRow (MetadataToken _parent, uint _nativeType)
		{
			FieldMarshalRow row = new FieldMarshalRow ();
			row.Parent = _parent;
			row.NativeType = _nativeType;
			return row;
		}

		public FieldRVARow CreateFieldRVARow (RVA _rVA, uint _field)
		{
			FieldRVARow row = new FieldRVARow ();
			row.RVA = _rVA;
			row.Field = _field;
			return row;
		}

		public FileRow CreateFileRow (FileAttributes _flags, uint _name, uint _hashValue)
		{
			FileRow row = new FileRow ();
			row.Flags = _flags;
			row.Name = _name;
			row.HashValue = _hashValue;
			return row;
		}

		public GenericParamRow CreateGenericParamRow (ushort _number, GenericParamAttributes _flags, MetadataToken _owner, uint _name)
		{
			GenericParamRow row = new GenericParamRow ();
			row.Number = _number;
			row.Flags = _flags;
			row.Owner = _owner;
			row.Name = _name;
			return row;
		}

		public GenericParamConstraintRow CreateGenericParamConstraintRow (uint _owner, MetadataToken _constraint)
		{
			GenericParamConstraintRow row = new GenericParamConstraintRow ();
			row.Owner = _owner;
			row.Constraint = _constraint;
			return row;
		}

		public ImplMapRow CreateImplMapRow (PInvokeAttributes _mappingFlags, MetadataToken _memberForwarded, uint _importName, uint _importScope)
		{
			ImplMapRow row = new ImplMapRow ();
			row.MappingFlags = _mappingFlags;
			row.MemberForwarded = _memberForwarded;
			row.ImportName = _importName;
			row.ImportScope = _importScope;
			return row;
		}

		public InterfaceImplRow CreateInterfaceImplRow (uint _class, MetadataToken _interface)
		{
			InterfaceImplRow row = new InterfaceImplRow ();
			row.Class = _class;
			row.Interface = _interface;
			return row;
		}

		public ManifestResourceRow CreateManifestResourceRow (uint _offset, ManifestResourceAttributes _flags, uint _name, MetadataToken _implementation)
		{
			ManifestResourceRow row = new ManifestResourceRow ();
			row.Offset = _offset;
			row.Flags = _flags;
			row.Name = _name;
			row.Implementation = _implementation;
			return row;
		}

		public MemberRefRow CreateMemberRefRow (MetadataToken _class, uint _name, uint _signature)
		{
			MemberRefRow row = new MemberRefRow ();
			row.Class = _class;
			row.Name = _name;
			row.Signature = _signature;
			return row;
		}

		public MethodRow CreateMethodRow (RVA _rVA, MethodImplAttributes _implFlags, MethodAttributes _flags, uint _name, uint _signature, uint _paramList)
		{
			MethodRow row = new MethodRow ();
			row.RVA = _rVA;
			row.ImplFlags = _implFlags;
			row.Flags = _flags;
			row.Name = _name;
			row.Signature = _signature;
			row.ParamList = _paramList;
			return row;
		}

		public MethodImplRow CreateMethodImplRow (uint _class, MetadataToken _methodBody, MetadataToken _methodDeclaration)
		{
			MethodImplRow row = new MethodImplRow ();
			row.Class = _class;
			row.MethodBody = _methodBody;
			row.MethodDeclaration = _methodDeclaration;
			return row;
		}

		public MethodSemanticsRow CreateMethodSemanticsRow (MethodSemanticsAttributes _semantics, uint _method, MetadataToken _association)
		{
			MethodSemanticsRow row = new MethodSemanticsRow ();
			row.Semantics = _semantics;
			row.Method = _method;
			row.Association = _association;
			return row;
		}

		public MethodSpecRow CreateMethodSpecRow (MetadataToken _method, uint _instantiation)
		{
			MethodSpecRow row = new MethodSpecRow ();
			row.Method = _method;
			row.Instantiation = _instantiation;
			return row;
		}

		public ModuleRow CreateModuleRow (ushort _generation, uint _name, uint _mvid, uint _encId, uint _encBaseId)
		{
			ModuleRow row = new ModuleRow ();
			row.Generation = _generation;
			row.Name = _name;
			row.Mvid = _mvid;
			row.EncId = _encId;
			row.EncBaseId = _encBaseId;
			return row;
		}

		public ModuleRefRow CreateModuleRefRow (uint _name)
		{
			ModuleRefRow row = new ModuleRefRow ();
			row.Name = _name;
			return row;
		}

		public NestedClassRow CreateNestedClassRow (uint _nestedClass, uint _enclosingClass)
		{
			NestedClassRow row = new NestedClassRow ();
			row.NestedClass = _nestedClass;
			row.EnclosingClass = _enclosingClass;
			return row;
		}

		public ParamRow CreateParamRow (ParamAttributes _flags, ushort _sequence, uint _name)
		{
			ParamRow row = new ParamRow ();
			row.Flags = _flags;
			row.Sequence = _sequence;
			row.Name = _name;
			return row;
		}

		public PropertyRow CreatePropertyRow (PropertyAttributes _flags, uint _name, uint _type)
		{
			PropertyRow row = new PropertyRow ();
			row.Flags = _flags;
			row.Name = _name;
			row.Type = _type;
			return row;
		}

		public PropertyMapRow CreatePropertyMapRow (uint _parent, uint _propertyList)
		{
			PropertyMapRow row = new PropertyMapRow ();
			row.Parent = _parent;
			row.PropertyList = _propertyList;
			return row;
		}

		public StandAloneSigRow CreateStandAloneSigRow (uint _signature)
		{
			StandAloneSigRow row = new StandAloneSigRow ();
			row.Signature = _signature;
			return row;
		}

		public TypeDefRow CreateTypeDefRow (TypeAttributes _flags, uint _name, uint _namespace, MetadataToken _extends, uint _fieldList, uint _methodList)
		{
			TypeDefRow row = new TypeDefRow ();
			row.Flags = _flags;
			row.Name = _name;
			row.Namespace = _namespace;
			row.Extends = _extends;
			row.FieldList = _fieldList;
			row.MethodList = _methodList;
			return row;
		}

		public TypeRefRow CreateTypeRefRow (MetadataToken _resolutionScope, uint _name, uint _namespace)
		{
			TypeRefRow row = new TypeRefRow ();
			row.ResolutionScope = _resolutionScope;
			row.Name = _name;
			row.Namespace = _namespace;
			return row;
		}

		public TypeSpecRow CreateTypeSpecRow (uint _signature)
		{
			TypeSpecRow row = new TypeSpecRow ();
			row.Signature = _signature;
			return row;
		}

		public override void VisitRowCollection (RowCollection coll)
		{
			m_blobHeapIdxSz = m_root.Streams.BlobHeap != null ?
				m_root.Streams.BlobHeap.IndexSize : 2;
			m_stringsHeapIdxSz = m_root.Streams.StringsHeap != null ?
				m_root.Streams.StringsHeap.IndexSize : 2;
			m_guidHeapIdxSz = m_root.Streams.GuidHeap != null ?
				m_root.Streams.GuidHeap.IndexSize : 2;
		}

		public override void VisitAssemblyRow (AssemblyRow row)
		{
			m_binaryWriter.Write ((uint) row.HashAlgId);
			m_binaryWriter.Write (row.MajorVersion);
			m_binaryWriter.Write (row.MinorVersion);
			m_binaryWriter.Write (row.BuildNumber);
			m_binaryWriter.Write (row.RevisionNumber);
			m_binaryWriter.Write ((uint) row.Flags);
			WriteBlobPointer (row.PublicKey);
			WriteStringPointer (row.Name);
			WriteStringPointer (row.Culture);
		}

		public override void VisitAssemblyOSRow (AssemblyOSRow row)
		{
			m_binaryWriter.Write (row.OSPlatformID);
			m_binaryWriter.Write (row.OSMajorVersion);
			m_binaryWriter.Write (row.OSMinorVersion);
		}

		public override void VisitAssemblyProcessorRow (AssemblyProcessorRow row)
		{
			m_binaryWriter.Write (row.Processor);
		}

		public override void VisitAssemblyRefRow (AssemblyRefRow row)
		{
			m_binaryWriter.Write (row.MajorVersion);
			m_binaryWriter.Write (row.MinorVersion);
			m_binaryWriter.Write (row.BuildNumber);
			m_binaryWriter.Write (row.RevisionNumber);
			m_binaryWriter.Write ((uint) row.Flags);
			WriteBlobPointer (row.PublicKeyOrToken);
			WriteStringPointer (row.Name);
			WriteStringPointer (row.Culture);
			WriteBlobPointer (row.HashValue);
		}

		public override void VisitAssemblyRefOSRow (AssemblyRefOSRow row)
		{
			m_binaryWriter.Write (row.OSPlatformID);
			m_binaryWriter.Write (row.OSMajorVersion);
			m_binaryWriter.Write (row.OSMinorVersion);
			WriteTablePointer (row.AssemblyRef, typeof (AssemblyRefTable));
		}

		public override void VisitAssemblyRefProcessorRow (AssemblyRefProcessorRow row)
		{
			m_binaryWriter.Write (row.Processor);
			WriteTablePointer (row.AssemblyRef, typeof (AssemblyRefTable));
		}

		public override void VisitClassLayoutRow (ClassLayoutRow row)
		{
			m_binaryWriter.Write (row.PackingSize);
			m_binaryWriter.Write (row.ClassSize);
			WriteTablePointer (row.Parent, typeof (TypeDefTable));
		}

		public override void VisitConstantRow (ConstantRow row)
		{
			m_binaryWriter.Write ((ushort) row.Type);
			WriteMetadataToken (row.Parent, CodedIndex.HasConstant);
			WriteBlobPointer (row.Value);
		}

		public override void VisitCustomAttributeRow (CustomAttributeRow row)
		{
			WriteMetadataToken (row.Parent, CodedIndex.HasCustomAttribute);
			WriteMetadataToken (row.Type, CodedIndex.CustomAttributeType);
			WriteBlobPointer (row.Value);
		}

		public override void VisitDeclSecurityRow (DeclSecurityRow row)
		{
			m_binaryWriter.Write ((short) row.Action);
			WriteMetadataToken (row.Parent, CodedIndex.HasDeclSecurity);
			WriteBlobPointer (row.PermissionSet);
		}

		public override void VisitEventRow (EventRow row)
		{
			m_binaryWriter.Write ((ushort) row.EventFlags);
			WriteStringPointer (row.Name);
			WriteMetadataToken (row.EventType, CodedIndex.TypeDefOrRef);
		}

		public override void VisitEventMapRow (EventMapRow row)
		{
			WriteTablePointer (row.Parent, typeof (TypeDefTable));
			WriteTablePointer (row.EventList, typeof (EventTable));
		}

		public override void VisitExportedTypeRow (ExportedTypeRow row)
		{
			m_binaryWriter.Write ((uint) row.Flags);
			m_binaryWriter.Write (row.TypeDefId);
			WriteStringPointer (row.TypeName);
			WriteStringPointer (row.TypeNamespace);
			WriteMetadataToken (row.Implementation, CodedIndex.Implementation);
		}

		public override void VisitFieldRow (FieldRow row)
		{
			m_binaryWriter.Write ((ushort) row.Flags);
			WriteStringPointer (row.Name);
			WriteBlobPointer (row.Signature);
		}

		public override void VisitFieldLayoutRow (FieldLayoutRow row)
		{
			m_binaryWriter.Write (row.Offset);
			WriteTablePointer (row.Field, typeof (FieldTable));
		}

		public override void VisitFieldMarshalRow (FieldMarshalRow row)
		{
			WriteMetadataToken (row.Parent, CodedIndex.HasFieldMarshal);
			WriteBlobPointer (row.NativeType);
		}

		public override void VisitFieldRVARow (FieldRVARow row)
		{
			m_binaryWriter.Write (row.RVA.Value);
			WriteTablePointer (row.Field, typeof (FieldTable));
		}

		public override void VisitFileRow (FileRow row)
		{
			m_binaryWriter.Write ((uint) row.Flags);
			WriteStringPointer (row.Name);
			WriteBlobPointer (row.HashValue);
		}

		public override void VisitGenericParamRow (GenericParamRow row)
		{
			m_binaryWriter.Write (row.Number);
			m_binaryWriter.Write ((ushort) row.Flags);
			WriteMetadataToken (row.Owner, CodedIndex.TypeOrMethodDef);
			WriteStringPointer (row.Name);
		}

		public override void VisitGenericParamConstraintRow (GenericParamConstraintRow row)
		{
			WriteTablePointer (row.Owner, typeof (GenericParamTable));
			WriteMetadataToken (row.Constraint, CodedIndex.TypeDefOrRef);
		}

		public override void VisitImplMapRow (ImplMapRow row)
		{
			m_binaryWriter.Write ((ushort) row.MappingFlags);
			WriteMetadataToken (row.MemberForwarded, CodedIndex.MemberForwarded);
			WriteStringPointer (row.ImportName);
			WriteTablePointer (row.ImportScope, typeof (ModuleRefTable));
		}

		public override void VisitInterfaceImplRow (InterfaceImplRow row)
		{
			WriteTablePointer (row.Class, typeof (TypeDefTable));
			WriteMetadataToken (row.Interface, CodedIndex.TypeDefOrRef);
		}

		public override void VisitManifestResourceRow (ManifestResourceRow row)
		{
			m_binaryWriter.Write (row.Offset);
			m_binaryWriter.Write ((uint) row.Flags);
			WriteStringPointer (row.Name);
			WriteMetadataToken (row.Implementation, CodedIndex.Implementation);
		}

		public override void VisitMemberRefRow (MemberRefRow row)
		{
			WriteMetadataToken (row.Class, CodedIndex.MemberRefParent);
			WriteStringPointer (row.Name);
			WriteBlobPointer (row.Signature);
		}

		public override void VisitMethodRow (MethodRow row)
		{
			m_binaryWriter.Write (row.RVA.Value);
			m_binaryWriter.Write ((ushort) row.ImplFlags);
			m_binaryWriter.Write ((ushort) row.Flags);
			WriteStringPointer (row.Name);
			WriteBlobPointer (row.Signature);
			WriteTablePointer (row.ParamList, typeof (ParamTable));
		}

		public override void VisitMethodImplRow (MethodImplRow row)
		{
			WriteTablePointer (row.Class, typeof (TypeDefTable));
			WriteMetadataToken (row.MethodBody, CodedIndex.MethodDefOrRef);
			WriteMetadataToken (row.MethodDeclaration, CodedIndex.MethodDefOrRef);
		}

		public override void VisitMethodSemanticsRow (MethodSemanticsRow row)
		{
			m_binaryWriter.Write ((ushort) row.Semantics);
			WriteTablePointer (row.Method, typeof (MethodTable));
			WriteMetadataToken (row.Association, CodedIndex.HasSemantics);
		}

		public override void VisitMethodSpecRow (MethodSpecRow row)
		{
			WriteMetadataToken (row.Method, CodedIndex.MethodDefOrRef);
			WriteBlobPointer (row.Instantiation);
		}

		public override void VisitModuleRow (ModuleRow row)
		{
			m_binaryWriter.Write (row.Generation);
			WriteStringPointer (row.Name);
			WriteGuidPointer (row.Mvid);
			WriteGuidPointer (row.EncId);
			WriteGuidPointer (row.EncBaseId);
		}

		public override void VisitModuleRefRow (ModuleRefRow row)
		{
			WriteStringPointer (row.Name);
		}

		public override void VisitNestedClassRow (NestedClassRow row)
		{
			WriteTablePointer (row.NestedClass, typeof (TypeDefTable));
			WriteTablePointer (row.EnclosingClass, typeof (TypeDefTable));
		}

		public override void VisitParamRow (ParamRow row)
		{
			m_binaryWriter.Write ((ushort) row.Flags);
			m_binaryWriter.Write (row.Sequence);
			WriteStringPointer (row.Name);
		}

		public override void VisitPropertyRow (PropertyRow row)
		{
			m_binaryWriter.Write ((ushort) row.Flags);
			WriteStringPointer (row.Name);
			WriteBlobPointer (row.Type);
		}

		public override void VisitPropertyMapRow (PropertyMapRow row)
		{
			WriteTablePointer (row.Parent, typeof (TypeDefTable));
			WriteTablePointer (row.PropertyList, typeof (PropertyTable));
		}

		public override void VisitStandAloneSigRow (StandAloneSigRow row)
		{
			WriteBlobPointer (row.Signature);
		}

		public override void VisitTypeDefRow (TypeDefRow row)
		{
			m_binaryWriter.Write ((uint) row.Flags);
			WriteStringPointer (row.Name);
			WriteStringPointer (row.Namespace);
			WriteMetadataToken (row.Extends, CodedIndex.TypeDefOrRef);
			WriteTablePointer (row.FieldList, typeof (FieldTable));
			WriteTablePointer (row.MethodList, typeof (MethodTable));
		}

		public override void VisitTypeRefRow (TypeRefRow row)
		{
			WriteMetadataToken (row.ResolutionScope, CodedIndex.ResolutionScope);
			WriteStringPointer (row.Name);
			WriteStringPointer (row.Namespace);
		}

		public override void VisitTypeSpecRow (TypeSpecRow row)
		{
			WriteBlobPointer (row.Signature);
		}

	}
}
