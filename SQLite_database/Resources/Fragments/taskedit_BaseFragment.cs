using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;


namespace SQLite_database
{
    public class taskedit_BaseFragment : Fragment//,taskedit_ReminderDateDialog.OnNewDatePass,taskedit_ReminderTimeDialog.OnNewTimePass
    {

        public interface OnNewTaskDataPass
        {
            void onNewTextPass(String text, int id);
            void onNewNamePass(String name, int id);
            void doDeleteTask(int id);
            void openDateDialog(int id, Bundle bundle);
        }

        public int _id = -1;

        public OnNewTaskDataPass dataPasser
        {
            get; set;
        }


        public override void OnAttach(Activity a)//COMPAT: do one with context as well for API 23
        {
            base.OnAttach(a);

            dataPasser = (OnNewTaskDataPass)a;
            Console.WriteLine("activity casted to data passer");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment_edit, container, false);//end potentially true

            EditText edtName = view.FindViewById<EditText>(Resource.Id.ActivityChangeShowNameField);
            EditText edtText = view.FindViewById<EditText>(Resource.Id.ActivityChangeShowTextField);

            String argname = Arguments.GetString("tskname");
            String argtext = Arguments.GetString("tsktext");
            _id = Arguments.GetInt("id");

            edtName.Text = argname;
            edtText.Text = argtext;

            Button buttonReminder = view.FindViewById<Button>(Resource.Id.ButtonSetReminder);
            Button buttonExit = view.FindViewById<Button>(Resource.Id.ButtonSaveEdit);
            Button buttonDelete = view.FindViewById<Button>(Resource.Id.ButtonDelete);

            var viewToHide = this.Activity.FindViewById<LinearLayout>(Resource.Id.ToHide);

            viewToHide.Visibility = ViewStates.Gone;

            buttonReminder.Click += delegate{
                Log.Info("GOING INTO REMINDER EDIT","yup");
                if (_id > -1)
                {
                    Console.WriteLine("OPENING DATE DIALOG");
                    dataPasser.openDateDialog(_id, savedInstanceState);
                }else
                {
                    Log.Info("Somehow no edit element selected","yup");
                }
            };

            buttonDelete.Click += delegate {
                Log.Info("DELETING THAT HOMEBOY", "yup");
                if (_id > -1)
                {
                    dataPasser.doDeleteTask(_id);
                }
                FragmentTransaction transaction = FragmentManager.BeginTransaction();

                transaction.Remove(this);
                transaction.AddToBackStack("moved back to original");

                transaction.Commit();

                var viewToShow = this.Activity.FindViewById<LinearLayout>(Resource.Id.ToHide);

                viewToShow.Visibility = ViewStates.Visible;
            };
            buttonExit.Click += delegate {
                if (_id > -1) {
                    dataPasser.onNewNamePass(edtName.Text, _id);
                    dataPasser.onNewTextPass(edtText.Text, _id);
                     }
                FragmentTransaction transaction = FragmentManager.BeginTransaction();

                transaction.Remove(this);
                transaction.AddToBackStack("moved back to original");

                transaction.Commit();

                var viewToShow = this.Activity.FindViewById<LinearLayout>(Resource.Id.ToHide);

                viewToShow.Visibility = ViewStates.Visible;
            };

            return view;


        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public void goToNext(View view, Bundle savedInstanceState)
        {
            //get data 

            var TextField = view.FindViewById<EditText>(Resource.Id.ActivityTextField);
            String gotData = TextField.Text;
            //work with data entered here
            Console.WriteLine("Got data");
            Console.WriteLine(gotData);
            //dataPasser.onTextPass(gotData);
            //end get data
            FragmentTransaction transaction = this.FragmentManager.BeginTransaction();

            DateFragment thirdFragment = new DateFragment();
            //firstFragment.Show(transaction,"dialog fragment");            
            transaction.Replace(Resource.Id.RootElement, thirdFragment, "TAG_FRAGMENT");
            thirdFragment.OnCreate(savedInstanceState);
            transaction.AddToBackStack("move to third pane");
            transaction.Commit();

 

        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();

            var viewToShow = this.Activity.FindViewById<LinearLayout>(Resource.Id.ToHide);

            viewToShow.Visibility = ViewStates.Visible;
        }
    }
}