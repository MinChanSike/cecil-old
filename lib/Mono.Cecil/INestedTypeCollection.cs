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

	public interface INestedTypeCollection : ICollection, IReflectionVisitable {

		ITypeDefinition this [string name] { get; set; }

		ITypeDefinition Container { get; }

		void Add (string name, ITypeDefinition value);
		void Clear ();
		bool Contains (ITypeDefinition value);
		void Remove (ITypeDefinition value);
	}
}
