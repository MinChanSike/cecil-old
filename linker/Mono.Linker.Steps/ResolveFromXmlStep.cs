//
// ResolveFromXmlStep.cs
//
// Author:
//   Jb Evain (jbevain@gmail.com)
//
// (C) 2006 Jb Evain
// (C) 2007 Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using SR = System.Reflection;
using System.Text;
using System.Xml.XPath;

using Mono.Cecil;

namespace Mono.Linker.Steps {

	public class ResolveFromXmlStep : ResolveStep {

		static readonly string _signature = "signature";
		static readonly string _fullname = "fullname";
		static readonly string _required = "required";
		static readonly string _preserve = "preserve";
		static readonly string _ns = "";

		XPathDocument _document;

		public ResolveFromXmlStep (XPathDocument document)
		{
			_document = document;
		}

		public override void Process (LinkContext context)
		{
			XPathNavigator nav = _document.CreateNavigator ();
			nav.MoveToFirstChild ();
			ProcessAssemblies (context, nav.SelectChildren ("assembly", _ns));
		}

		void ProcessAssemblies (LinkContext context, XPathNodeIterator iterator)
		{
			while (iterator.MoveNext ()) {
				AssemblyDefinition assembly = GetAssembly (context, GetFullName (iterator.Current));
				ProcessTypes (assembly, iterator.Current.SelectChildren ("type", _ns));
			}
		}

		void ProcessTypes (AssemblyDefinition assembly, XPathNodeIterator iterator)
		{
			while (iterator.MoveNext ()) {
				XPathNavigator nav = iterator.Current;
				string fullname = GetFullName (nav);
				TypeDefinition type = assembly.MainModule.Types [fullname];
				if (type == null)
					continue;

				TypePreserve preserve = GetTypePreserve (nav);

				if (!IsRequired (nav)) {
					Annotations.SetPreserve (type, preserve);
					continue;
				}

				Annotations.Mark (type);

				switch (preserve) {
				case TypePreserve.Nothing:
					if (nav.HasChildren) {
						MarkSelectedFields (nav, type);
						MarkSelectedMethods (nav, type);
					} else
						Annotations.SetPreserve (type, TypePreserve.All);
					break;
				default:
					Annotations.SetPreserve (type, preserve);
					break;
				}
			}
		}

		void MarkSelectedFields (XPathNavigator nav, TypeDefinition type)
		{
			XPathNodeIterator fields = nav.SelectChildren ("field", _ns);
			if (fields.Count == 0)
				return;

			ProcessFields (type, fields);
		}

		void MarkSelectedMethods (XPathNavigator nav, TypeDefinition type)
		{
			XPathNodeIterator methods = nav.SelectChildren ("method", _ns);
			if (methods.Count == 0)
				return;

			ProcessMethods (type, methods);
		}

		static TypePreserve GetTypePreserve (XPathNavigator nav)
		{
			try {
				return (TypePreserve) Enum.Parse (typeof (TypePreserve), GetAttribute (nav, _preserve), true);
			} catch {
				return TypePreserve.Nothing;
			}
		}

		void ProcessFields (TypeDefinition type, XPathNodeIterator iterator)
		{
			while (iterator.MoveNext ()) {
				string signature = GetSignature (iterator.Current);
				FieldDefinition field = GetField (type, signature);
				if (field != null)
					Annotations.Mark (field);
				else
					AddUnresolveMarker (string.Format ("T: {0}; F: {1}", type, signature));
			}
		}

		static FieldDefinition GetField (TypeDefinition type, string signature)
		{
			foreach (FieldDefinition field in type.Fields)
				if (signature == GetFieldSignature (field))
					return field;

			return null;
		}

		static string GetFieldSignature (FieldDefinition field)
		{
			return field.FieldType.FullName + " " + field.Name;
		}

		void ProcessMethods (TypeDefinition type, XPathNodeIterator iterator)
		{
			while (iterator.MoveNext()) {
				string signature = GetSignature (iterator.Current);
				MethodDefinition meth = GetMethod (type, signature);
				if (meth != null) {
					Annotations.Mark (meth);
					Annotations.SetAction (meth, MethodAction.Parse);
				} else
					AddUnresolveMarker (string.Format ("T: {0}; M: {1}", type, signature));
			}
		}

		static MethodDefinition GetMethod (TypeDefinition type, string signature)
		{
			foreach (MethodDefinition meth in type.Methods)
				if (signature == GetMethodSignature (meth))
					return meth;

			foreach (MethodDefinition ctor in type.Constructors)
				if (signature == GetMethodSignature (ctor))
					return ctor;

			return null;
		}

		static string GetMethodSignature (MethodDefinition meth)
		{
			StringBuilder sb = new StringBuilder ();
			sb.Append (meth.ReturnType.ReturnType.FullName);
			sb.Append (" ");
			sb.Append (meth.Name);
			sb.Append ("(");
			for (int i = 0; i < meth.Parameters.Count; i++) {
				if (i > 0)
					sb.Append (",");

				sb.Append (meth.Parameters [i].ParameterType.FullName);
			}
			sb.Append (")");
			return sb.ToString ();
		}

		static AssemblyDefinition GetAssembly (LinkContext context, string assemblyName)
		{
			AssemblyNameReference reference = AssemblyNameReference.Parse (assemblyName);
			if (IsSimpleName (assemblyName)) {
				foreach (AssemblyDefinition assembly in context.GetAssemblies ()) {
					if (assembly.Name.Name == assemblyName)
						return assembly;
				}
			}

			AssemblyDefinition res = context.Resolve (reference);
			ProcessReferences (res, context);
			return res;
		}

		static void ProcessReferences (AssemblyDefinition assembly, LinkContext context)
		{
			foreach (AssemblyNameReference name in assembly.MainModule.AssemblyReferences)
				context.Resolve (name);
		}

		static bool IsSimpleName (string assemblyName)
		{
			return assemblyName.IndexOf (",") == -1;
		}

		static bool IsRequired (XPathNavigator nav)
		{
			string attribute = GetAttribute (nav, _required);
			if (attribute == null || attribute.Length == 0)
				return true;

			return TryParseBool (attribute);
		}

		static bool TryParseBool (string s)
		{
			try {
				return bool.Parse (s);
			} catch {
				return false;
			}
		}

		static string GetSignature (XPathNavigator nav)
		{
			return GetAttribute (nav, _signature);
		}

		static string GetFullName (XPathNavigator nav)
		{
			return GetAttribute (nav, _fullname);
		}

		static string GetAttribute (XPathNavigator nav, string attribute)
		{
			return nav.GetAttribute (attribute, _ns);
		}
	}
}