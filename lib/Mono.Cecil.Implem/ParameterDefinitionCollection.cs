/*
 * Copyright (c) 2004 DotNetGuru and the individuals listed
 * on the ChangeLog entries.
 *
 * Authors :
 *   Jb Evain   (jb.evain@dotnetguru.org)
 *
 * This is a free software distributed under a MIT/X11 license
 * See LICENSE.MIT file for more details
 *
 * Generated by /CodeGen/cecil-gen.rb do not edit
 * Sun Jan 30 19:35:08 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Implem {

    using System;
    using System.Collections;
    using System.Collections.Specialized;

    using Mono.Cecil;

    internal class ParameterDefinitionCollection : IParameterDefinitionCollection, ILazyLoadableCollection {

        private IDictionary m_items;
        private MemberDefinition m_container;

        private bool m_loaded;

        public IParameterDefinition this [string name] {
            get {
                ((TypeDefinition)m_container.DeclaringType).Module.Loader.ReflectionReader.Visit (this);
                return m_items [name] as IParameterDefinition;
            }
            set { m_items [name] = value; }
        }

        public IMemberDefinition Container {
            get { return m_container; }
        }

        public int Count {
            get {
                ((TypeDefinition)m_container.DeclaringType).Module.Loader.ReflectionReader.Visit (this);
                return m_items.Count;
            }
        }

        public bool IsSynchronized {
            get { return false; }
        }

        public object SyncRoot {
            get { return this; }
        }

        public bool Loaded {
            get { return m_loaded; }
            set { m_loaded = value; }
        }

        public ParameterDefinitionCollection (MemberDefinition container)
        {
            m_container = container;
            m_items = new ListDictionary ();
        }

        public void Clear ()
        {
            m_items.Clear ();
        }

        public bool Contains (IParameterDefinition value)
        {
            return m_items.Contains (value);
        }

        public void Remove (IParameterDefinition value)
        {
            m_items.Remove (value);
        }

        public void CopyTo (Array ary, int index)
        {
            ((TypeDefinition)m_container.DeclaringType).Module.Loader.ReflectionReader.Visit (this);
            m_items.Values.CopyTo (ary, index);
        }

        public IEnumerator GetEnumerator ()
        {
            ((TypeDefinition)m_container.DeclaringType).Module.Loader.ReflectionReader.Visit (this);
            return m_items.Values.GetEnumerator ();
        }

        public void Accept (IReflectionVisitor visitor)
        {
            visitor.Visit (this);
            IParameterDefinition [] items = new IParameterDefinition [m_items.Count];
            m_items.Values.CopyTo (items, 0);
            for (int i = 0; i < items.Length; i++)
                items [i].Accept (visitor);
        }
    }
}