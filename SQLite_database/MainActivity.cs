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
    [Activity(Label = "Organizr", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity,DateFragment.OnDatePass,TimeFragment.OnTimePass,NameFragment.OnNamePass,TextFragment.OnTextPass,taskedit_BaseFragment.OnNewTaskDataPass,ReminderDateDialog.OnNewDatePass,ReminderTimeDialog.OnNewTimePass//,taskedit_BaseFragment.OnChangeAddDataPass
    {

        ListView listData;
        List<Task> listSource = new List<Task>();
        DataBase db;
        Button _buttonAdd;
        Task _tempTask;
        DateTime _tempDateTime = DateTime.MinValue;
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

            listData.ItemLongClick += (s, e) => //Want to edit task
            {
                Log.Info("COUNT:", listData.Count.ToString());
                int id = (int)e.View.GetTag(Resource.Id.editLayout);
                Log.Info("DATA ID: ", id.ToString());
                Log.Info("CORRESPONDING NAME:", db.queryTableTask(id)[0].Name);

                Task entry = db.queryTaskById(id);

                //Let the transaction begin

                FragmentTransaction transaction = FragmentManager.BeginTransaction();

                Bundle taskdata = new Bundle();
                taskdata.PutString("tskname", entry.Name);
                taskdata.PutString("tsktext", entry.Text);
                taskdata.PutInt("id", entry.Id);

                taskedit_BaseFragment editFragment = new taskedit_BaseFragment();

                editFragment.Arguments = taskdata;

                transaction.Add(Resource.Id.RootElement, editFragment);
                transaction.AddToBackStack("adding first fragment");
                transaction.Commit();
                editFragment.OnCreate(bundle);
                var viewToHide = FindViewById<LinearLayout>(Resource.Id.ToHide);

                

                viewToHide.Visibility = ViewStates.Gone;
            };
            listData.ItemClick += (s, e) => {
                //Set background for selected item
                Log.Info("COUNT:", listData.Count.ToString());
                Log.Info("Position:", e.Position.ToString());
                for (int i = 0; i < listData.Count; i++)
                {
                    var elem = listData.GetChildAt(i);
                    /*var textView = elem.FindView;*/
                    if (e.Position == i)
                    {
                        elem.SetBackgroundColor(Android.Graphics.Color.ParseColor(GetString(Resource.Color.ListItemBackgroundActive)));
                        elem.FindViewById<TextView>(Resource.Id.textText).Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        elem.SetBackgroundColor(Android.Graphics.Color.ParseColor(GetString(Resource.Color.ListItemBackground)));
                        elem.FindViewById<TextView>(Resource.Id.textText).Visibility = ViewStates.Gone;
                        //TODO: Animate
                    }
                        /*elem.SetTextColor(Android.Graphics.Color.ParseColor(GetString(Resource.Color.ListItemBackground)));*/
                    Log.Info("I:", i.ToString());

                }

                //Binding Data
                /*var txtName = e.View.FindViewById<TextView>(Resource.Id.textName);//SS
                var txtText = e.View.FindViewById<TextView>(Resource.Id.textText);//SS
                var txtDate = e.View.FindViewById<TextView>(Resource.Id.textDate);//SS
                
                editName.Text = txtName.Text;
                editName.Tag = e.Id;
                editAge.Text = txtAge.Text;
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

                _tempTask = new Task() { Name=name,Date=DateTime.Today,Text=null};

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

        public void onDatePass(DateTime date)
        {
            Console.WriteLine("Got that date:" + date.ToString());


            //IS END OF PROCESS
            if (_tempTask != null)
            {
                _tempTask.Date = date;
               /* listSource.Add(_tempTask);
                _tempTask = null;*/

            }
            
        }
        public void onTimePass(int hour, int min)
        {
            Console.WriteLine("Got that time:" + hour + " minutes: " + min);
            //Console.WriteLine("Got that name:" + date.ToString());
            // FindViewById<TextView>(Resource.Id.dateText).Text = date;

            //IS END OF PROCESS
            if (_tempTask != null)
            {
                DateTime t = _tempTask.Date;
                DateTime tTempDateTime = new DateTime(t.Year,t.Month,t.Day,hour, min,0);


                _tempTask.Date = tTempDateTime;
                Console.WriteLine(_tempTask.Date);
                db.InsertIntoTableTask(_tempTask);
                LoadData();
                _tempTask = null;

            }

        }

        public void onNewTextPass(String text, int id)
        {
            Task tempTask = db.queryTaskById(id);

            tempTask.Text = text;

            db.updateTableTask(tempTask);
            LoadData();
        }

        public void onNewDateTimePass(DateTime datetime, int id)
        {
            Task tempTask = db.queryTaskById(id);

            tempTask.Date = datetime;

            db.updateTableTask(tempTask);
            LoadData();

        }

        public void onNewNamePass(String name, int id)
        {
            Task tempTask = db.queryTaskById(id);

            tempTask.Name = name;

            db.updateTableTask(tempTask);
            LoadData();
        }

        public void onNewDatePass(DateTime date, int id)
        {
            Console.WriteLine("New date passed");

            _tempDateTime = date;

            
        }

        public void onNewTimePass(DateTime time, int id)
        {
            Task tempTask = db.queryTaskById(id);

            DateTime t = _tempDateTime;
            DateTime tTempDateTime = new DateTime(t.Year, t.Month, t.Day, time.Hour, time.Minute, 0);
            tempTask.Date = tTempDateTime;
            db.updateTableTask(tempTask);
            LoadData();
        }

        public void doDeleteTask(int id)
        {
            //TODO:remove assoiated reminders
            Task tempTask = db.queryTaskById(id);
            db.deleteTableTask(tempTask);
            LoadData();
        }

        public void openTimeDialog(int id,Bundle bundle) {
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            //Remove fragment else it will crash as it is already added to backstack
            Fragment prev = FragmentManager.FindFragmentByTag("dialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }

            ft.AddToBackStack(null);
            // Create and show the dialog.


            //Add fragment

            Task wTask = db.queryTaskById(id);

            int hour = wTask.Date.Hour;
            int minute = wTask.Date.Minute;

            Bundle taskdata = new Bundle();
            taskdata.PutInt("id", id);
            taskdata.PutInt("hour",  hour);
            taskdata.PutInt("minute", minute);

            ReminderTimeDialog timeDialog = ReminderTimeDialog.NewInstance(taskdata);
            timeDialog.Arguments = taskdata;
            timeDialog.SetStyle(DialogFragmentStyle.NoTitle, 0);//TODO: Create own theme and style
            timeDialog.Show(ft, "dialog");
        }

    public void openDateDialog(int id, Bundle bundle)
    {
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            //Remove fragment else it will crash as it is already added to backstack
            Fragment prev = FragmentManager.FindFragmentByTag("dialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }

            ft.AddToBackStack(null);
            // Create and show the dialog.


            //Add fragment


            Task wTask = db.queryTaskById(id);

            int day = wTask.Date.Day;
            int month = wTask.Date.Month;
            int year = wTask.Date.Year;

            Bundle taskdata = new Bundle();
            taskdata.PutInt("id", id);
            taskdata.PutInt("day", day);
            taskdata.PutInt("month", month);
            taskdata.PutInt("year", year);

            ReminderDateDialog dateDialog = ReminderDateDialog.NewInstance(taskdata);
            dateDialog.Arguments = taskdata;
            dateDialog.SetStyle(DialogFragmentStyle.NoTitle, 0);//TODO: Create own theme and style
            dateDialog.Show(ft, "dialog");
  
        }
    }
}

