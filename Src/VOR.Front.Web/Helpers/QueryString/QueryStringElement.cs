using System;


public class QueryStringElement
{
    #region Private Fields

    private string _Name;
    private string _Value;
    #endregion

    #region Public Properties

    public string Name
    {
        get { return _Name; }
        set { _Name = value; }
    }


    public string Value
    {
        get { return _Value; }
        set { _Value = value; }
    }
    #endregion
}
