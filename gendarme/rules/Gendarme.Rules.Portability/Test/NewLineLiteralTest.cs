//
// Unit tests for NewLineLiteralRule
//
// Authors:
//	Sebastien Pouliot <sebastien@ximian.com>
//
// Copyright (C) 2006 Novell, Inc (http://www.novell.com)
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
using System.Reflection;

using Gendarme.Framework;
using Gendarme.Rules.Portability;
using Mono.Cecil;
using NUnit.Framework;

namespace Test.Rules.Interop {

	[TestFixture]
	public class NewLineTest {

		private IMethodRule rule;
		private IAssemblyDefinition assembly;
		private IModuleDefinition module;
		private ITypeDefinition type;

		private string GetNewLineLiteral_13 ()
		{
			return "Hello\nMono";
		}

		private string GetNewLineLiteral_10 ()
		{
			return "\rHello Mono";
		}

		private string GetNewLineLiteral ()
		{
			return "Hello Mono\r\n";
		}

		private string GetNewLine ()
		{
			return String.Concat ("Hello Mono", Environment.NewLine);
		}

		private string GetNull ()
		{
			return null;
		}

		private string GetEmpty ()
		{
			return "";
		}

		[TestFixtureSetUp]
		public void FixtureSetUp ()
		{
			string unit = Assembly.GetExecutingAssembly ().Location;
			assembly = AssemblyFactory.GetAssembly (unit);
			module = assembly.MainModule;
			type = assembly.MainModule.Types ["Test.Rules.Interop.NewLineTest"];
			rule = new NewLineLiteralRule ();
		}

		private IMethodDefinition GetTest (string name)
		{
			foreach (IMethodDefinition method in type.Methods) {
				if (method.Name == name)
					return method;
			}
			return null;
		}

		[Test]
		public void HasNewLineLiteral_13 ()
		{
			IMethodDefinition method = GetTest ("GetNewLineLiteral_13");
			Assert.IsNotNull (rule.CheckMethod (assembly, module, type, method, new MinimalRunner ()));
		}

		[Test]
		public void HasNewLineLiteral_10 ()
		{
			IMethodDefinition method = GetTest ("GetNewLineLiteral_10");
			Assert.IsNotNull (rule.CheckMethod (assembly, module, type, method, new MinimalRunner ()));
		}

		[Test]
		public void HasNewLineLiteral ()
		{
			IMethodDefinition method = GetTest ("GetNewLineLiteral");
			Assert.IsNotNull (rule.CheckMethod (assembly, module, type, method, new MinimalRunner ()));
		}

		[Test]
		public void HasNewLine ()
		{
			IMethodDefinition method = GetTest ("GetNewLine");
			Assert.IsNull (rule.CheckMethod (assembly, module, type, method, new MinimalRunner ()));
		}

		[Test]
		public void HasNull ()
		{
			IMethodDefinition method = GetTest ("GetNull");
			Assert.IsNull (rule.CheckMethod (assembly, module, type, method, new MinimalRunner ()));
		}

		[Test]
		public void HasEmpty ()
		{
			IMethodDefinition method = GetTest ("GetEmpty");
			Assert.IsNull (rule.CheckMethod (assembly, module, type, method, new MinimalRunner ()));
		}
	}
}