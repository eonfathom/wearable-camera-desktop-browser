﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SenseCamBrowser1.Database_Versioning
{
    class text_for_stored_procedures
    {

        #region get user details

        public static string spGet_List_Of_Users()
        {
            string end_string = "";
            end_string += "\n" + "select [user_id], username, [password], [name]";
            end_string += "\n" + "from Users";
            end_string += "\n" + "order by [name];";
            return end_string;
        } //close method spGet_List_Of_Users()...


        public static string spGet_UserID_of_Most_Recent_Data_Upload()
        {
            string end_string = "";
            end_string += "\n" + "select [user_id]";
            end_string += "\n" + "from All_Events";
            end_string += "\n" + "group by [user_id]";
            end_string += "\n" + "order by max([day]) desc";
            end_string += "\n" + "LIMIT 1;";
            return end_string;
        } //close method spGet_UserID_of_Most_Recent_Data_Upload()...

        public static string spInsert_New_User_Into_Database_and_Return_ID(string new_user_name)
        {
            //todo multiquery
            string end_string = "";
            end_string += "\n" + "insert into Users (username,password,name) values ('" + new_user_name + "','" + new_user_name + "','" + new_user_name + "');";
            end_string += "\n" + "";
            end_string += "\n" + "select max([user_id]) from Users;";
            return end_string;
        } //close method spInsert_New_User_Into_Database_and_Return_ID()...


        public static string spCreate_dummy_image(int user_id, int event_id)
        {
            //todo multiquery
            string end_string = "";
            end_string += "\n" + "insert into All_Images (user_id,event_id,image_path,image_time) values (" + user_id + "," + event_id + ",'','1991-01-23 08:02:00');";
            return end_string;
        } //close method spCreate_dummy_image()...
        
        #endregion get user details



        #region update event details

        public static string spCreate_new_event_and_return_its_ID(int user_id)
        {
            DateTime default_day = new DateTime(1999, 1, 1);
            return spCreate_new_event_and_return_its_ID(user_id, default_day);
        } //close method spCreate_new_event_and_return_its_ID()...


        public static string spCreate_new_event_and_return_its_ID(int user_id, DateTime day_of_source_event)
        {
            string end_string = "INSERT INTO All_Events(user_id,day,utc_day,start_time,end_time,keyframe_path,number_times_viewed) VALUES (" + user_id + ", " + convert_datetime_to_sql_string(day_of_source_event) + ", " + convert_datetime_to_sql_string(day_of_source_event) + ", " + convert_datetime_to_sql_string(day_of_source_event) + ", " + convert_datetime_to_sql_string(day_of_source_event) + ", '', 0);";
            end_string += "\n" + "SELECT MAX(Event_ID) FROM All_Events WHERE [user_id]=" + user_id + ";";
            return end_string;
        } //close method spCreate_new_event_and_return_its_ID()...


        public static string spDelete_Event(int user_id, int event_id)
        {
            string end_string = "DELETE";
            end_string += "\n" + "FROM All_Events";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND [event_id] = " + event_id;
            return end_string;
        } //close method spDelete_Event()...


        public static string spUpdateEventComment(int user_id, int event_id, string comment)
        {
            string end_string = "UPDATE All_Events";
            end_string += "\n" + "SET comment = '" + comment + "'";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND [event_id] = " + event_id;
            return end_string;
        } //close method spUpdateEventComment()...
        

        public static string spUpdate_Event_Keyframe_Path(int user_id, int event_id, string keyframe_path)
        {
            string end_string = "UPDATE All_Events";
            end_string += "\n" + "SET keyframe_path = '" + keyframe_path + "'";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND [event_id] = " + event_id;
            return end_string;
        } //end method spUpdate_Event_Keyframe_Path()...


        public static string spUpdate_Event_time_keyframe_info(int user_id, int event_id, DateTime start_time, DateTime end_time, string new_keyframe_path)
        {
            string end_string = "UPDATE All_Events";
            end_string += "\n" + "SET start_time = " + convert_datetime_to_sql_string(start_time) + ",";
            end_string += "\n" + "end_time = " + convert_datetime_to_sql_string(end_time) + ",";
            end_string += "\n" + "keyframe_path = '" + new_keyframe_path + "'";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND event_id = " + event_id + ";";
            return end_string;
        } //close method spUpdate_Event_time_keyframe_info()...
        

        public static string spUpdateEvent_Number_Times_Viewed(int user_id, int event_id)
        {
            string end_string = "";
            end_string += "\n" + "UPDATE All_Events";
            end_string += "\n" + "SET number_times_viewed = number_times_viewed + 1";
            end_string += "\n" + "WHERE [user_id]= " + user_id;
            end_string += "\n" + "and event_id = " + event_id + ";";
            return end_string;
        } //close method spUpdateEvent_Number_Times_Viewed()...
        
        #endregion update event details




        #region get event details

        public static string spGet_most_recent_event_id_for_user(int user_id)
        {
            string end_string = "";
            end_string += "\n" + "SELECT CASE WHEN MAX(event_id) IS NOT NULL THEN MAX(event_id)-1 ELSE -1 END "; //WILL JUST GO BACK 1 JUST TO BE ON THE SAFE SIDE SO THAT I DON'T MISS ASSIGNING THE EVENT TO ANY IMAGE
            end_string += "\n" + "FROM All_Images where [user_id] = " + user_id;

            return end_string;
        } //close method spGet_most_recent_event_id_for_user()....


        public static string spGet_All_Events_In_Day(int user_id, DateTime day)
        {
            string end_string = "SELECT event_id, start_time, end_time, keyframe_path, comment";
            end_string += "\n" + "FROM All_Events";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND day >=" + convert_datetime_to_sql_string(new DateTime(day.Year, day.Month, day.Day)) + "";
            end_string += "\n" + "AND day <=" + convert_datetime_to_sql_string(new DateTime(day.Year, day.Month, day.Day, 23, 59, 59)) + "";
            //end_string += "\n" + "AND DATEPART(YEAR, [day]) = DATEPART(YEAR, " + convert_datetime_to_sql_string(day) + ")";
            //end_string += "\n" + "AND DATEPART(DAYOFYEAR, [day]) = DATEPART(DAYOFYEAR, " + convert_datetime_to_sql_string(day) + ")";
            end_string += "\n" + "ORDER BY start_time";

            return end_string;
        } //close method spGet_All_Events_In_Day()...


        public static string spGet_Last_Keyframe_Path(int user_id)
        {
            string end_string = "SELECT keyframe_path";
            end_string += "\n" + "FROM All_Events";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "ORDER BY [day] DESC";
            end_string += "\n" + "LIMIT 1";

            return end_string;
        } //close method spGet_Last_Keyframe_Path()


        public static string spGet_day_of_source_event(int user_id, int event_id)
        {
            string end_string = "SELECT [day] FROM All_Events WHERE [user_id]=" + user_id + " AND event_id=" + event_id + ";";

            return end_string;
        } //close method spGet_day_of_source_event()...


        public static string spGet_start_end_time_of_event(int user_id, int event_id)
        {
            string end_string = "SELECT MIN(image_time) as start_time, MAX(image_time) as end_time";
            end_string += "\n" + "FROM All_Images";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND event_id = " + event_id + ";";

            return end_string;
        } //close method spGet_start_end_time_of_event()...


        public static string spGet_Num_Images_In_Event(int user_id, int event_id)
        {
            string end_string = "SELECT COUNT(image_id)";
            end_string += "\n" + "FROM All_Images";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND [event_id] = " + event_id;

            return end_string;
        } //close method spGet_Num_Images_In_Event()...


        public static string spGet_Paths_Of_All_Images_In_Event(int user_id, int event_id)
        {
            string end_string = "SELECT image_path, image_time";
            end_string += "\n" + "FROM All_Images";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND [event_id] = " + event_id;
            end_string += "\n" + "ORDER BY image_time";

            return end_string;
        } //close method spGet_Paths_Of_All_Images_In_Events()...
        

        public static string spSelect_random_image_from_event_around_target_window(int user_id, int event_id, DateTime target_time, int search_window_minutes)
        {
            string end_string = "SELECT image_path";
            end_string += "\n" + "FROM All_Images";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND event_id = " + event_id;
            end_string += "\n" + "AND image_time >= " + convert_datetime_to_sql_string(target_time.AddMinutes(-search_window_minutes));
            end_string += "\n" + "AND image_time <= " + convert_datetime_to_sql_string(target_time.AddMinutes(search_window_minutes));
            end_string += "\n" + "ORDER BY RANDOM()";
            end_string += "\n" + "LIMIT 1;";

            return end_string;
        } //close method spSelect_random_image_from_event_around_target_window()...


        public static string spSelect_any_random_image_from_event(int user_id, int event_id)
        {
            string end_string = "SELECT image_path";
            end_string += "\n" + "FROM All_Images";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND event_id = " + event_id;
            end_string += "\n" + "ORDER BY RANDOM()";
            end_string += "\n" + "LIMIT 1;";

            return end_string;
        } //close method spSelect_any_random_image_from_event()...


        public static string spGet_id_of_event_before_ID_and_time(int user_id, int source_event_id, DateTime target_end_time, DateTime source_day)
        {
            string end_string = "SELECT event_id";
            end_string += "\n" + "FROM All_Events";
            end_string += "\n" + "WHERE [user_id]=" + user_id;
            end_string += "\n" + "AND event_id!=" + source_event_id;
            end_string += "\n" + "AND start_time >= " + convert_datetime_to_sql_string(target_end_time.AddHours(-6));
            end_string += "\n" + "AND start_time < " + convert_datetime_to_sql_string(target_end_time);
            end_string += "\n" + "AND [day] = " + convert_datetime_to_sql_string(source_day);
            end_string += "\n" + "ORDER BY start_time DESC";
            end_string += "\n" + "LIMIT 1 ;";
            return end_string;
        } //close method spGet_id_of_event_before_ID_and_time()...


        public static string spGet_id_of_event_after_ID_and_time(int user_id, int source_event_id, DateTime target_start_time, DateTime source_day)
        {
            string end_string = "SELECT event_id";
            end_string += "\n" + "FROM All_Events";
            end_string += "\n" + "WHERE [user_id]=" + user_id;
            end_string += "\n" + "AND event_id!=" + source_event_id;
            end_string += "\n" + "AND start_time > " + convert_datetime_to_sql_string(target_start_time);
            end_string += "\n" + "AND start_time <= " + convert_datetime_to_sql_string(target_start_time.AddHours(6));
            end_string += "\n" + "AND [day] = " + convert_datetime_to_sql_string(source_day);
            end_string += "\n" + "ORDER BY start_time DESC";
            end_string += "\n" + "LIMIT 1 ;";

            return end_string;
        } //close method spGet_id_of_event_after_ID_and_time()...


        #endregion get event details




        #region update event id of all images (usually for newly uploaded data)



        public static string spUpdate_Images_With_Event_ID_step2_update_images_with_relevant_event_id(int user_id, int most_recent_event_id)
        {
            string end_string = "";
            end_string += "\n" + "UPDATE All_Images";
            end_string += "\n" + "SET event_id=";
            end_string += "\n" + "(";
            end_string += "\n" + "SELECT event_id"; //todo may have to change this to max(event_id) and remove limit 1 a few lines below?
            end_string += "\n" + "FROM All_Events";
            end_string += "\n" + "WHERE event_id > " + most_recent_event_id;
            end_string += "\n" + "AND [user_id] = " + user_id;
            end_string += "\n" + "AND start_time <= All_Images.image_time AND end_time >= All_Images.image_time";
            end_string += "\n" + "LIMIT 1";
            end_string += "\n" + ")";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND event_id IS NULL;";

            return end_string;
        } //close method spUpdate_Images_With_Event_ID_step2_update_images_with_relevant_event_id()....


        public static string spUpdate_Images_With_Event_ID_step3_update_sensor_readings_with_relevant_event_id(int user_id, int most_recent_event_id)
        {
            string end_string = "";
            end_string += "\n" + "UPDATE Sensor_Readings";
            end_string += "\n" + "SET event_id=";
            end_string += "\n" + "(";
            end_string += "\n" + "SELECT MAX(event_id)";
            end_string += "\n" + "FROM All_Events";
            end_string += "\n" + "WHERE event_id > " + most_recent_event_id;
            end_string += "\n" + "AND [user_id] = " + user_id;
            //end_string += "\n" + "AND DATEADD(MINUTE,-1,start_time) <= Sensor_Readings.sample_time AND DATEADD(MINUTE,1,end_time) >= Sensor_Readings.sample_time";
            end_string += "\n" + "AND DATETIME(start_time,'-1 minute') <= Sensor_Readings.sample_time AND DATETIME(end_time,'+1 minute') >= Sensor_Readings.sample_time";
            end_string += "\n" + ")";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND event_id IS NULL;";

            return end_string;
        } //close method spUpdate_Images_With_Event_ID_step2_update_images_with_relevant_event_id()....


        public static string spUpdate_Images_With_Event_ID_step4_tidy_up_stage(int user_id)
        {
            string end_string = "";
            end_string += "\n" + "DELETE FROM All_Events";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "and start_time>end_time;";
            end_string += "\n" + "";
            end_string += "\n" + "";
            end_string += "\n" + "UPDATE All_Images";
            end_string += "\n" + "SET event_id = (SELECT MAX(event_id) FROM All_Events WHERE [user_id] = " + user_id + ")";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND event_id IS NULL;";

            return end_string;
        } //close method spUpdate_Images_With_Event_ID_step2_update_images_with_relevant_event_id()....

        #endregion update event id of all images (usually for newly uploaded data)




        #region get day details

        public static string spGet_List_Of_All_Days_For_User(int user_id)
        {
            string end_string = "SELECT MIN([day])";
            end_string += "\n" + "FROM All_Events";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "GROUP BY strftime('%Y-%m-%d',day)";
            end_string += "\n" + "ORDER BY MIN([day]) DESC;";
            return end_string;
        } //close method spGet_List_Of_All_Days_For_User()...


        public static string spGet_Day_Start_and_End_Times(int user_id, DateTime day)
        {
            string end_string = "SELECT MIN(start_time) AS start_time, MAX(end_time) AS end_time";
            end_string += "\n" + "FROM All_Events";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND day >=" + convert_datetime_to_sql_string(new DateTime(day.Year, day.Month, day.Day));
            end_string += "\n" + "AND day <=" + convert_datetime_to_sql_string(new DateTime(day.Year, day.Month, day.Day, 23, 59, 59)) + ";";
            //end_string += "\n" + "AND DATEPART(YEAR, [day]) = DATEPART(YEAR, " + convert_datetime_to_sql_string(day) + ")";
            //end_string += "\n" + "AND DATEPART(DAYOFYEAR, [day]) = DATEPART(DAYOFYEAR, " + convert_datetime_to_sql_string(day) + ")";
            return end_string;
        } //close method spGet_Day_Start_and_End_Times()...


        public static string spGet_Num_Images_In_Day(int user_id, DateTime day)
        {
            string end_string = "SELECT COUNT(*)";
            end_string += "\n" + "";
            end_string += "\n" + "FROM All_Images";
            end_string += "\n" + "";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND event_id in";
            end_string += "\n" + "(";
            end_string += "\n" + "SELECT event_id";
            end_string += "\n" + "FROM All_Events";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND day >=" + convert_datetime_to_sql_string(new DateTime(day.Year, day.Month, day.Day));
            end_string += "\n" + "AND day <=" + convert_datetime_to_sql_string(new DateTime(day.Year, day.Month, day.Day, 23, 59, 59)) + "";
            //end_string += "\n" + "AND DATEPART(YEAR, [day]) = DATEPART(YEAR, " + convert_datetime_to_sql_string(day) + ")";
            //end_string += "\n" + "AND DATEPART(DAYOFYEAR, [day]) = DATEPART(DAYOFYEAR, " + convert_datetime_to_sql_string(day) + ")";
            end_string += "\n" + ");";
            return end_string;
        } //close method spGet_Num_Images_In_Day()...

        #endregion get user and day details





        public static string Jan11_SPLIT_EVENT_INTO_TWO_part3_update_image_sensors_tables_with_new_event_id(int user_id, int event_id_to_append_images_to, int event_id_of_source_images, DateTime time_of_start_image)
        {
            string end_string = "UPDATE All_Images";
            end_string += "\n" + "SET event_id = " + event_id_to_append_images_to;
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND event_id = " + event_id_of_source_images;
            end_string += "\n" + "AND image_time >= " + convert_datetime_to_sql_string(time_of_start_image) + ";";
            end_string += "\n" + "";
            end_string += "\n" + "UPDATE Sensor_Readings";
            end_string += "\n" + "SET event_id = " + event_id_to_append_images_to;
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND event_id = " + event_id_of_source_images;
            end_string += "\n" + "AND sample_time >= " + convert_datetime_to_sql_string(time_of_start_image) + ";";
            end_string += "\n" + "";
            return end_string;
        } //close method Jan11_SPLIT_EVENT_INTO_TWO()...





        public static string Oct10_ADD_NEW_MERGED_IMAGES_TO_PREVIOUS_EVENT_part4_update_images_sensors_tables_with_new_event_id(int user_id, int event_id_to_append_images_to, int event_id_of_new_source_images, DateTime time_of_end_image)
        {
            string end_string = "UPDATE All_Images";
            end_string += "\n" + "SET event_id = " + event_id_to_append_images_to;
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND event_id = " + event_id_of_new_source_images;
            end_string += "\n" + "AND image_time <= " + convert_datetime_to_sql_string(time_of_end_image) + ";";
            end_string += "\n" + "";
            end_string += "\n" + "UPDATE Sensor_Readings";
            end_string += "\n" + "SET event_id = " + event_id_to_append_images_to;
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND event_id = " + event_id_of_new_source_images;
            end_string += "\n" + "AND sample_time <= " + convert_datetime_to_sql_string(time_of_end_image) + ";";
            return end_string;
        } //close method Oct10_ADD_NEW_MERGED_IMAGES_TO_PREVIOUS_EVENT()...





        public static string spLog_User_Interaction(int user_id, DateTime interaction_time, string uixaml_element, string comma_seperated_parameters)
        {
            string end_string = "INSERT INTO User_Interaction_Log";
            end_string += "\n" + "VALUES(" + user_id + ", " + convert_datetime_to_sql_string(interaction_time) + ", '" + uixaml_element + "', '" + comma_seperated_parameters + "');";
            return end_string;
        } //close method spLog_User_Interaction()...




        public static string spDelete_Image_From_Event(int user_id, int event_id, DateTime image_time)
        {
            string end_string = "DELETE";
            end_string += "\n" + "FROM All_Images";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND [event_id] = " + event_id;
            end_string += "\n" + "AND image_time = " + convert_datetime_to_sql_string(image_time) + ";";
            return end_string;
        } //close method spDelete_Image_From_Event()...





        public static string spGet_annotated_events_in_day(int user_id, DateTime day)
        {
            //string end_string = "SELECT annotations.event_id, annotations.annotation_name,DATEDIFF(SECOND, All_Events.start_time, All_Events.end_time) AS duration_in_seconds";
            string end_string = "SELECT annotations.event_id, annotations.annotation_name, strftime('%s',All_Events.end_time) - strftime('%s',All_Events.start_time) AS duration_in_seconds";
            end_string += "\n" + "FROM SC_Browser_User_Annotations AS annotations";
            end_string += "\n" + "";
            end_string += "\n" + "INNER JOIN All_Events";
            end_string += "\n" + "ON annotations.event_id = All_Events.[event_id]";
            end_string += "\n" + "";
            end_string += "\n" + "WHERE annotations.[user_id]=" + user_id;
            end_string += "\n" + "AND day >=" + convert_datetime_to_sql_string(new DateTime(day.Year, day.Month, day.Day)) + "";
            end_string += "\n" + "AND day <=" + convert_datetime_to_sql_string(new DateTime(day.Year, day.Month, day.Day, 23, 59, 59)) + "";
            //end_string += "\n" + "AND DATEPART(YEAR, All_Events.[day]) = DATEPART(YEAR, " + convert_datetime_to_sql_string(day) + ")";
            //end_string += "\n" + "AND DATEPART(DAYOFYEAR, All_Events.[day]) = DATEPART(DAYOFYEAR, " + convert_datetime_to_sql_string(day) + ")";
            end_string += "\n" + "";
            end_string += "\n" + "ORDER BY All_Events.start_time";

            return end_string;
        } //close method JAN11_GET_ANNOTATED_EVENTS_IN_DAY()...




        private static string convert_datetime_to_sql_string(DateTime time)
        {
            string month, day, hour, minute, second;
            if (time.Month < 10)
                month = "0" + time.Month;
            else month = time.Month.ToString();

            if (time.Day < 10)
                day = "0" + time.Day;
            else day = time.Day.ToString();
            
            if (time.Hour < 10)
                hour = "0" + time.Hour;
            else hour = time.Hour.ToString();

            if (time.Minute < 10)
                minute = "0" + time.Minute;
            else minute = time.Minute.ToString();

            if (time.Second < 10)
                second = "0" + time.Second;
            else second = time.Second.ToString();

            return "'" + time.Year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second + "'";// +"." + time.Millisecond + "'";
        } //close method convert_datetime_to_sql_string()...




        public static string AUG12_CLEAR_EVENT_ANNOTATIONS_INDIVIDUAL(int user_id, int event_id, string individual_annotation_text)
        {
            string end_string = "";
            end_string += "\n" + "DELETE FROM SC_Browser_User_Annotations";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND [event_id] = " + event_id;
            end_string += "\n" + "AND [annotation_name] = '" + individual_annotation_text + "';";
            end_string += "\n" + "";
            end_string += "\n" + "";

            return end_string;
        } //close method AUG12_CLEAR_EVENT_ANNOTATIONS_INDIVIDUAL()...




        public static string APR11_REMOVE_ANNOTATION_TYPE(string annotation_type_name)
        {
            string end_string = "";
            end_string += "\n" + "DELETE FROM Annotation_Types";
            end_string += "\n" + "WHERE annotation_type = '" + annotation_type_name + "'";
            end_string += "\n" + "";

            return end_string;
        } //close method APR11_REMOVE_ANNOTATION_TYPE()...




        public static string APR11_REMOVE_ALL_ANNOTATION_TYPES()
        {
            string end_string = "";
            end_string += "\n" + "DELETE FROM Annotation_Types;";

            return end_string;
        } //close APR11_REMOVE_ALL_ANNOTATION_TYPES()...




        public static string NOV10_GET_LIST_OF_ANNOTATION_CLASSES()
        {
            string end_string = "";
            end_string += "\n" + "SELECT annotation_id, annotation_type, [description]";
            end_string += "\n" + "FROM Annotation_Types";
            end_string += "\n" + "order by annotation_type;";

            return end_string;
        } //close method NOV10_GET_LIST_OF_ANNOTATION_CLASSES()...




        public static string NOV10_GET_EVENTS_IDS_IN_DAY_FOR_GIVEN_ACTIVITY(int user_id, DateTime day, string annotation_type)
        {
            string end_string = "";
            end_string += "\n" + "SELECT annotations.event_id";
            end_string += "\n" + "FROM SC_Browser_User_Annotations AS annotations";
            end_string += "\n" + "INNER JOIN All_Events";
            end_string += "\n" + "ON annotations.event_id = All_Events.event_id";
            end_string += "\n" + "";
            end_string += "\n" + "WHERE annotations.[user_id]=" + user_id;
            end_string += "\n" + "  AND day >=" + convert_datetime_to_sql_string(new DateTime(day.Year, day.Month, day.Day)) + "";
            end_string += "\n" + "  AND day <=" + convert_datetime_to_sql_string(new DateTime(day.Year, day.Month, day.Day, 23, 59, 59)) + "";
            //end_string += "\n" + "AND DATEPART(YEAR, All_Events.[day]) = DATEPART(YEAR," + convert_datetime_to_sql_string(day) + ")";
            //end_string += "\n" + "AND DATEPART(DAYOFYEAR, All_Events.[day]) = DATEPART(DAYOFYEAR," + convert_datetime_to_sql_string(day) + ")";
            end_string += "\n" + "AND annotations.annotation_name='" + annotation_type + "'";
            end_string += "\n" + "";
            end_string += "\n" + "ORDER BY annotations.event_id;";
            end_string += "\n" + "";
            end_string += "\n" + "";

            return end_string;
        } //close method NOV10_GET_EVENTS_IDS_IN_DAY_FOR_GIVEN_ACTIVITY()...




        public static string NOV10_GET_DAILY_ACTIVITY_SUMMARY_FROM_ANNOTATIONS(int user_id, DateTime day)
        {
            string end_string = "";
            end_string += "\n" + "SELECT annotation_name, sum(duration_in_seconds) as total_time_spent_at_activity";
            end_string += "\n" + "";
            end_string += "\n" + "FROM (";
            end_string += "\n" + "  SELECT annotations.annotation_name, ";
            //end_string += "\n" + "  DATEDIFF(SECOND, All_Events.start_time, All_Events.end_time) AS duration_in_seconds";
            end_string += "\n" + "  strftime('%s', All_Events.end_time) - strftime('%s', All_Events.start_time) AS duration_in_seconds";
            end_string += "\n" + "";
            end_string += "\n" + "  FROM SC_Browser_User_Annotations AS annotations";
            end_string += "\n" + "      INNER JOIN All_Events";
            end_string += "\n" + "      ON annotations.event_id = All_Events.[event_id]";
            end_string += "\n" + "";
            end_string += "\n" + "  WHERE annotations.[user_id]=" + user_id;
            end_string += "\n" + "      AND day >=" + convert_datetime_to_sql_string(new DateTime(day.Year, day.Month, day.Day)) + "";
            end_string += "\n" + "      AND day <=" + convert_datetime_to_sql_string(new DateTime(day.Year, day.Month, day.Day, 23, 59, 59)) + "";
            //end_string += "\n" + "  AND DATEPART(YEAR, All_Events.[day]) = DATEPART(YEAR," + convert_datetime_to_sql_string(day) + ")";
            //end_string += "\n" + "  AND DATEPART(DAYOFYEAR, All_Events.[day]) = DATEPART(DAYOFYEAR," + convert_datetime_to_sql_string(day) + ")";
            end_string += "\n" + "";
            end_string += "\n" + "  ) AS inner_table";
            end_string += "\n" + "";
            end_string += "\n" + "GROUP BY annotation_name";
            end_string += "\n" + "ORDER BY annotation_name;";
            end_string += "\n" + "";
            end_string += "\n" + "";

            return end_string;
        } //close method NOV10_GET_DAILY_ACTIVITY_SUMMARY_FROM_ANNOTATIONS()...




        public static string NOV10_GET_ANNOTATIONS_FOR_EVENT(int user_id, int event_id)
        {
            string end_string = "";
            end_string += "\n" + "SELECT annotation_name";
            end_string += "\n" + "FROM SC_Browser_User_Annotations AS annotations";
            end_string += "\n" + "WHERE annotations.[user_id]=" + user_id;
            end_string += "\n" + "AND annotations.event_id=" + event_id;

            return end_string;
        } //close method NOV10_GET_ANNOTATIONS_FOR_EVENT()...




        public static string NOV10_CLEAR_EVENT_ANNOTATIONS(int user_id, int event_id)
        {
            string end_string = "";
            end_string += "\n" + "DELETE FROM SC_Browser_User_Annotations";
            end_string += "\n" + "WHERE [user_id] = " + user_id;
            end_string += "\n" + "AND [event_id] = " + event_id;

            return end_string;
        } //close method NOV10_CLEAR_EVENT_ANNOTATIONS()...




        public static string NOV10_ADD_EVENT_ANNOTATION(int user_id, int event_id, string event_annotation_name)
        {
            string end_string = "";
            end_string += "\n" + "INSERT INTO SC_Browser_User_Annotations";
            end_string += "\n" + "VALUES (" + user_id + "," + event_id + "," + convert_datetime_to_sql_string(DateTime.Now) + ",'" + event_annotation_name + "')";

            return end_string;
        } //close method NOV10_ADD_EVENT_ANNOTATION()...


        
        



        public static string APR11_ADD_ANNOTATION_TYPE(string annotation_type_name)
        {
            //todo multiple query
            string end_string = "";
            end_string += "\n" + "DELETE FROM Annotation_Types";
            end_string += "\n" + "WHERE annotation_type = '" + annotation_type_name + "';";
            end_string += "\n" + "INSERT INTO Annotation_Types (annotation_type,description) VALUES('" + annotation_type_name + "','" + annotation_type_name + "');";

            return end_string;
        } //close method APR11_ADD_ANNOTATION_TYPE()...



    } //close class text_for_stored_procedures...
} //close namespace...