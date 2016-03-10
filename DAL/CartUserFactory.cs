using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using B24.Common;

namespace B24.Sales4.DAL
{
  /// <summary>
  /// Summary description for CartUserFactory
  /// </summary>
  public class CartUserFactory : UserFactory
  {
    #region Constructors
    public CartUserFactory() : base() { }
    public CartUserFactory(string connStr) : base(connStr) { }
    #endregion Constructors

    #region Public Methods
    /// <summary>
    /// Get an individual user by userid
    /// </summary>
    /// <param name="userid">The user's userid to load</param>
    /// <returns>a B24.Sales4.CartUser object</returns>
    public new CartUser GetUserByID(Guid userid)
    {
      Verify(ConnectionString, "ConnectionString");
      Verify(userid, "userid");

      try
      {
        using (SqlConnection conn = new SqlConnection(this.ConnectionString))
        {
          string query = "";
          query += "select userid, firstname, lastname, login, email, s.subscriptionid, lastlogin, DisplayLogin=BaseLogin, passwordroot,c.Name as CompanyName,l.IngenUserID ";
          query += "  from libuser l with(nolock)";
          query += "  join person p with(nolock) on p.personid = l.personid";
          query += "  left outer join subscription s with(nolock) on s.subscriptionid = l.subscriptionid";
          query += "  left outer join company c with(nolock) on c.companyid = s.companyid";

          query += "  where userid = @userid";

          SqlCommand cmd = new SqlCommand(query, conn);
          cmd.Parameters.Add("@UserID", SqlDbType.UniqueIdentifier).Value = userid;
          cmd.CommandType = CommandType.Text;
          conn.Open();
          SqlDataReader reader = cmd.ExecuteReader();
          CartUser u = new CartUser();
          while (reader.Read())
          {
            u = new CartUser(reader);
          }
          return u;
        }
      }
      catch (SqlException e)
      {
        LogSQLException(e);
        throw;
      }
    }


    /// <summary>
    /// Move the given user to a different sub
    /// </summary>
    /// <param name="subID">The target subscription</param>
    /// <param name="userID">The user to move</param>
    /// <param name="requesterID">ID of the sales user requesting this action</param>
    public void MoveUserToNewSub(Guid subID, Guid userID, Guid requesterID)
    {
      Verify(ConnectionString, "ConnectionString");
      Verify(userID, "userID");
      Verify(subID, "subID");
      Verify(requesterID, "requesterID");

      try
      {
        using (SqlConnection conn = new SqlConnection(this.ConnectionString))
        {
          SqlCommand cmd = new SqlCommand("B24_MoveUserToNewSubscription", conn);
          cmd.Parameters.Add("@SubscriptionID", SqlDbType.UniqueIdentifier).Value = subID;
          cmd.Parameters.Add("@UserID", SqlDbType.UniqueIdentifier).Value = userID;
          cmd.Parameters.Add("@RequesterUserID", SqlDbType.UniqueIdentifier).Value = requesterID;
          cmd.CommandType = CommandType.StoredProcedure;
          conn.Open();
          cmd.ExecuteNonQuery();
        }
      }
      catch (SqlException e)
      {
        LogSQLException(e);
        throw;
      }
    }

#endregion Public Methods

  }
}