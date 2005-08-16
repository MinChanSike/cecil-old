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
 *****************************************************************************/

namespace Mono.Cecil.Signatures {

	using System;
	using System.Text;

	using Mono.Cecil.Binary;
	using Mono.Cecil.Implem;
	using Mono.Cecil.Metadata;

	internal sealed class SignatureWriter : ISignatureVisitor {

		private MetadataWriter m_mdWriter;
		private ReflectionWriter m_reflectWriter;

		private MemoryBinaryWriter m_sigWriter;

		public SignatureWriter (MetadataWriter mdWriter, ReflectionWriter reflectWriter)
		{
			m_mdWriter = mdWriter;
			m_reflectWriter = reflectWriter;

			m_sigWriter = new MemoryBinaryWriter ();
		}

		private uint GetPointer ()
		{
			return m_mdWriter.AddBlob (m_sigWriter.ToArray (), true);
		}

		public uint AddMethodDefSig (MethodDefSig methSig)
		{
			return AddSignature (methSig);
		}

		public uint AddMethodRefSig (MethodRefSig methSig)
		{
			return AddSignature (methSig);
		}

		public uint AddPropertySig (PropertySig ps)
		{
			return AddSignature (ps);
		}

		public uint AddFieldSig (FieldSig fSig)
		{
			return AddSignature (fSig);
		}

		private uint AddSignature (Signature s)
		{
			m_sigWriter.Empty ();
			s.Accept (this);
			return GetPointer ();
		}

		public uint AddTypeSpec (TypeSpec ts)
		{
			m_sigWriter.Empty ();
			Write (ts);
			return GetPointer ();
		}

		public uint AddMarshalSig (MarshalSig ms)
		{
			m_sigWriter.Empty ();
			Write (ms);
			return GetPointer ();
		}

		public uint AddCustomAttribute (CustomAttrib ca, IMethodReference ctor)
		{
			m_sigWriter.Write (CompressCustomAttribute (ca, ctor));
			return GetPointer ();
		}

		public byte [] CompressCustomAttribute (CustomAttrib ca, IMethodReference ctor)
		{
			m_sigWriter.Empty ();
			Write (ca, ctor);
			return m_sigWriter.ToArray ();
		}

		public void VisitMethodDefSig (MethodDefSig methodDef)
		{
			m_sigWriter.Write (methodDef.CallingConvention);
			Write (methodDef.ParamCount);
			Write (methodDef.RetType);
			Write (methodDef.Parameters);
		}

		public void VisitMethodRefSig (MethodRefSig methodRef)
		{
			m_sigWriter.Write (methodRef.CallingConvention);
			Write (methodRef.ParamCount);
			Write (methodRef.RetType);
			Write (methodRef.Parameters);
		}

		public void VisitFieldSig (FieldSig field)
		{
			m_sigWriter.Write (field.CallingConvention);
			Write (field.CustomMods);
			Write (field.Type);
		}

		public void VisitPropertySig (PropertySig property)
		{
			m_sigWriter.Write (property.CallingConvention);
			Write (property.ParamCount);
			Write (property.Type);
			Write (property.Parameters);
		}

		public void VisitLocalVarSig (LocalVarSig localvar)
		{
			m_sigWriter.Write (localvar.CallingConvention);
			Write (localvar.Count);
			Write (localvar.LocalVariables);
		}

		private void Write (LocalVarSig.LocalVariable [] vars)
		{
			foreach (LocalVarSig.LocalVariable var in vars)
				Write (var);
		}

		private void Write (LocalVarSig.LocalVariable var)
		{
			if ((var.Constraint & Constraint.Pinned) != 0)
				Write (ElementType.Pinned);
			if (var.ByRef)
				Write (ElementType.ByRef);
			Write (var.Type);
		}

		private void Write (RetType retType)
		{
			Write (retType.CustomMods);
			if (retType.Void)
				Write (ElementType.Void);
			else if (retType.TypedByRef)
				Write (ElementType.TypedByRef);
			else if (retType.ByRef) {
				Write (ElementType.ByRef);
				Write (retType.Type);
			} else
				Write (retType.Type);
		}

		private void Write (Param [] parameters)
		{
			foreach (Param p in parameters)
				Write (p);
		}

		private void Write (ElementType et)
		{
			Write ((int) et);
		}

		private void Write (SigType t)
		{
			Write ((int) t.ElementType);

			switch (t.ElementType) {
			case ElementType.ValueType :
				Write ((int) Utilities.CompressMetadataToken (
						CodedIndex.TypeDefOrRef, ((VALUETYPE) t).Type));
				break;
			case ElementType.Class :
				Write ((int) Utilities.CompressMetadataToken (
						CodedIndex.TypeDefOrRef, ((CLASS) t).Type));
				break;
			case ElementType.Ptr :
				PTR p = (PTR) t;
				if (p.Void)
					Write (ElementType.Void);
				else {
					Write (p.CustomMods);
					Write (p.PtrType);
				}
				break;
			case ElementType.FnPtr :
				FNPTR fp = (FNPTR) t;
				if (fp.Method is MethodRefSig)
					(fp.Method as MethodRefSig).Accept (this);
				else
					(fp.Method as MethodDefSig).Accept (this);
				break;
			case ElementType.Array :
				ARRAY ary = (ARRAY) t;
				ArrayShape shape = ary.Shape;
				Write (ary.Type);
				Write (shape.Rank);
				Write (shape.NumSizes);
				foreach (int size in shape.Sizes)
					Write (size);
				Write (shape.NumLoBounds);
				foreach (int loBound in shape.LoBounds)
					Write (loBound);
				break;
			case ElementType.SzArray :
				Write (((SZARRAY) t).Type);
				break;
			}
		}

		private void Write (TypeSpec ts)
		{
			Write (ts.Type);
		}

		private void Write (Param p)
		{
			Write (p.CustomMods);
			if (p.TypedByRef)
				Write (ElementType.TypedByRef);
			else if (p.ByRef) {
				Write (ElementType.ByRef);
				Write (p.Type);
			} else
				Write (p.Type);
		}

		private void Write (CustomMod [] customMods)
		{
			foreach (CustomMod cm in customMods)
				Write (cm);
		}

		private void Write (CustomMod cm)
		{
			switch (cm.CMOD) {
			case CustomMod.CMODType.OPT :
				Write (ElementType.CModOpt);
				break;
			case CustomMod.CMODType.REQD :
				Write (ElementType.CModReqD);
				break;
			}

			Write ((int) Utilities.CompressMetadataToken (
					CodedIndex.TypeDefOrRef, cm.TypeDefOrRef));
		}

		private void Write (MarshalSig ms)
		{
			Write ((int) ms.NativeInstrinsic);
			switch (ms.NativeInstrinsic) {
			case NativeType.ARRAY :
				MarshalSig.Array ar = (MarshalSig.Array) ms.Spec;
				Write ((int) ar.ArrayElemType);
				if (ar.ParamNum != -1)
					Write (ar.ParamNum);
				if (ar.NumElem != -1)
					Write (ar.NumElem);
				if (ar.ElemMult != -1)
					Write (ar.ElemMult);
				break;
			case NativeType.CUSTOMMARSHALER :
				MarshalSig.CustomMarshaler cm = (MarshalSig.CustomMarshaler) ms.Spec;
				Write (cm.Guid);
				Write (cm.UnmanagedType);
				Write (cm.ManagedType);
				Write (cm.Cookie);
				break;
			case NativeType.FIXEDARRAY :
				MarshalSig.FixedArray fa = (MarshalSig.FixedArray) ms.Spec;
				Write (fa.NumElem);
				if (fa.ArrayElemType != NativeType.NONE)
					Write ((int) fa.ArrayElemType);
				break;
			case NativeType.SAFEARRAY :
				Write ((int) ((MarshalSig.SafeArray) ms.Spec).ArrayElemType);
				break;
			case NativeType.FIXEDSYSSTRING :
				Write (((MarshalSig.FixedSysString) ms.Spec).Size);
				break;
			}
		}

		private void Write (CustomAttrib ca, IMethodReference ctor)
		{
			if (ca.Prolog != CustomAttrib.StdProlog)
				return;

			m_sigWriter.Write (ca.Prolog);

			for (int i = 0; i < ctor.Parameters.Count; i++) {
				ITypeReference paramType = ctor.Parameters [i].ParameterType;
				Write (ca.FixedArgs [i], paramType);
			}

			m_sigWriter.Write ((ushort) ca.NamedArgs.Length);
			foreach (CustomAttrib.NamedArg na in ca.NamedArgs)
				Write (na);
		}

		private void Write (CustomAttrib.FixedArg fa, object type)
		{
			if (fa.SzArray)
				m_sigWriter.Write (fa.NumElem);

			foreach (CustomAttrib.Elem elem in fa.Elems)
				Write (elem, type);
		}

		private void Write (CustomAttrib.NamedArg na)
		{
			if (na.Field)
				m_sigWriter.Write ((byte) 0x53);
			else if (na.Property)
				m_sigWriter.Write ((byte) 0x54);
			else
				throw new MetadataFormatException ("Unknown kind of namedarg");

			if (na.FixedArg.SzArray)
				m_sigWriter.Write ((byte) ElementType.SzArray);

			m_sigWriter.Write ((byte) na.FieldOrPropType);

			if (na.FieldOrPropType == ElementType.Enum)
				Write (na.FixedArg.Elems [0].ElemType.FullName);

			Write (na.FieldOrPropName);

			Write (na.FixedArg, na.FieldOrPropType);
		}

		private void Write (CustomAttrib.Elem elem, object type)
		{
			if (type is ITypeReference)
				Write (elem, type as ITypeReference);
			else if (type is ElementType)
				Write (elem, (ElementType) type);
			else
				throw new MetadataFormatException ("Wrong parameter type for Elem" + type.GetType ().FullName);
		}

		private void Write (CustomAttrib.Elem elem, ITypeReference type)
		{
			throw new NotImplementedException ("TODO"); // TODO
		}

		private void Write (CustomAttrib.Elem elem, ElementType type)
		{
			throw new NotImplementedException ("TODO"); // TODO
		}

		private void Write (string s)
		{
			Write (s.Length);
			m_sigWriter.Write (Encoding.UTF8.GetBytes (s));
		}

		private void Write (int i)
		{
			Utilities.WriteCompressedInteger (m_sigWriter, i);
		}
	}
}
