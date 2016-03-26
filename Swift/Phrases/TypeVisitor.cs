﻿using Swift.AST_Nodes;
using Swift.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swift.AssTargets;
using Swift.AST_Nodes.Types;
using Swift.Instructions;
using Swift.Instructions.Directives;

namespace Swift.Phrases
{
    public class TypeVisitor
    {
        public ASTType visit(BinaryConstant n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(HexadecimalConstant n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(RegisterOffset n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(Base n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(FunctionCallExp n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(AndExp n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(DivisionExp n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(MinusExp n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(MultiplicationExp n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(IdentifierExp n)
        {
            return n.Scope.Lookup(n.ID.Name).accept(this);
        }

        public ASTType visit(PlusExp n)
        {
            ASTType t1 = n.e1.accept(this);
            ASTType t2 = n.e2.accept(this);
            if (t1.GetType() == t2.GetType())
                return t1;
            Swift.error("The types of the terms of the expression on line " + n.Context.GetLine() + ", column " + n.Context.GetPos() + " don't match", 1);
            return null;
        }

        public ASTType visit(IntegerLiteral n)
        {
            switch (Swift.architecture)
            {
                case Global.InstructionSets.X86: return new Int32Type();
                case Global.InstructionSets.X86_64: return new Int64Type();
                default: return null;
            }
        }

        public ASTType visit(BooleanType t)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(DoubleType t)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(IdentifierType id)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(Int16Type t)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(Int64Type t)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(UInt8Type t)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(UInt32Type t)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(TupleType t)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(Call n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(Label n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(Move n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(Pop n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(Ret n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(Debug n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(MakeGlobal n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(BuiltinFunctionSymbol n)
        {
            return n.ReturnValue;
        }

        public ASTType visit(FunctionSymbol n)
        {
            return n.ReturnValue;
        }

        public ASTType visit(VariableSymbol n)
        {
            return n.Type;
        }

        public ASTType visit(ConstantSymbol n)
        {
            return n.Type;
        }

        public ASTType visit(SectionCode n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(Sub n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(Push n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(Nope n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(Leave n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(Comment n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(Add n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(UInt64Type t)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(UInt16Type t)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(StringType t)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(Int32Type t)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(Int8Type t)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(FloatType t)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(CharType t)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(StringLiteral n)
        {
            return new StringType();
        }

        public ASTType visit(PowerExp n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(Identifier identifier)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(OrExp powerExp)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(ModuloExp n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(ExclamationExp n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(BitwiseComplementExp n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(VarDeclaration n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(ConstDeclaration n)
        {
            if (n.Type == null)
                return n.RHS.accept(this);
            else
                return n.Type;
        }

        public ASTType visit(Assignment n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(Register n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(OctalConstant n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(ByteConstant n)
        {
            throw new NotImplementedException();
        }

        public ASTType visit(IntegerConstant n)
        {
            throw new NotImplementedException();
        }
    }
}