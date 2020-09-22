using COSEC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace COSEC.Repositories
{
    public class DBAccess
    {
        string conn = DBConnection.Connection;

        public bool CheckLogin(Login user)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
                {
                    return false;
                }
                SqlCommand cmd = new SqlCommand("sp_login", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                connection.Open();
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@password", user.Password);
                object read = cmd.ExecuteScalar();
                int Var = Convert.ToInt32(read);
                connection.Close();
                return Var == 1;
            }
        }

        public void ApproveUser(int id)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand cmd = new SqlCommand("sp_approve_user", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                connection.Open();
                cmd.Parameters.AddWithValue("@Id", id);
                object read = cmd.ExecuteScalar();
                int Var = Convert.ToInt32(read);
                connection.Close();
            }
        }

        public void RejectUser(int id)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand cmd = new SqlCommand("sp_reject_user", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                connection.Open();
                cmd.Parameters.AddWithValue("@Id", id);
                object read = cmd.ExecuteScalar();
                int Var = Convert.ToInt32(read);
                connection.Close();
            }
        }

        public void ApproveFinalUser(int id)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand cmd = new SqlCommand("sp_approve_final_user", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                connection.Open();
                cmd.Parameters.AddWithValue("@Id", id);
                object read = cmd.ExecuteScalar();
                int Var = Convert.ToInt32(read);
                connection.Close();
            }
        }

        public void RejectFinalUser(int id)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand cmd = new SqlCommand("sp_reject_final_user", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                connection.Open();
                cmd.Parameters.AddWithValue("@Id", id);
                object read = cmd.ExecuteScalar();
                int Var = Convert.ToInt32(read);
                connection.Close();
            }
        }
    }
}
