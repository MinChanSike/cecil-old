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
 *****************************************************************************/

namespace Mono.Cecil.Cil {

    public interface ICodeVisitor {
        void Visit (IMethodBody body);
        void Visit (IInstructionCollection instructions);
        void Visit (IInstruction instr);
        void Visit (IExceptionHandlerCollection seh);
        void Visit (IExceptionHandler eh);
        void Visit (IVariableDefinitionCollection variables);
        void Visit (IVariableDefinition var);
    }
}