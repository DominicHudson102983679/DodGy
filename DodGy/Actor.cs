using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DodGy;
// using Newtonsoft.Json;

namespace DodGy
{
    public class Actor
    {
        
        public int ActorNo { get; set; }
        public string FullName { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }

        public Actor() {
            
            this.ActorNo = 0;
            this.FullName = "";
            this.GivenName = "";
            this.Surname = "";
        }

        public Actor(int acnum, string fname, string gname, string sname) {
            this.ActorNo = acnum;
            this.FullName = fname;
            this.GivenName = gname;
            this.Surname = sname;
        }

        public string setFullName(int acnum)
        {
            string connectionString = 
            @"Data Source=rpsdb.cgluvdnfm6uc.us-east-1.rds.amazonaws.com; 
            Initial Catalog=Movies;
            User ID=admin; 
            Password=password";

            SqlConnection con = new SqlConnection(connectionString);

            string queryString = "SELECT GivenName, Surname FROM ACTOR WHERE ActorNo = @ActorNo";
            SqlCommand command = new SqlCommand(queryString, con);
            command.Parameters.AddWithValue("@ActorNo", (int)acnum);

            var fullname = "";
            con.Open();

            using (SqlDataReader reader = command.ExecuteReader()){
                while (reader.Read()){
                    fullname = reader[0].ToString() + " " + reader[1].ToString();
                }
            }

            return fullname;
        }

    }
}