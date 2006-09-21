//
// ResultWriter
//
// Authors:
//	Christian Birkl <christian.birkl@gmail.com>
//	Sebastien Pouliot <sebastien@ximian.com>
//
// Copyright (C) 2006 Christian Birkl
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
using System.Collections;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Gendarme.Framework;

public class XmlResultWriter : IResultWriter {

	private XmlTextWriter writer;

	public XmlResultWriter (string output)
	{
		if ((output == null) || (output.Length == 0))
			writer = new XmlTextWriter (Console.Out);
		else
			writer = new XmlTextWriter (output, Encoding.UTF8);
	}
	
	public void Start ()
	{
		writer.Formatting = Formatting.Indented;
		writer.WriteProcessingInstruction ("xml", "version='1.0'");
		writer.WriteStartElement ("gendarme-output");
		writer.WriteAttributeString ("date", DateTime.UtcNow.ToString ());
	}

	public void End ()
	{
		writer.WriteEndElement ();
		writer.Flush ();
		writer.Close ();
		writer = null;
	}

	public void Write (ICollection assemblies)
	{
		foreach (string assembly in assemblies) {
			writer.WriteElementString("input", assembly);
		}
	}

	public void Write (Rules rules)
	{
		writer.WriteStartElement ("rules");
		Rules("Assembly", rules.Assembly);
		Rules("Module", rules.Module);
		Rules("Type", rules.Type);
		Rules("Method", rules.Method);
		writer.WriteEndElement ();
	}

	private void Rules (string type, RuleCollection rules)
	{
		foreach (IRule rule in rules) {
			writer.WriteStartElement ("rule");			
			writer.WriteAttributeString ("Type", type);
			writer.WriteString (rule.GetType ().FullName);
			writer.WriteEndElement ();
		}
	}

	public void Write (Violation v)
	{
		RuleInformation ri = RuleInformationManager.GetRuleInformation (v.Rule);

		writer.WriteStartElement ("violation");		
		writer.WriteAttributeString ("Assembly", v.Assembly.ToString ());
		writer.WriteAttributeString ("Name", ri.Name);
		writer.WriteAttributeString ("Uri", ri.Uri);
		writer.WriteElementString ("problem", String.Format (ri.Problem, v.Violator));
		writer.WriteElementString ("solution", String.Format (ri.Solution, v.Violator));
		
		if ((v.Messages != null) && (v.Messages.Count > 0)) {
			writer.WriteStartElement ("messages");
			foreach (object o in v.Messages) {
				writer.WriteStartElement ("message");
				// FIXME: review existing rules
				Message message = (o as Message);
				if (message != null) {
					if (message.Location != null)
						writer.WriteAttributeString ("Location", message.Location.ToString());
					writer.WriteAttributeString ("Type", message.Type.ToString());
					writer.WriteString (message.Text);
				} else {
					writer.WriteString (o.ToString ());
				}
				writer.WriteEndElement ();
			}
			writer.WriteEndElement ();
		}

		writer.WriteEndElement ();
	}
}