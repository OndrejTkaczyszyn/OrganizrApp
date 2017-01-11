using Android.App;
using Android.Widget;
using Android.OS;
using SQLite_database.Resources.DBHelper;
using System.Collections.Generic;
using SQLite_database.Model;
using Android.Util;
using System;
using Android.Content;
using Android;
using Android.Views;

namespace SQLite_database
{
    [Activity(Label = "SQLite_database", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity,DateFragment.OnDatePass,NameFragment.OnNamePass,TextFragment.OnTextPass
    {

        ListView listData;
        List<Task> listSource = new List<Task>();
        DataBase db;
        Button _buttonAdd;
        Task _tempTask;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource

            SetContentView (Resource.Layout.Main);

            //Create database
            db = new DataBase();
            db.createDataBase();

            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            Log.Info("DB_PATH", folder);

            listData = FindViewById<ListView>(Resource.Id.tasksList);

            var editName = FindViewById<EditText>(Resource.Id.editName);
            //var editAge = FindViewById<EditText>(Resource.Id.editAge);
           // var editEmail = FindViewById<EditText>(Resource.Id.editEmail);

            var btnAdd = FindViewById<Button>(Resource.Id.buttonAdd);
           // var btnEdit = FindViewById<Button>(Resource.Id.buttonEdit);
           // var btnDelete = FindViewById<Button>(Resource.Id.buttonDelete);

            //LoadData
            LoadData();
            //Event
            /*btnAdd.Click += delegate
            {

                Person person = new Person()
                {
                    Name = editName.Text,
                    Age = int.Parse(editAge.Text),//TODO: handle exception here, or force input only to contain number
                    Email = editEmail.Text
                };
                db.InsertIntoTablePerson(person);
                LoadData();
            };

            btnEdit.Click += delegate {

                Person person = new Person()//TODO: handle exception here as well, apparently
                {
                    Id = int.Parse(editName.Tag.ToString()),
                    Name = editName.Text,
                    Age = int.Parse(editAge.Text),
                    Email = editEmail.Text
                };
                db.updateTablePerson(person);
                LoadData();
            };

            btnDelete.Click += delegate {

                Person person = new Person()
                {
                    Id = int.Parse(editName.Tag.ToString()),
                    Name = editName.Text,
                    Age = int.Parse(editAge.Text),
                    Email = editEmail.Text
                };
                db.deleteTablePerson(person);
                LoadData();
            };*/

            listData.ItemClick += (s, e) => {
                //Set background for selected item
                Log.Info("COUNT:", listData.Count.ToString());
                for (int i = 0; i < listData.Count; i++)
                {
                    if (e.Position == i)
                        listData.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.ParseColor("#444444FF"));
                    else
                        listData.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.ParseColor("#AAAAAAFF"));

                    Log.Info("I:", i.ToString());

                }

                //Binding Data

                var txtName = e.View.FindViewById<TextView>(Resource.Id.textName);//SS
                var txtText = e.View.FindViewById<TextView>(Resource.Id.textText);//SS
                var txtDate = e.View.FindViewById<TextView>(Resource.Id.textDate);//SS

                editName.Text = txtName.Text;
                editName.Tag = e.Id;
                /*editAge.Text = txtAge.Text;
                editEmail.Text = txtEmail.Text;*/
            };


            //fragments stuff


            _buttonAdd = FindViewById<Button>(Resource.Id.buttonAdd);

            _buttonAdd.Click += (object sender, EventArgs args) =>
            { //pull up dialog
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                NameFragment firstFragment = new NameFragment();
                
                transaction.Add(Resource.Id.RootElement, firstFragment);
                transaction.AddToBackStack("adding first fragment");
                transaction.Commit();
                firstFragment.OnCreate(bundle);
                var viewToHide = FindViewById<LinearLayout>(Resource.Id.ToHide);

                viewToHide.Visibility = ViewStates.Gone;
            };


        }

        private void LoadData()
        {
            listSource = db.selectTableTask();
            var adapter = new ListViewAdapter(this, listSource);
            listData.Adapter = adapter;
        }

        //implementing communication with fragments

        public void onNamePass(String name)
        {

                _tempTask = new Task() { Name=name,Date=null,Text=null};

            Console.WriteLine("Got that name:" + name);
           // FindViewById<TextView>(Resource.Id.nameText).Text = name;
        }

        public void onTextPass(String text)
        {

            if (_tempTask != null)
            {
                _tempTask.Text = text;
            }
            Console.WriteLine("Got that text:" + text);
            //FindViewById<TextView>(Resource.Id.textText).Text = text;


        }

        public void onDatePass(String date)
        {
            Console.WriteLine("Got that name:" + date);
            //Console.WriteLine("Got that name:" + date.ToString());
            // FindViewById<TextView>(Resource.Id.dateText).Text = date;

            //IS END OF PROCESS
            if (_tempTask != null)
            {
                _tempTask.Date = date;
                listSource.Add(_tempTask);
                
                db.InsertIntoTableTask(_tempTask);
                LoadData();
                _tempTask = null;

            }
            
        }
        

    }
}

