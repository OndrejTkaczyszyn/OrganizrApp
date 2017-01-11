using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using Android.Util;
using SQLite_database.Model;

namespace SQLite_database.Resources.DBHelper
{

    public class DataBase
    {

        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        public bool createDataBase()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Tasks.db")))
                {
                    connection.CreateTable<Task>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool InsertIntoTableTask(Task task)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Tasks.db")))
                {
                    connection.Insert(task);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public List<Task> selectTableTask()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Tasks.db")))
                {
                    return connection.Table<Task>().ToList();


                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }

        public bool updateTableTask(Task task)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Tasks.db")))
                {
                    connection.Query<Task>("UPDATE Task set Name=?,Text=?,Date=? Where Id=?",task.Name,task.Text,task.Date,task.Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool deleteTableTask(Task task)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Tasks.db")))
                {
                    connection.Delete(task);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool selectQueryTableTask(int Id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Tasks.db")))
                {
                    connection.Query<Task>("SELECT * FROM Task Where Id=?", Id);
                    return true;
                }
            }
            catch(SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

    }
}