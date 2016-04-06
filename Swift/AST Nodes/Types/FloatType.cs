﻿using Swift.Tokens;

namespace Swift.AST_Nodes.Types
{
    public class FloatType : ASTType
    {
        public FloatType()
        {
        }
        public override void accept(Visitor v)
        {
            v.visit(this);
        }
    }
}