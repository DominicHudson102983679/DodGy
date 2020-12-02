using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
                    string queryString = "select * from is.information_schema.tables where table_type = 'base table'";
                    using (SqlCommand command = new SqlCommand(queryString, con))
                    {
                        con.Open();
                        var result = command.ExecuteNonQuery();
                        return result.ToString();
                    }
                }
           } catch (Exception e){
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

            string queryString = "select * from movie";
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

            string queryString = "select title from movie where title like 'The %'";
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
            
            @"select title from
            (select c.actorno, c.movieno, m.title

            from ((casting c
            
            inner join actor a
            on c.actorno = a.actorno)
                                
            inner join movie m 
            on m.movieno = c.movieno))

            s inner join (
            select actorno from actor where fullname = 'Luke Wilson')
            
            a on s.actorno = a.actorno
            where s.actorno = a.actorno;";

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

            con.Close();

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

            string queryString = "select * from movie";
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
        // works, updates Speed in movies.sql to a runtime of 1000 minutes
        /*
            
            {
                "Runtime": 1000,
                "title": "Speed"
            }

            select * from Movie
            where runtime > 500;

        */ 
        [HttpPost("ChangeRuntime")]
        public string ChangeRuntime(Movie m){
            string connectionString = 
            @"Data Source=rpsdb.cgluvdnfm6uc.us-east-1.rds.amazonaws.com; 
            Initial Catalog=Movies;
            User ID=admin;
            Password=password";

            SqlConnection con = new SqlConnection(connectionString);

            string queryString = "update movie set runtime = @Runtime where title = @title";
            SqlCommand command = new SqlCommand(queryString, con);
            command.Parameters.AddWithValue("@Runtime", (int)m.RunTime);
            command.Parameters.AddWithValue("@title", m.Title);

            con.Open();
            var result = command.ExecuteNonQuery();

            return result.ToString() + " rows updated in Movie table in Movies.sql";
        }

        public class Object{
        public string oGivenname { get; set;}
        public string oSurname { get; set;}
        public string oNewSurname { get; set;}

        public Object(){
            this.oGivenname = "";
            this.oSurname = "";
            this.oNewSurname = "";
        }
    }

        // https://localhost:5001/Movie/ChangeSurname
        // works, changes Ryan Reynolds fullname in the database to Ryan PurpleMonkeyDishwasher
        /* 
        {
            "oGivenname": "Ryan",
            "oSurname": "Reynolds",
            "oNewSurname": "PurpleMonkeyDishwasher"
        }

        select * from actor
        where fullname LIKE 'Ryan PurpleMonkeyDishwasher';

        */
        [HttpPost("ChangeSurname")]
        public string ChangeSurname(Object o){
            string connectionString = 
            @"Data Source=rpsdb.cgluvdnfm6uc.us-east-1.rds.amazonaws.com; 
            Initial Catalog=Movies;
            User ID=admin;
            Password=password";

            SqlConnection con = new SqlConnection(connectionString);

            string queryString = "update actor set fullName = @givenName + ' ' + @newSurname where givenName = @givenName and surname = @surname;";
            SqlCommand command = new SqlCommand(queryString, con);
            command.Parameters.AddWithValue("@givenName",  o.oGivenname);
            command.Parameters.AddWithValue("@surname", o.oSurname);
            command.Parameters.AddWithValue("@newSurname", o.oNewSurname);

            con.Open();
            var result = command.ExecuteNonQuery();

            return result.ToString() + " rows updated in Actor table in Movies.sql";
        }

        // https://localhost:5001/Movie/CreateMovie
        // works, creates movie and inserts into movie table
        /*

        {
            "MovieNo": 111112,
            "Title": "test test test",
            "ReleaseYear": 4000,
            "Runtime" : 1
        }

        select * from movie
        where relyear = 4000;

        */

        [HttpPost("CreateMovie")]
        public string CreateMovie(Movie m){
            string connectionString = 
            @"Data Source=rpsdb.cgluvdnfm6uc.us-east-1.rds.amazonaws.com; 
            Initial Catalog=Movies;
            User ID=admin;
            Password=password";

            SqlConnection con = new SqlConnection(connectionString);

            string queryString = "insert into movie (movieno, title, relyear, runtime) values (@no, @title , @year ,@time)";
            SqlCommand command = new SqlCommand(queryString, con);
            command.Parameters.AddWithValue("@no",  m.MovieNo);
            command.Parameters.AddWithValue("@title", m.Title);
            command.Parameters.AddWithValue("@year", m.ReleaseYear);
            command.Parameters.AddWithValue("@time", m.RunTime);

            con.Open();
            var result = command.ExecuteNonQuery();

            return result.ToString() + " rows updated in Movie table in Movies.sql";
        }

        // https://localhost:5001/Movie/CreateActor
        // works, creates new actor
        /*

        {
            "ActorNo": 123456,
            "FullName": "Crom Tuise",
            "GivenName": "Crom",
            "Surname" : "Tuise"
        }

        select * from actor
        where GIVENNAME LIKE 'Crom';

        */

        [HttpPost("CreateActor")]
        public string CreateActor(Actor a){
            string connectionString = 
            @"Data Source=rpsdb.cgluvdnfm6uc.us-east-1.rds.amazonaws.com; 
            Initial Catalog=Movies;
            User ID=admin;
            Password=password";

            SqlConnection con = new SqlConnection(connectionString);

            string queryString = "insert into actor (actorNo,fullName,givenName,surname) values (@no, @fname, @gname, @surname)";
            SqlCommand command = new SqlCommand(queryString, con);
            command.Parameters.AddWithValue("@no",  a.ActorNo);
            command.Parameters.AddWithValue("@fname", a.FullName);
            command.Parameters.AddWithValue("@gname", a.GivenName);
            command.Parameters.AddWithValue("@surname", a.Surname);

            con.Open();
            var result = command.ExecuteNonQuery();

            return result.ToString() + " rows updated in Actor table in Movies.sql";
        }

        public class IntObj{
        public int ioId {get; set;}
        public int ioAnum {get; set;}
        public int ioMnum {get; set;}

        public IntObj(){
            this.ioId = 0;
            this.ioAnum = 1;
            this.ioMnum = 2;
        }
    }

        // https://localhost:5001/Movie/CastActor
        // works, creates casting and puts actor into movie
        /*

        {
            "ioId": 123456,
            "ioAnum": 123456,
            "ioMnum": 324668
        }

        select * 
        from Actor a
        inner join Casting c
        On a.ACTORNO = c.ACTORNO
        where MOVIENO = 324668     

        */
        [HttpPost("CastActor")]
        public string CastActor(IntObj o){
            string connectionString = 
            @"Data Source=rpsdb.cgluvdnfm6uc.us-east-1.rds.amazonaws.com; 
            Initial Catalog=Movies;
            User ID=admin;
            Password=password";

            SqlConnection con = new SqlConnection(connectionString);

            string queryString = "insert into casting (castid, actorno, movieno) values (@id, @aNum, @mNum)";
            SqlCommand command = new SqlCommand(queryString, con);
            command.Parameters.AddWithValue("@id", (int) o.ioId);
            command.Parameters.AddWithValue("@aNum", (int) o.ioAnum);
            command.Parameters.AddWithValue("@mNum", (int)o.ioMnum);
           

            con.Open();
            var result = command.ExecuteNonQuery();

            return result.ToString() + " rows updated in Casting table in Movies.sql";
        }
        
    }
}


