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
 * Mon Aug 15 18:16:45 CEST 2005
 *
 *****************************************************************************/

namespace Mono.Cecil {

	using System.Collections;

	public interface ITypeReferenceCollection : ICollection, IReflectionVisitable {

		ITypeReference this [string name] { get; set; }

		IModuleDefinition Container { get; }

		void Add (string name, ITypeReference value);
		void Clear ();
		bool Contains (ITypeReference value);
		void Remove (ITypeReference value);
	}
}
