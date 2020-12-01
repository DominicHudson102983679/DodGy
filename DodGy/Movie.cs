using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DodGy
{
    
    public class Movie
    {      
        public int MovieNo { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public int RunTime { get; set; }

        public Movie()
        {
            this.MovieNo = 0;
            this.Title = "";
            this.ReleaseYear = 0;
            this.RunTime = 0;
        }

        public Movie(int mnum, string title, int relyear, int rtime){
            this.MovieNo = mnum;
            this.Title = title;
            this.ReleaseYear = relyear;
            this.RunTime = rtime;
        }

        public int NumActors(int MovieNo) {
            string connectionString = 
            @"Data Source=rpsdb.cgluvdnfm6uc.us-east-1.rds.amazonaws.com; 
            Initial Catalog=Movies;
            User ID=admin;
            Password=password";

            SqlConnection con = new SqlConnection(connectionString);

            string queryString = "SELECT COUNT(*) FROM CASTING WHERE MovieNo = @MovieNo";
            SqlCommand command = new SqlCommand(queryString, con);
            command.Parameters.AddWithValue("@MovieNo", (int)MovieNo);

            var numActors = 0;
            con.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    numActors = (int)reader[0];
                }
            }

            return numActors;
        }
        
        public int GetAge(int MovieNo) {
            string connectionString = 
            @"Data Source=rpsdb.cgluvdnfm6uc.us-east-1.rds.amazonaws.com; 
            Initial Catalog=Movies;
            User ID=admin;
            Password=password";

            SqlConnection con = new SqlConnection(connectionString);

            string queryString = "SELECT CAST(YEAR(GETDATE()) AS INT) - RELYEAR FROM MOVIE WHERE MOVIENO = @MovieNo";
            SqlCommand command = new SqlCommand(queryString, con);
            command.Parameters.AddWithValue("@MovieNo", (int)MovieNo);

            var age = 0;
            con.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    age = (int)reader[0];
                }
            }

            return age;
        }

        


        
    }
}