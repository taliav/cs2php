﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lang.Cs.Compiler.Visitors
{
    public class ExpressionConverterVisitor
    {
        #region Static Methods

        // Public Methods 

        public static IValue Visit(IValue src)
        {


            if (src.GetType() == typeof(StaticMemberAccessExpression))
            {
                var xx = src as StaticMemberAccessExpression;
                var fieldInfo = xx.Expression.DotnetType.GetField(xx.MemberName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (fieldInfo != null)
                {
                    throw new NotSupportedException();
                    // return Visit(new ClassFieldAccessExpression(fieldInfo));
                }
                var propertyInfo = xx.Expression.DotnetType.GetProperty(xx.MemberName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (propertyInfo != null)
                    return Visit(new CsharpInstancePropertyAccessExpression(propertyInfo, null));
                return src;
            }
            if (src.GetType() == typeof(InstanceMemberAccessExpression))
            {
                var xx = src as InstanceMemberAccessExpression;
                var t = xx.Expression.ValueType;
                var fieldInfo = t.GetField(xx.MemberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (fieldInfo != null)
                    return Visit(new InstanceFieldAccessExpression(fieldInfo, xx.Expression));

                var propertyInfo = t.GetProperty(xx.MemberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (propertyInfo != null)
                    return Visit(new CsharpInstancePropertyAccessExpression(propertyInfo, xx.Expression));
                return src;
            }

            if (src.GetType() == typeof(MemberAccessExpression))
            {
                var xx = src as MemberAccessExpression;
                if (xx.Expression is TypeValue)
                {
                    var yy = xx.Expression as TypeValue;
                    var tmp = new StaticMemberAccessExpression(xx.MemberName, new LangType(yy.DotnetType));
                    return Visit(tmp);

                }
                /*
                if (xx.Expression is LangType)
                {
                    var yy = xx.Expression as LangType;
                    var tmp = new StaticMemberAccessExpression(xx.MemberName, yy);
                    return Visit(tmp);
                } */
            }
            if (src is UnknownIdentifierValue)
            {

            }
            return src;
            throw new NotSupportedException();
        }

        #endregion Static Methods
    }
}
