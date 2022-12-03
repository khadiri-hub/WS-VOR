using System;
using System.Collections.Generic;


/// <summary>
/// 
/// </summary>
public sealed class QueryStringParametersCollection : List<QueryStringElement>
{
    public QueryStringElement this[string queryStringName]
    {
        get { return this.Find(delegate(QueryStringElement e) { return e.Name == queryStringName; }); }
    }

    public QueryStringParametersCollection()
    {
    }
}
