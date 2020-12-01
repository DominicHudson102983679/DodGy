using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DodGy;
using System.Data.SqlClient;


namespace DodGy.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        public string ConStringWally {get; set;}
        public string ConnectionString {get; set;}
        public List<Movie> Movies {get; set;} 
        public List<string> MovieTitles {get; set;} 

        public MovieController(){
            this.ConStringWally = 
            @"no.database.here.com;
            Initial Catalog=Is;
            User ID=Wally; 
            Password=Where";
            this.ConnectionString = 
            @"Data Source=rpsdb.cgluvdnfm6uc.us-east-1.rds.amazonaws.com; 
            Initial Catalog=Movies;
            User ID=admin;
            Password=password";

            
            this.Movies = new List<Movie>();
            this.MovieTitles = new List<string>();
        }

        // https://localhost:5001/Movie/ExceptionTask
        // ERROR: Keyword not supported: 'no.database.here.com; initial catalog'.

        [HttpGet("ExceptionTask")]
        public string ExceptionTask()
        { 
          try{
               using (SqlConnection con = new SqlConnection(this.ConStringWally))
                {
                    string queryString = " SELECT * FROM Is.INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' GO";
                    using (SqlCommand command = new SqlCommand(queryString, con))
                    {
                        con.Open();
                        var result = command.ExecuteNonQuery();
                        return result.ToString();
                    }
                }
           }catch (ArgumentException e){
              return "ERROR: " + e.Message;
           }

        }

        // https://localhost:5001/Movie/ReadAllMovies
        // works
        [HttpGet("ReadAllMovies")]
        public List<Movie> ReadAllMovies(){
            string connectionString = 
            @"Data Source=rpsdb.cgluvdnfm6uc.us-east-1.rds.amazonaws.com; 
            Initial Catalog=Movies;
            User ID=admin;
            Password=password";

            SqlConnection con = new SqlConnection(connectionString);

            string queryString = "SELECT * FROM MOVIE";
            SqlCommand command = new SqlCommand(queryString, con);

            con.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Movies.Add(
                        new Movie(Convert.ToInt32(reader[0]), reader[1].ToString(), Convert.ToInt32(reader[2]), Convert.ToInt32(reader[3]))
                    );
                }
            }

            return this.Movies;
        }

        // https://localhost:5001/Movie/FindTheMovie
        // ["The Lord of the Rings: The Fellowship of the Ring","The Lord of the Rings: The Two Towers","The Lord of the Rings: The Return of the King",
        // "The Dark Knight","The Silence of the Lambs","The Shawshank Redemption","The Devil Wears Prada","The Life Aquatic With Steve Zissou",
        // "There's Something About Mary","The Mummy","The Matrix","The Matrix Reloaded","The Matrix Revolutions","The Usual Suspects","The Sixth Sense",
        // "The Fog","The Departed","Thelma & Louise","The Island","The Amazing Spider-Man","The Bourne Identity","The Aviator","The Fugitive",
        // "The 40 Year Old Virgin","The Bucket List","The Lovely Bones","The Truth About Cats & Dogs","The Sting","The Bone Collector","The Italian Job",
        // "The Black Dahlia","The Princess Diaries","The Producers","The Ides of March","The Italian Job","The Doors","The Wedding Singer","The Odd Couple",
        // "The Pink Panther","The Big Chill","The Hangover","The Cabin in the Woods","The Invention of Lying","The Avengers","The Expendables",
        // "The Accidental Tourist","The A-Team","The Rocky Horror Picture Show","The Firm","The Graduate","The Social Network","The Hangover Part II",
        // "The Dark Knight Rises","The Bourne Legacy","The Hobbit: An Unexpected Journey","The Lone Ranger","The Girl with the Dragon Tattoo","The Hunger Games",
        // "The Artist","The Expendables 2","The Watch","The Impossible","The Words","The Hunger Games: Catching Fire","The Amazing Spider-Man 2",
        // "The Grand Budapest Hotel","The Hunger Games: Mockingjay - Part 1","The Hunger Games: Mockingjay - Part 2","The Heat","The Monuments Men",
        // "The Man from U.N.C.L.E.","The Intern","The Martian","The Nice Guys"]

        [HttpGet("FindTheMovie")]
        public List<string> FindTheMovie(){
            string connectionString = 
            @"Data Source=rpsdb.cgluvdnfm6uc.us-east-1.rds.amazonaws.com; 
            Initial Catalog=Movies;
            User ID=admin;
            Password=password";

            SqlConnection con = new SqlConnection(connectionString);

            string queryString = "SELECT TITLE FROM MOVIE WHERE TITLE LIKE 'The%';";
            SqlCommand command = new SqlCommand(queryString, con);

            con.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    MovieTitles.Add(
                        reader[0].ToString()
                    );
                }
            }

            return this.MovieTitles;
        }

        // https://localhost:5001/Movie/CastedInTitles
        // ["Anchorman: The Legend of Ron Burgundy","Charlie's Angels: Full Throttle","Charlie's Angels"]
        [HttpGet("CastedInTitles")]
        public List<string> CastedInTitles(){
            string connectionString = 
            @"Data Source=rpsdb.cgluvdnfm6uc.us-east-1.rds.amazonaws.com; 
            Initial Catalog=Movies;
            User ID=admin;
            Password=password";

            SqlConnection con = new SqlConnection(connectionString);

            string queryString = 
            @"SELECT TITLE FROM
            (SELECT C.ACTORNO, C.MOVIENO, M.TITLE

            FROM  ((CASTING C
            
            INNER JOIN ACTOR A 
            ON C.ACTORNO = A. ACTORNO)
                                
            INNER JOIN MOVIE M 
            ON M.MOVIENO = C.MOVIENO))

            S INNER JOIN (
            SELECT ACTORNO FROM ACTOR WHERE FULLNAME = 'Luke Wilson') 
            
            A ON S.ACTORNO = A.ACTORNO
            WHERE S.ACTORNO = A.ACTORNO;";

            SqlCommand command = new SqlCommand(queryString, con);

            con.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    MovieTitles.Add(
                        reader[0].ToString()
                    );
                }
            }

            return this.MovieTitles;
        }

        // https://localhost:5001/Movie/AllMoviesRunTime
        // 37736
        [HttpGet("AllMoviesRunTime")]
        public int AllMoviesRunTime(){
            string connectionString = 
            @"Data Source=rpsdb.cgluvdnfm6uc.us-east-1.rds.amazonaws.com; 
            Initial Catalog=Movies;
            User ID=admin;
            Password=password";

            SqlConnection con = new SqlConnection(connectionString);

            string queryString = "SELECT * FROM MOVIE";
            SqlCommand command = new SqlCommand(queryString, con);

            con.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Movies.Add(
                        new Movie(Convert.ToInt32(reader[0]), reader[1].ToString(), Convert.ToInt32(reader[2]), Convert.ToInt32(reader[3]))
                    );
                }
            }

            var total = 0;
            foreach(Movie m in this.Movies){
                total += m.RunTime;
            }
            
            return total;
        }

        // https://localhost:5001/Movie/ChangeRuntime
        [HttpPost("ChangeRuntime")]
        public string ChangeRuntime(Movie m){
            string connectionString = 
            @"Data Source=rpsdb.cgluvdnfm6uc.us-east-1.rds.amazonaws.com; 
            Initial Catalog=Movies;
            User ID=admin;
            Password=password";

            SqlConnection con = new SqlConnection(connectionString);

            string queryString = "UPDATE MOVIE SET RUNTIME = @Runtime WHERE TITLE = @title";
            SqlCommand command = new SqlCommand(queryString, con);
            command.Parameters.AddWithValue("@Runtime", (int)m.RunTime);
            command.Parameters.AddWithValue("@title", m.Title);

            con.Open();
            var result = command.ExecuteNonQuery();

            return "Updated " + result.ToString() + " row";
        }

        public class Obj{
        public string firstVal { get; set;}
        public string secondVal { get; set;}
        public string thirdVal { get; set;}

        public Obj(){
            this.firstVal = "";
            this.secondVal = "";
            this.thirdVal = "";
        }
    }

        // https://localhost:5001/Movie/ChangeSurname
        [HttpPost("ChangeSurname")]
        public string ChangeSurname(Obj o){
            string connectionString = 
            @"Data Source=rpsdb.cgluvdnfm6uc.us-east-1.rds.amazonaws.com; 
            Initial Catalog=Movies;
            User ID=admin;
            Password=password";

            SqlConnection con = new SqlConnection(connectionString);

            string queryString = "UPDATE ACTOR SET FullName = @givenName + ' ' + @newSurname WHERE GivenName = @givenName AND Surname = @surname;";
            SqlCommand command = new SqlCommand(queryString, con);
            command.Parameters.AddWithValue("@givenName",  o.firstVal);
            command.Parameters.AddWithValue("@surname", o.secondVal);
            command.Parameters.AddWithValue("@newSurname", o.thirdVal);

            con.Open();
            var result = command.ExecuteNonQuery();

            return "Updated row: (" + result.ToString() + ") in database";
        }

        // https://localhost:5001/Movie/CreateMovie
        [HttpPost("CreateMovie")]
        public string CreateMovie(Movie m){
            string connectionString = 
            @"Data Source=rpsdb.cgluvdnfm6uc.us-east-1.rds.amazonaws.com; 
            Initial Catalog=Movies;
            User ID=admin;
            Password=password";

            SqlConnection con = new SqlConnection(connectionString);

            string queryString = "INSERT INTO MOVIE (MOVIENO, TITLE, RELYEAR, RUNTIME) VALUES (@no, @title , @year ,@time)";
            SqlCommand command = new SqlCommand(queryString, con);
            command.Parameters.AddWithValue("@no",  m.MovieNo);
            command.Parameters.AddWithValue("@title", m.Title);
            command.Parameters.AddWithValue("@year", m.ReleaseYear);
            command.Parameters.AddWithValue("@time", m.RunTime);

            con.Open();
            var result = command.ExecuteNonQuery();

            return "Updated row: (" + result.ToString() + ") in database";
        }

        // https://localhost:5001/Movie/CreateActor
        [HttpPost("CreateActor")]
        public string CreateActor(Actor a){
            string connectionString = 
            @"Data Source=rpsdb.cgluvdnfm6uc.us-east-1.rds.amazonaws.com; 
            Initial Catalog=Movies;
            User ID=admin;
            Password=password";

            SqlConnection con = new SqlConnection(connectionString);

            string queryString = "INSERT INTO ACTOR (ActorNo,FullName,GivenName,Surname) values(@no, @fname, @gname, @surname)";
            SqlCommand command = new SqlCommand(queryString, con);
            command.Parameters.AddWithValue("@no",  a.ActorNo);
            command.Parameters.AddWithValue("@fname", a.FullName);
            command.Parameters.AddWithValue("@gname", a.GivenName);
            command.Parameters.AddWithValue("@surname", a.Surname);

            con.Open();
            var result = command.ExecuteNonQuery();

            return "Updated row: (" + result.ToString() + ") in database";
        }

        public class IntObj{
        public int firstVal {get; set;}
        public int secondVal {get; set;}
        public int thirdVal {get; set;}

        public IntObj(){
            this.firstVal = 0;
            this.secondVal = 1;
            this.thirdVal = 2;
        }
    }

        // https://localhost:5001/Movie/CastActor
        [HttpPost("CastActor")]
        public string CastActor(IntObj o){
            string connectionString = 
            @"Data Source=rpsdb.cgluvdnfm6uc.us-east-1.rds.amazonaws.com; 
            Initial Catalog=Movies;
            User ID=admin;
            Password=password";

            SqlConnection con = new SqlConnection(connectionString);

            string queryString = "INSERT INTO CASTING (Castid, ActorNo, MovieNo) values (@id, @aNum, @mNum)";
            SqlCommand command = new SqlCommand(queryString, con);
            command.Parameters.AddWithValue("@id", (int) o.firstVal);
            command.Parameters.AddWithValue("@aNum", (int) o.secondVal);
            command.Parameters.AddWithValue("@mNum", (int)o.thirdVal);
           

            con.Open();
            var result = command.ExecuteNonQuery();

            return "Updated row: (" + result.ToString() + ") in database";
        }
        
    }
}


