﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift
{
    class SymbolVisitor : Visitor
    {
        Table table;
        public SymbolVisitor(Table table)
        {
            this.table = table;
        }
        Exp Visitor.visit(DivisionExp n)
        {
            throw new NotImplementedException();
        }

        Exp Visitor.visit(MinusExp n)
        {
            throw new NotImplementedException();
        }

        Exp Visitor.visit(MultiplicationExp n)
        {
            throw new NotImplementedException();
        }

        Exp Visitor.visit(Identifier identifier)
        {
            return identifier.
        }

        Exp Visitor.visit(PowerExp n)
        {
            throw new NotImplementedException();
        }

        Exp Visitor.visit(IntegerLiteral n)
        {
            throw new NotImplementedException();
        }

        Exp Visitor.visit(PlusExp n)
        {
            throw new NotImplementedException();
        }

        Exp Visitor.visit(OrExp powerExp)
        {
            throw new NotImplementedException();
        }

        Exp Visitor.visit(ModuloExp n)
        {
            throw new NotImplementedException();
        }

        Exp Visitor.visit(ExclamationExp n)
        {
            throw new NotImplementedException();
        }

        Exp Visitor.visit(BitwiseComplementExp n)
        {
            throw new NotImplementedException();
        }

        Exp Visitor.visit(AndExp n)
        {
            throw new NotImplementedException();
        }
    }
}