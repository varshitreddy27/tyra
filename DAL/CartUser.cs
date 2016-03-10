using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using B24.Common;

namespace B24.Sales4.DAL
{
  /// <summary>
  /// Summary description for CartUser
  /// </summary>
  public class CartUser : User
  {
    #region Private Fields
    private string passwordRoot;
    #endregion Private Fields

    #region Constructors
    public CartUser() : base() { }

    #region Public Members
    public string PasswordRoot
    {
      get { return passwordRoot; }
      set { passwordRoot = value; }
    }
    #endregion Public Members

    /// <summary>
    /// Instantiate a User from a row in the current record set
    /// </summary>
    /// <param name="reader">SqlDataReader pointing to current row</param>
    public CartUser(SqlDataReader reader)
      : base(reader)
    {
      initialize(reader);
    }
    #endregion Constructors



    #region Private Methods

    /// <summary>
    /// Data exchange for user object
    /// </summary>
    /// <param name="reader">SqlDataReader pointing to current row</param>
    private void initialize(SqlDataReader reader)
    {

      passwordRoot = ConvertDBNull.To<string>(reader["PasswordRoot"], null);
    }


    #endregion Private Methods


  }
}