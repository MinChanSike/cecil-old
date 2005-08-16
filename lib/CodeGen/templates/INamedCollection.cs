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
 * <%=Time.now%>
 *
 *****************************************************************************/

namespace Mono.Cecil {

	using System.Collections;

	public interface <%=$cur_coll.intf%> : ICollection<% if (!$cur_coll.visitable.nil?) then %>, <%=$cur_coll.visitable%><% end %> {

		<%=$cur_coll.type%> this [string name] { get; set; }

		<%=$cur_coll.container%> Container { get; }

		void Add (string name, <%=$cur_coll.type%> value);
		void Clear ();
		bool Contains (<%=$cur_coll.type%> value);
		void Remove (<%=$cur_coll.type%> value);
	}
}
