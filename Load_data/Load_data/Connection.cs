using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web;
using System.IO;
using LINQtoCSV;
using System.Data.Linq.Mapping;
using Newtonsoft.Json;


namespace Load_data
{
    internal class Connection
    {
        string[] arr;
        SqlConnection _connection;
        string Error_path;

        internal Connection()
        {
            arr = new string[5] { "yelp_academic_dataset_review.JSON",
                                           "yelp_academic_dataset_tip.JSON",
                                           "yelp_academic_dataset_checkin.JSON",
                                           "yelp_academic_dataset_user.JSON",
                                           "yelp_academic_dataset_business.JSON",
                                           };
            Error_path = "C:\\ErrorFile.txt";

        }


        internal string ConnectToDB()
        {
            string error = "";

            try
            {
                SqlConnectionStringBuilder sqlConnString = new SqlConnectionStringBuilder();             //ConnectionString

                sqlConnString.DataSource = "DESKTOP-HP464K5";     // Server
                sqlConnString.InitialCatalog = "analytics_yelp";                                 // BD            
                sqlConnString.ConnectTimeout = 150;                                                      //TimeOut
                sqlConnString.PersistSecurityInfo = false;                                              //Security ASK
                sqlConnString.UserID = "r_test";                                                         //Login
                sqlConnString.Password = "r_test";                                                   //PASS

                _connection = new SqlConnection(sqlConnString.ToString());                              //SQL Connection step 1
                _connection.Open();

            }
            catch (Exception e)
            {
                error = e.Message;
            }

            return error;
        }

        internal bool FilesExists(string Path)
        {
            bool rez = true;



            foreach (string s in arr)
            {
                string z1 = Path + "\\" + s;
                if (!File.Exists(z1))
                {
                    rez = false;
                    break;
                }
            }
            string z = (Path + "\\" + arr[0]);
            return rez;
        }


        internal string Start_Load(string Path, int i)
        {
            try
            { 
                switch (i)
                {
                    case 1:
                        DoWork(Path + "\\" + arr[0], 1);
                        break;
                    case 2:
                        DoWork(Path + "\\" + arr[1], 2);
                        break;
                    case 3:
                        DoWork(Path + "\\" + arr[4], 3);
                        break;
                    case 4:
                        DoWork(Path + "\\" + arr[3], 4);       
                        break;
                    case 5:
                        DoWork(Path + "\\" + arr[2], 5);
                        break;
                    case 6:
                        DoWork("", 6);
                        break;
                    default: break;
                }
                return "All ok";
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }


        internal void DoWork(string Path, int i) //static
        {
            const Int32 BufferSize = 1024;
            int breack_proc = 0;
            switch (i)
                {
                case 1:
                    using (var fileStream = File.OpenRead(Path))
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                    {
                        String line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            if (breack_proc == 10000) break; 
                            Review_base m1 = JsonConvert.DeserializeObject<Review_base>(line);
                            EnterData(m1);
                            breack_proc++;
                        }
                    }
                    break;
                case 2:
                    using (var fileStream = File.OpenRead(Path))
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                    {
                        String line;
                        Int64 r = 0;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                           // if (breack_proc == 100) break;
                            tip_base m1 = JsonConvert.DeserializeObject<tip_base>(line);

                            try
                            {
                                using (SqlCommand command = new SqlCommand(
                                    "INSERT INTO dbo.Tip_base VALUES(@text, @date, @likes, @business_id, @User_id, @type, @business_id_int, @user_id_int)", _connection))
                                {
                                  
                                    command.Parameters.Add(new SqlParameter("text", m1.text));
                                    command.Parameters.Add(new SqlParameter("date", m1.date));
                                    command.Parameters.Add(new SqlParameter("likes", m1.likes));
                                    command.Parameters.Add(new SqlParameter("business_id", m1.business_id));
                                    command.Parameters.Add(new SqlParameter("User_id", m1.user_id));
                                    command.Parameters.Add(new SqlParameter("type", m1.type));
                                    command.Parameters.Add(new SqlParameter("business_id_int", r));
                                    command.Parameters.Add(new SqlParameter("user_id_int", r));
                                    command.ExecuteNonQuery();
                                }
                            }
                            catch (Exception e)
                            {
                                using (SqlCommand command1 = new SqlCommand(
                                   "INSERT INTO dbo.Errors VALUES(@Date_insert,@determination, @Text_error)", _connection))
                                {
                                    command1.Parameters.Add(new SqlParameter("Date_insert", DateTime.Now));
                                    command1.Parameters.Add(new SqlParameter("determination", "dbo.Tip_base"));
                                    command1.Parameters.Add(new SqlParameter("Text_error", e.Message));
                                   
                                    command1.ExecuteNonQuery();
                                }
                            }
                            breack_proc++;
                        }
                    }
                    break;
                case 3:
                    using (var fileStream = File.OpenRead(Path))
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                    {
                        String line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            //if (breack_proc == 100) break;
                            business_base m1 = JsonConvert.DeserializeObject<business_base>(line);

                            try
                            {
                                using (SqlCommand command = new SqlCommand("dbo.Insert_buisness", _connection))
                                {
                                    command.CommandType = CommandType.StoredProcedure;

                                    command.Parameters.Add(new SqlParameter("business_id", m1.business_id));
                                    command.Parameters.Add(new SqlParameter("name", m1.name));
                                    command.Parameters.Add(new SqlParameter("neighborhood", m1.neighborhood));
                                    command.Parameters.Add(new SqlParameter("address", m1.address));
                                    command.Parameters.Add(new SqlParameter("city", m1.city));
                                    command.Parameters.Add(new SqlParameter("state", m1.state));

                                    string pcode = m1.postal_code == null || m1.postal_code == "" ? m1.postal_code = "x182" : m1.postal_code;

                                    command.Parameters.Add(new SqlParameter("postal_code", pcode));
                                    command.Parameters.Add(new SqlParameter("latitude", m1.latitude));
                                    command.Parameters.Add(new SqlParameter("longitude", m1.longitude));
                                    command.Parameters.Add(new SqlParameter("stars", m1.stars));
                                    command.Parameters.Add(new SqlParameter("review_count", m1.review_count));
                                    command.Parameters.Add(new SqlParameter("is_open", m1.is_open));

                                    string atribut;
                                    if (m1.attributes == null) { atribut = "0"; }
                                    else
                                    { 
                                        atribut = m1.attributes[0];
                                        for (int j = 1;j< m1.attributes.Length;j++)
                                        {
                                            atribut += ("," + m1.attributes[j]);
                                        }
                                    }
                                    command.Parameters.Add(new SqlParameter("attributes", atribut));


                                    string categories;
                                    if (m1.categories == null) { categories = "0"; }
                                    else
                                    {
                                        categories = m1.categories[0];
                                        for (int j = 1; j < m1.categories.Length; j++)
                                        {
                                            categories += ("," + m1.categories[j]);
                                        }
                                    }
                                    command.Parameters.Add(new SqlParameter("categories", categories));


                                    string hours;
                                    if (m1.hours == null) { hours = "0"; }
                                    else
                                    {
                                        hours = m1.hours[0];
                                        for (int j = 1; j < m1.hours.Length; j++)
                                        {
                                            hours += ("," + m1.hours[j]);
                                        }
                                    }
                                    command.Parameters.Add(new SqlParameter("hours", hours));

                                    

                                    command.ExecuteNonQuery();
                                }
                            }
                            catch (Exception e)
                            {
                                using (SqlCommand command1 = new SqlCommand(
                                   "INSERT INTO dbo.Errors VALUES(@Date_insert,@determination, @Text_error)", _connection))
                                {
                                    command1.Parameters.Add(new SqlParameter("Date_insert", DateTime.Now));
                                    command1.Parameters.Add(new SqlParameter("determination", "business_base"));
                                    command1.Parameters.Add(new SqlParameter("Text_error", e.Message));

                                    command1.ExecuteNonQuery();
                                }
                            }
                            breack_proc++;
                        }
                    }
                    break;

                case 4:
                    using (var fileStream = File.OpenRead(Path))
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                    {
                        String line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                           // if (breack_proc == 100) break;
                            user_base m1 = JsonConvert.DeserializeObject<user_base>(line);

                            try
                            {
                                using (SqlCommand command = new SqlCommand("dbo.Insert_user", _connection))
                                {
                                    command.CommandType = CommandType.StoredProcedure;

                                    command.Parameters.Add(new SqlParameter("User_id", m1.user_id));
                                    command.Parameters.Add(new SqlParameter("User_Name", m1.name));
                                    command.Parameters.Add(new SqlParameter("review_count", m1.review_count));
                                    command.Parameters.Add(new SqlParameter("yelping_since", m1.yelping_since));
                                    command.Parameters.Add(new SqlParameter("useful", m1.useful));
                                    command.Parameters.Add(new SqlParameter("funny", m1.funny));
                                    command.Parameters.Add(new SqlParameter("cool", m1.cool));
                                    command.Parameters.Add(new SqlParameter("fans", m1.fans));
                                    command.Parameters.Add(new SqlParameter("average_stars", m1.average_stars));
                                    command.Parameters.Add(new SqlParameter("compliment_hot", m1.compliment_hot));
                                    command.Parameters.Add(new SqlParameter("compliment_more", m1.compliment_more));
                                    command.Parameters.Add(new SqlParameter("compliment_profile", m1.compliment_profile));
                                    command.Parameters.Add(new SqlParameter("compliment_cute", m1.compliment_cute));
                                    command.Parameters.Add(new SqlParameter("compliment_list", m1.compliment_list));
                                    command.Parameters.Add(new SqlParameter("compliment_note", m1.compliment_note));
                                    command.Parameters.Add(new SqlParameter("compliment_plain", m1.compliment_plain));
                                    command.Parameters.Add(new SqlParameter("compliment_cool", m1.compliment_cool));
                                    command.Parameters.Add(new SqlParameter("compliment_funny", m1.compliment_funny));
                                    command.Parameters.Add(new SqlParameter("compliment_writer", m1.compliment_writer));
                                    command.Parameters.Add(new SqlParameter("compliment_photos", m1.compliment_photos));

                                    string friend;
                                    if (m1.friends == null) { friend = "0"; }
                                    else { 
                                        friend = m1.friends[0];
                                        for (int j = 1; j < m1.friends.Length; j++)
                                        {
                                            friend += ("," + m1.friends[j]);
                                        }
                                    }
                                    command.Parameters.Add(new SqlParameter("friends", friend));


                                    string Elite;
                                    if (m1.elite == null) { Elite = "0"; }
                                    else { 
                                            Elite = m1.elite[0];
                                            for (int j = 1; j < m1.elite.Length; j++)
                                            {
                                                Elite += ("," + m1.elite[j]);
                                            }
                                            command.Parameters.Add(new SqlParameter("Elite", Elite));
                                        }

                                    command.ExecuteNonQuery();
                                }
                            }
                            catch (Exception e)
                            {
                                using (SqlCommand command1 = new SqlCommand(
                                   "INSERT INTO dbo.Errors VALUES(@Date_insert,@determination, @Text_error)", _connection))
                                {
                                    command1.Parameters.Add(new SqlParameter("Date_insert", DateTime.Now));
                                    command1.Parameters.Add(new SqlParameter("determination", "user_base"));
                                    command1.Parameters.Add(new SqlParameter("Text_error", e.Message));

                                    command1.ExecuteNonQuery();
                                }
                            }
                            breack_proc++;
                        }
                    }
                    break;
                case 5:
                    using (var fileStream = File.OpenRead(Path))
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                    {
                        String line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            //if (breack_proc == 100) break;
                            checkin_base m1 = JsonConvert.DeserializeObject<checkin_base>(line);
                            try
                            {
                                using (SqlCommand command = new SqlCommand("dbo.Insert_CheckIn", _connection))
                                {
                                    command.CommandType = CommandType.StoredProcedure;

                                    string m_time = m1.time[0];
                                    for (int j = 1; j < m1.time.Length; j++)
                                    {
                                        m_time += ("," + m1.time[j]);
                                    }
                                    command.Parameters.Add(new SqlParameter("time", m_time));
                                    command.Parameters.Add(new SqlParameter("business_id", m1.business_id));

                                    command.ExecuteNonQuery();
                                }
                            }
                            catch (Exception e)
                            {
                                using (SqlCommand command1 = new SqlCommand(
                                  "INSERT INTO dbo.Errors VALUES(@Date_insert,@determination, @Text_error)", _connection))
                                {
                                    command1.Parameters.Add(new SqlParameter("Date_insert", DateTime.Now));
                                    command1.Parameters.Add(new SqlParameter("determination", "checkin_base"));
                                    command1.Parameters.Add(new SqlParameter("Text_error", e.Message));

                                    command1.ExecuteNonQuery();
                                }
                            }
                            breack_proc++;
                        }
                    }
                    break;

                case 6:
                    try
                    {
                        using (SqlCommand command = new SqlCommand("dbo.Update_User_ID", _connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.ExecuteNonQuery();
                        }

                        using (SqlCommand command = new SqlCommand("dbo.Create_Constraints", _connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.ExecuteNonQuery();
                        }
                    }
                    catch(Exception e)
                    {
                        using (SqlCommand command1 = new SqlCommand(
                                 "INSERT INTO dbo.Errors VALUES(@Date_insert,@determination, @Text_error)", _connection))
                        {
                            command1.Parameters.Add(new SqlParameter("Date_insert", DateTime.Now));
                            command1.Parameters.Add(new SqlParameter("determination", "Update_User_ID"));
                            command1.Parameters.Add(new SqlParameter("Text_error", e.Message));

                            command1.ExecuteNonQuery();
                        }
                    }
                    break;

                default: break;
            }
                   
            
        }


        private bool EnterData(Review_base z)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(
                    "INSERT INTO dbo.Review_base VALUES(@review_id, @User_id, @business_id, @stars, @date, @text, @useful, @funny, @cool, @type, @business_id_int,@user_id_int)", _connection))
                {
                    Int64 r = 0;
                    command.Parameters.Add(new SqlParameter("review_id", z.review_id));
                    command.Parameters.Add(new SqlParameter("User_id", z.user_id));
                    command.Parameters.Add(new SqlParameter("business_id", z.business_id));
                    command.Parameters.Add(new SqlParameter("stars", z.stars));
                    command.Parameters.Add(new SqlParameter("date", z.date));
                    command.Parameters.Add(new SqlParameter("text", z.text));
                    command.Parameters.Add(new SqlParameter("useful", z.useful));
                    command.Parameters.Add(new SqlParameter("funny", z.funny));
                    command.Parameters.Add(new SqlParameter("cool", z.cool));
                    command.Parameters.Add(new SqlParameter("type", z.type));
                    command.Parameters.Add(new SqlParameter("business_id_int", r));
                    command.Parameters.Add(new SqlParameter("user_id_int", r));
                    command.ExecuteNonQuery();
                }

                return true;
            }
            catch
            {
                return false;
            }

        }


    }


    [JsonObject(MemberSerialization.OptIn)]
    internal class Review_base
    {
        [JsonProperty("review_id")]
        public string review_id { get; set; }

        [JsonProperty("user_id")]
        public string user_id { get; set; }

        [JsonProperty("business_id")]
        public string business_id { get; set; }

        [JsonProperty("stars")]
        public int stars { get; set; }

        [JsonProperty("date")]
        public DateTime date { get; set; }

        [JsonProperty("text")]
        public string text { get; set; }

        [JsonProperty("useful")]
        public int useful { get; set; }

        [JsonProperty("funny")]
        public int funny { get; set; }

        [JsonProperty("cool")]
        public int cool { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

    }


    [JsonObject(MemberSerialization.OptIn)]
    internal class tip_base
    {
        [JsonProperty("text")]
        public string text { get; set; }

        [JsonProperty("date")]
        public DateTime date { get; set; }

        [JsonProperty("likes")]
        public int likes { get; set; }

        [JsonProperty("business_id")]
        public string business_id { get; set; }

        [JsonProperty("user_id")]
        public string user_id { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }
    }


    [JsonObject(MemberSerialization.OptIn)]
    internal class business_base
    {
        [JsonProperty("business_id")]
        public string business_id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("neighborhood")]
        public string neighborhood { get; set; }

        [JsonProperty("address")]
        public string address { get; set; }

        [JsonProperty("city")]
        public string city { get; set; }

        [JsonProperty("state")]
        public string state { get; set; }

        [JsonProperty("postal code")]
        public string postal_code { get; set; }

        [JsonProperty("latitude")]
        public string latitude { get; set; }

        [JsonProperty("longitude")]
        public string longitude { get; set; }

        [JsonProperty("stars")]
        public float stars { get; set; }

        [JsonProperty("review_count")]
        public int review_count { get; set; }

        [JsonProperty("is_open")]
        public byte is_open { get; set; }

        [JsonProperty("attributes")]
        public string[] attributes { get; set; }

        [JsonProperty("categories")]
        public string[] categories { get; set; }

        [JsonProperty("hours")]
        public string[] hours { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }
    }



    [JsonObject(MemberSerialization.OptIn)]
    internal class checkin_base
    {

        [JsonProperty("time")]
        public string[] time { get; set; }

        [JsonProperty("business_id")]
        public string business_id { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    internal class user_base
    {
        [JsonProperty("user_id")]
        public string user_id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("review_count")]
        public int review_count { get; set; }

        [JsonProperty("yelping_since")]
        public DateTime yelping_since { get; set; }

        [JsonProperty("friends")]
        public string[] friends { get; set; }

        [JsonProperty("useful")]
        public int useful { get; set; }

        [JsonProperty("funny")]
        public int funny { get; set; }

        [JsonProperty("cool")]
        public int cool { get; set; }

        [JsonProperty("fans")]
        public int fans { get; set; }

        [JsonProperty("elite")]
        public string[] elite { get; set; }

        [JsonProperty("average_stars")]
        public float average_stars { get; set; }

        [JsonProperty("compliment_hot")]
        public int compliment_hot { get; set; }

        [JsonProperty("compliment_more")]
        public int compliment_more { get; set; }

        [JsonProperty("compliment_profile")]
        public int compliment_profile { get; set; }

        [JsonProperty("compliment_cute")]
        public int compliment_cute { get; set; }

        [JsonProperty("compliment_list")]
        public int compliment_list { get; set; }

        [JsonProperty("compliment_note")]
        public int compliment_note { get; set; }

        [JsonProperty("compliment_plain")]
        public int compliment_plain { get; set; }

        [JsonProperty("compliment_cool")]
        public int compliment_cool { get; set; }

        [JsonProperty("compliment_funny")]
        public int compliment_funny { get; set; }

        [JsonProperty("compliment_writer")]
        public int compliment_writer { get; set; }

        [JsonProperty("compliment_photos")]
        public int compliment_photos { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }
    }

}
